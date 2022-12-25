using System.Numerics;
using System.Security.Cryptography;
using Data;
using Data.Entity;
using Domain.Helpers;
using Domain.Services;

namespace EVoting6.Web.Services;

public class ClientVotingService
{
    private DataProviderService _dataProviderService;

    private BlumBlumShub _bbs;

    private VotingCenter _votingCenter;

    public ClientVotingService(DataProviderService dps, VotingCenter vc)
    {
        _dataProviderService = dps;
        _votingCenter = vc;
        
        _bbs = new BlumBlumShub();
    }

    public byte[] CreateMessage(string token, int candidateId)
    {
        var data = TokenHelper.GetTokenData(token);

        var bulletinId = _dataProviderService.GenerateBulletinId(data.id, candidateId);

        _bbs.ImportParams(new BigInteger(data.data.P), new BigInteger(data.data.Q));

        var bit = _bbs.GetBit();

        var rsa = new RSACryptoServiceProvider();
        rsa.ImportParameters(data.data);

        var encrypted = rsa.Encrypt(BitConverter.GetBytes(bulletinId), RSAEncryptionPadding.Pkcs1);

        var msg = encrypted.Concat(BitConverter.GetBytes(bit)).ToArray();
        msg = msg.Concat(BitConverter.GetBytes(data.id)).ToArray();

        return _votingCenter.Encrypt(msg);
    }

    public void SendBulletin(byte[] msg)
    {
        _votingCenter.AcceptVote(msg);
    }

    public Dictionary<string, int> GetResults()
    {
        var res = _votingCenter.ComputeResults();
        
        return res.Select(r => new 
            {
                Score = r.Value,
                CandidateName = _dataProviderService.GetAllCandidates().FirstOrDefault(c => c.Id == r.Key)?.Name
            })
            .ToDictionary(e => e.CandidateName, e => e.Score);
    }

    public List<Candidate> GetCandidates()
    {
        return _dataProviderService.GetAllCandidates();
    }
}