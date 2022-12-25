using System.Text.Json.Serialization;

namespace Data.Entity;

[Serializable]
public class Candidate
{
    [JsonInclude]
    public int Id { get; set; }
    
    [JsonInclude]
    public string Name { get; set; } = "anonymous";
    
    public override string ToString()
    {
        return $"{Id}: {Name}";
    }
}