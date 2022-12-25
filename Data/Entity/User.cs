using System.Security.Cryptography;

namespace Data.Entity;

public class User
{
    public int Id { get; set; }
    
    public bool CanVote { get; set; }
}