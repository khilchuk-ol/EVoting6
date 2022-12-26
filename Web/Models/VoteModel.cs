using System.Text.Json.Serialization;

namespace EVoting6.Web.Models;

[Serializable]
public class VoteModel
{
    [JsonInclude]
    public int Candidate { get; set; }
}