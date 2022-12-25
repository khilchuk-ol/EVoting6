using System.Security.Cryptography;
using Data;
using Domain.Helpers;

namespace Domain.Services;

public class VotingCenter
{
    private Dictionary<int, RSAParameters> _votersKeys = new();

    private List<byte[]> bulletins = new();

    private RSA rsa = RSA.Create(2048);

    private DataProviderService _dataProviderService;

    public VotingCenter(DataProviderService dps)
    {
        _dataProviderService = dps;
    }

    public void GenerateKeys(List<int> ids)
    {
        foreach (var id in ids)
        {
            if (_votersKeys.ContainsKey(id))
            {
                throw new Exception("not unique id");
            }

            var rsa = RSA.Create();
            
            _votersKeys.Add(id, rsa.ExportParameters(true));
        }
    }

    public List<string> GetTokens()
    {
        return _votersKeys.Select(pair => TokenHelper.GenerateToken(pair.Key, pair.Value)).ToList();
    }

    public void AcceptVote(byte[] msg)
    {
        bulletins.Add(msg);
    }

    public IDictionary<int, int> ComputeResults()
    {
        var ids = new HashSet<int>();
        var res = new Dictionary<int, int>();

        foreach (var msg in bulletins)
        {
            var (id, candidate) = GetVoteResult(msg);

            if (ids.Contains(id)) throw new Exception("user already voted");

            if (res.ContainsKey(candidate))
            {
                res[candidate]++;
            }
            else
            {
                res.Add(candidate, 1);
            }
        }

        return res;
    }

    private (int id, int candidate) GetVoteResult(byte[] msg)
    {
        var decrypted = Decrypt(msg);

        var bbs = new BlumBlumShub();
        var bit = BitConverter.GetBytes(bbs.GetBit());

        if (bit.Except(decrypted).Any())
        {
            throw new Exception("mismatch");
        }

        var pos1 = -1;
        var pos2 = -1;

        for (int i = 0; i < decrypted.Length; i++)
        {
            if (decrypted.Skip(i).Take(bit.Length).ToArray().Equals(bit))
            {
                pos1 = i;
                pos2 = i + bit.Length;

                break;
            }
        }

        if (pos1 == -1 || pos2 == -1)
        {
            throw new Exception("mismatch");
        }
        
        var id = BitConverter.ToInt32(decrypted.Skip(pos2).ToArray());

        if (!_votersKeys.ContainsKey(id))
        {
            throw new Exception("invalid id");
        }

        var rsa = new RSACryptoServiceProvider();
        rsa.ImportParameters(_votersKeys[id]);

        var bulletinEncrypted = decrypted.Take(pos1).ToArray();
        var bulletinDecrypted = rsa.Decrypt(bulletinEncrypted, RSAEncryptionPadding.Pkcs1);

        var bulletin = _dataProviderService.UnwrapBulletinId(BitConverter.ToInt32(bulletinDecrypted));

        return (bulletin.UserId, bulletin.CandidateId);
    }

    public byte[] Encrypt(byte[] msg)
    {
        return rsa.Encrypt(msg, RSAEncryptionPadding.Pkcs1);
    }
    
    public byte[] Decrypt(byte[] msg)
    {
        return rsa.Decrypt(msg, RSAEncryptionPadding.Pkcs1);
    }
}