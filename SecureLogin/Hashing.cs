using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace SecureLogin;

public static class Hashing
{
    // Hash a password with the PBKDF2 algorithm. Returns a 24-byte hash and a 24-byte salt.
    public static string? HashPassword(string password, out string? salt)
    {
        // Generate a 24-byte salt using a cryptographically strong random sequence of nonzero values.
        byte[] saltBytes = new byte[24];
        using (RandomNumberGenerator rngCsp = RandomNumberGenerator.Create())
        {
            rngCsp.GetNonZeroBytes(saltBytes);
            salt = Convert.ToBase64String(saltBytes);
        }

        // Hash the password and encode the parameters.
        byte[] hashBytes = KeyDerivation.Pbkdf2(
            password: password,
            salt: saltBytes,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 10000,
            numBytesRequested: 24);
        
        return Convert.ToBase64String(hashBytes);
    }
    
    // Verify a password against a hash.
    public static bool VerifyPassword(string password, string salt, string hash)
    {
        // Hash the password and encode the parameters.
        byte[] hashBytes = KeyDerivation.Pbkdf2(
            password: password,
            salt: Convert.FromBase64String(salt),
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 10000,
            numBytesRequested: 24);
        
        return Convert.ToBase64String(hashBytes) == hash;
    }
}