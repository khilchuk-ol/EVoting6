using Data;
using Data.Entity;
using Domain.Helpers;

namespace Domain.Services;

public class RegistrationBureau
{
    private DataProviderService _dataProvider;

    private Dictionary<int, string> _tokens = new();

    private Dictionary<int, Voter> _voters = new();

    public RegistrationBureau(DataProviderService dps, VotingCenter vc)
    {
        _dataProvider = dps;

        var ids = GenerateIds();
        vc.GenerateKeys(ids);
        
        SaveTokens(vc.GetTokens());
    }

    private List<int> GenerateIds()
    {
        var count = _dataProvider.GetVoters().Count;

        var rnd = new Random();
        
        return Enumerable.Range(0, count)
            .Select(i => new Tuple<int, int>(rnd.Next(count), i))
            .OrderBy(i => i.Item1)
            .Select(i => i.Item2)
            .ToList();
    }

    private void SaveTokens(List<string> tokens)
    {
        var i = 1;
        
        tokens.ForEach(t => _tokens.Add(i++, t));
    }

    public (Voter voter, string token) RegisterVoter(int ipn)
    {
        var u = _dataProvider.GetUserById(ipn);
        if (u == null || !u.CanVote)
        {
            throw new Exception("user cannot vote");
        }

        var pair = _voters.FirstOrDefault(p => p.Value.Id == ipn);
        if (!EqualityComparer<KeyValuePair<int, Voter>>.Default.Equals(pair, default))
        {
            return (pair.Value, _tokens[pair.Key]);
        }

        var voter = new Voter();
        voter.Login = RandomStringGenerator.Generate();
        voter.Password = RandomStringGenerator.Generate();
        voter.Id = ipn;

        var token = _tokens.First(p => !_voters.ContainsKey(p.Key));
        
        _voters.Add(token.Key, voter);

        return (voter, token.Value);
    }

    public string GetToken(string login, string password)
    {
        var pair = _voters.FirstOrDefault(p => p.Value.Login == login && p.Value.Password == password);
        if (EqualityComparer<KeyValuePair<int, Voter>>.Default.Equals(pair, default))
        {
            throw new Exception("invalid credentials");
        }

        return _tokens[pair.Key];
    }
}