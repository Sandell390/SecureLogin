using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace SecureLogin;

public static class Hashing
{
    // Hash a password with the PBKDF2 algorithm. Returns a 24-byte salt.
    public static string GenerateSalt()
    {
        // Generate a 24-byte salt using a cryptographically strong random sequence of nonzero values.
        byte[] saltBytes = new byte[24];
        using (RandomNumberGenerator rngCsp = RandomNumberGenerator.Create())
        {
            rngCsp.GetNonZeroBytes(saltBytes);
        }

        byte[] hashData = SHA256.HashData(saltBytes);
        
        return Convert.ToHexString(hashData).ToLower();
    }
    
    // Verify a password against a hash.
    public static bool VerifyPassword(string hashedDBpassword, string salt, string hash)
    {
        string decodedDBpassword = hashedDBpassword.Substring(0, 64);
        
        string decodedPassword = hash.Substring(0, 64);
        string decodedSalt = hash.Substring(64, 64);
        
        return decodedSalt == salt && decodedPassword == decodedDBpassword;
    }
}