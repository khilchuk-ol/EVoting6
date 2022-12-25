namespace Domain.Helpers;

public static class RandomStringGenerator
{
    private static Random random = new();

    private const int LENGTH = 8;
    
    public static string Generate()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, LENGTH)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}