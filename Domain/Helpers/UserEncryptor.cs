using System.Security.Cryptography;

namespace Domain.Helpers;

public class UserEncryptor
{
    private static RSA rsa = RSA.Create();

    private static RSA rsa2 = RSA.Create(2048 + 512);
    
    public static byte[] Encrypt(byte[] msg)
    {
        return rsa.Encrypt(msg, RSAEncryptionPadding.Pkcs1);
    }
    
    public static byte[] Decrypt(byte[] msg)
    {
        return rsa.Decrypt(msg, RSAEncryptionPadding.Pkcs1);
    }
    
    public static byte[] Encrypt2(byte[] msg)
    {
        return rsa2.Encrypt(msg, RSAEncryptionPadding.Pkcs1);
    }
    
    public static byte[] Decrypt2(byte[] msg)
    {
        return rsa2.Decrypt(msg, RSAEncryptionPadding.Pkcs1);
    }
}