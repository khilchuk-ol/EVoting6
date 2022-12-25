using Data.Entity;
using Domain.Services;

namespace EVoting6.Web.Services;

public class UserService
{
    private RegistrationBureau _registrationBureau;

    private VotingCenter _votingCenter;

    public UserService(RegistrationBureau rb, VotingCenter vc)
    {
        _registrationBureau = rb;
        _votingCenter = vc;
    }

    public (Voter, string) Register(int ipn)
    {
        return _registrationBureau.RegisterVoter(ipn);
    }

    public string LogIn(string login, string password)
    {
        return _registrationBureau.GetToken(login, password);
    }
}