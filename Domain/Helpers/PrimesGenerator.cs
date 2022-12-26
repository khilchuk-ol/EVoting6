namespace Domain.Helpers;

public class PrimesGenerator
{
    public static List<int> GeneratePrimesNaive(int n, int firstPrime, int secondPrime)
    {
        var primes = new List<int>();
        primes.Add(firstPrime);
        var nextPrime = secondPrime;
        while (primes.Count < n)
        {
            var sqrt = (int)Math.Sqrt(nextPrime);
            var isPrime = true;
            for (var i = 0; primes[i] <= sqrt; i++)
            {
                if (nextPrime % primes[i] == 0)
                {
                    isPrime = false;
                    break;
                }
            }
            if (isPrime)
            {
                primes.Add(nextPrime);
            }
            nextPrime += 2;
        }
        return primes;
    }
}