namespace HealthConnect.Application.Interfaces;

/// <summary>
/// Provides methods for generating salts and hashing passwords.
/// </summary>
public interface IPasswordHasher
{
    /// <summary>
    /// Generates a cryptographically secure salt for password hashing.
    /// </summary>
    /// <returns>A base64-encoded salt string.</returns>
    string GenerateSalt();

    /// <summary>
    /// Hashes the specified password using the provided salt.
    /// </summary>
    /// <param name="password">The password to hash.</param>
    /// <param name="salt">The salt to use for hashing.</param>
    /// <returns>The hashed password as a string.</returns>
    string HashPassword(string password, string salt);
}
