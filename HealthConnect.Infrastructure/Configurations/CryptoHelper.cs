using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HealthConnect.Infrastructure.Configurations;

public static class CryptoHelper
{
    public static string GenerateSalt()
    {
        byte[] salt = new byte[128 / 8];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        if (salt == null || salt.Length == 0)
        {
            throw new InvalidOperationException("Failed to generate salt.");
        }

        return Convert.ToBase64String(salt);
    }

    public static string HashPassword(string password, string salt)
    {
        if (string.IsNullOrEmpty(password))
        {
            throw new ArgumentException("Password cannot be null or empty.", nameof(password));
        }

        if (string.IsNullOrEmpty(salt))
        {
            throw new ArgumentException("Salt cannot be null or empty.", nameof(salt));
        }

        byte[] saltBytes = Convert.FromBase64String(salt);

        byte[] hash = KeyDerivation.Pbkdf2(
            password: password,
            salt: saltBytes,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8);

        return Convert.ToBase64String(hash);
    }

    public static bool VerifyPassword(string password, string storedHash, string storedSalt)
    {
        var hashedPassword = HashPassword(password, storedSalt);
        return hashedPassword == storedHash;
    }

}
