using Data.DataSource;
using Data.Entity;

namespace Data;

public class DataProviderService
{
    private InMemoryDataSource source;

    public DataProviderService(InMemoryDataSource dataSource)
    {
        source = dataSource;
    }

    public List<Candidate> GetAllCandidates() => source.Candidates;
    
    public User? GetUserById(int id) => source.Users.FirstOrDefault(u => u.Id == id);
    
    public Candidate? GetCandidateById(int id) => source.Candidates.FirstOrDefault(c => c.Id == id);

    public int GenerateBulletinId(int userId, int candidateId) => source.GenerateBulletinId(userId, candidateId);

    public (int UserId, int CandidateId) UnwrapBulletinId(int id) => source.UnwrapBulletinId(id);
    
    public List<User> GetVoters() => source.Users.FindAll(u => u.CanVote).ToList();
}