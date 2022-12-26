using Data;
using Domain.Helpers;

namespace Domain.Services;

public class VotingCenter
{
    private Dictionary<int, (int p, int q)> _votersKeys = new();

    private List<byte[]> bulletins = new();

    private DataProviderService _dataProviderService;

    public VotingCenter(DataProviderService dps)
    {
        _dataProviderService = dps;
    }

    public void GenerateKeys(List<int> ids)
    {
        var primes = PrimesGenerator.GeneratePrimesNaive(ids.Count * 2, 3, 5);
        var pos = 0;
        
        foreach (var id in ids)
        {
            if (_votersKeys.ContainsKey(id))
            {
                throw new Exception("not unique id");
            }
            
            _votersKeys.Add(id, (primes[pos++], primes[pos++]));
        }
    }

    public List<string> GetTokens()
    {
        return _votersKeys.Select(pair => TokenHelper.GenerateToken(pair.Key, pair.Value.p * pair.Value.q)).ToList();
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

            if (ids.Contains(id)) continue; // voter already voted

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
        var decrypted = UserEncryptor.Decrypt2(msg);
        var decryptedWithoutId = decrypted;

        var keys = (-1, -1);

        foreach (var pair in _votersKeys)
        {
            var idBytes = BitConverter.GetBytes(pair.Key);

            var last = decrypted.TakeLast(idBytes.Length);

            if (last.SequenceEqual(idBytes))
            {
                keys = pair.Value;

                decryptedWithoutId = decrypted.SkipLast(idBytes.Length).ToArray();
                
                break;
            }
        }

        if (keys.Item1 == -1 || keys.Item2 == -1)
        {
            throw new Exception("voter not found");
        }

        var bit = BitConverter.GetBytes(BlumBlumShub.GetBit(keys.Item1 * keys.Item2));

        if (!decryptedWithoutId.TakeLast(bit.Length).SequenceEqual(bit))
        {
            throw new Exception("mismatch");
        }

        var bulletinEncrypted = decryptedWithoutId.SkipLast(bit.Length).ToArray();
        var bulletinDecrypted = UserEncryptor.Decrypt(bulletinEncrypted);

        var bulletin = _dataProviderService.UnwrapBulletinId(BitConverter.ToInt32(bulletinDecrypted));

        return (bulletin.UserId, bulletin.CandidateId);
    }
}