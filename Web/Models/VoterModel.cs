using System.Text.Json.Serialization;

namespace EVoting6.Web.Models;

[Serializable]
public class VoterModel
{
    [JsonInclude] 
    public string Token { get; set; } = "";
    
    [JsonInclude]
    public string Login { get; set; }
    
    [JsonInclude]
    public string Password { get; set; }
}