using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace SecureLogin;

public static class Hashing
{
    // Hash a password with the PBKDF2 algorithm. Returns a 24-byte pepper.
    public static string GeneratePepper()
    {
        // Generate a 24-byte salt using a cryptographically strong random sequence of nonzero values.
        byte[] pepperBytes = new byte[24];
        using (RandomNumberGenerator rngCsp = RandomNumberGenerator.Create())
        {
            rngCsp.GetNonZeroBytes(pepperBytes);
        }

        byte[] hashData = SHA256.HashData(pepperBytes);
        
        return Convert.ToHexString(hashData).ToLower();
    }
}