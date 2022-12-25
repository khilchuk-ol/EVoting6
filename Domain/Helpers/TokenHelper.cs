using System.Security.Cryptography;

namespace Domain.Helpers;

public static class TokenHelper
{
    private static string _separator = "_";
    
    public static String GenerateToken(int id, RSAParameters data)
    {
        var n = BitConverter.ToInt64(data.Modulus);
        var e = BitConverter.ToInt64(data.Exponent);
        
        return String.Join(_separator, id, n, e);
    }

    public static (int id, RSAParameters data) GetTokenData(string token)
    {
        var pieces = token.Split(_separator);
        if (pieces.Length != 3)
        {
            throw new Exception("token exception");
        }

        var id = Int32.Parse(pieces[0]);
        
        var modulus  = Int64.Parse(pieces[1]);
        var exponent = Int64.Parse(pieces[2]);

        return (id, new RSAParameters
        {
            Modulus = BitConverter.GetBytes(modulus),
            Exponent = BitConverter.GetBytes(exponent)
        });
    }
}