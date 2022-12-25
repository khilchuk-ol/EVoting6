using System.Text.Json.Serialization;

namespace EVoting6.Web.Models;

[Serializable]
public class UserRegisterModel
{
    [JsonInclude]
    public int Ipn { get; set; }
}