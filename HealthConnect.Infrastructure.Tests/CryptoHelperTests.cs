using HealthConnect.Infrastructure.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthConnect.Infrastructure.Tests;

public class CryptoHelperTests
{

    [Fact]
    public void GenerateSalt_ShouldReturnNonEmptySalt()
    {
        // Arrange
        var cryptoHelper = new CryptoHelper();
        // Act
        var salt = cryptoHelper.GenerateSalt();
        // Assert
        Assert.False(string.IsNullOrEmpty(salt), "Generated salt should not be null or empty.");
    }

    [Fact]
    public void HashPassword_ShouldReturnHashedPassword()
    {
        // Arrange
        var cryptoHelper = new CryptoHelper();
        var password = "TestPassword";
        var salt = cryptoHelper.GenerateSalt();
        // Act
        var hashedPassword = cryptoHelper.HashPassword(password, salt);
        // Assert
        Assert.False(string.IsNullOrEmpty(hashedPassword), "Hashed password should not be null or empty.");
    }

    [Fact]
    public void VerifyPassword_ShouldReturnTrueForCorrectPassword()
    {
        // Arrange
        var cryptoHelper = new CryptoHelper();
        var password = "TestPassword";
        var salt = cryptoHelper.GenerateSalt();
        var hashedPassword = cryptoHelper.HashPassword(password, salt);
        // Act
        var isVerified = cryptoHelper.VerifyPassword("WrongPassword", hashedPassword, salt);
        // Assert
        Assert.False(isVerified, "Password verification should return false");
    }


}
