using System.Numerics;

namespace Domain.Helpers;

public class BlumBlumShub
{
    public static readonly BigInteger p = 3263849;
    public static readonly BigInteger q = 1302498943;
    public static readonly BigInteger m = p*q;

    private static BigInteger Next(BigInteger prev) {
        return prev * prev % m;
    }

    public void ImportParams(BigInteger p, BigInteger q)
    {
        var P = p;
        var Q = q;
    }

    private static int Parity(BigInteger n) {
        BigInteger q = n;
        BigInteger count = 0;
        while (q != BigInteger.Zero) {
            count += q & BigInteger.One;
            q >>= 1;
        }
        return ((count & BigInteger.One) != BigInteger.Zero) ? 1 : 0; // even parity
    }

    private static int LSB(BigInteger n) {
        return ((n & BigInteger.One) != BigInteger.Zero) ? 1 : 0;
    }

    public int GetBit()
    {
        BigInteger seed = 6367859;

        int[] bits = new int[100];

        BigInteger xprev = seed;
        for(int k = 0; k != 100; ++k) {
            BigInteger xnext = Next(xprev);
            int bit = Parity(xnext); // extract random bit from generated BBS number using parity,
            // or just int bit = LSB(xnext);

            bits[k] = bit;

            xprev = xnext;
        }

        return bits[0];
    }
}