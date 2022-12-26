namespace Domain.Helpers;

public static class TokenHelper
{
    private static string _separator = "_";
    
    public static String GenerateToken(int id, int bbsKey)
    {
        return String.Join(_separator, id, bbsKey);
    }

    public static (int id, int bbsKey) GetTokenData(string token)
    {
        var pieces = token.Split(_separator);
        if (pieces.Length != 2)
        {
            throw new Exception("token exception");
        }

        var id     = Int32.Parse(pieces[0]);
        var bbsKey = Int32.Parse(pieces[1]);

        return (id, bbsKey);
    }
}