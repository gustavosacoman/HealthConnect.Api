namespace HealthConnect.Infrastructure.Configurations;

using HealthConnect.Application.Interfaces;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

/// <summary>
/// Provides cryptographic helper methods for password hashing and verification.
/// </summary>
public class CryptoHelper : IPasswordHasher
{
    /// <summary>
    /// Generates a cryptographically secure random salt.
    /// </summary>
    /// <returns>A base64-encoded salt string.</returns>
    public string GenerateSalt()
    {
        byte[] salt = new byte[128 / 8];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        if (salt is null || salt.Length == 0)
        {
            throw new InvalidOperationException("Failed to generate salt.");
        }

        return Convert.ToBase64String(salt);
    }

    /// <summary>
    /// Hashes the specified password using the provided salt.
    /// </summary>
    /// <param name="password">The password to hash.</param>
    /// <param name="salt">The base64-encoded salt.</param>
    /// <returns>A base64-encoded password hash.</returns>
    public string HashPassword(string password, string salt)
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

    /// <summary>
    /// Verifies that the provided password matches the stored hash using the stored salt.
    /// </summary>
    /// <param name="password">The password to verify.</param>
    /// <param name="storedHash">The stored base64-encoded password hash.</param>
    /// <param name="storedSalt">The stored base64-encoded salt.</param>
    /// <returns><c>true</c> if the password matches the hash; otherwise, <c>false</c>.</returns>
    public bool VerifyPassword(string password, string storedHash, string storedSalt)
    {
        var hashedPassword = HashPassword(password, storedSalt);
        return hashedPassword == storedHash;
    }
}
