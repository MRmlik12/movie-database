using MovieDatabase.Api.Core.Utils;
using Shouldly;

namespace MovieDatabase.UnitTests.Core.Utils;

public class PasswordUtilsTests
{
    [Fact]
    public void HashPassword_ShouldReturnNonEmptyHash()
    {
        const string password = "TestPassword123!";

        var hashedPassword = PasswordUtils.HashPassword(password);

        hashedPassword.ShouldNotBeNullOrEmpty();
        hashedPassword.ShouldNotBe(password, "hashed password should differ from original");
    }

    [Fact]
    public void HashPassword_ShouldReturnDifferentHashesForSamePassword()
    {
        const string password = "TestPassword123!";

        var hash1 = PasswordUtils.HashPassword(password);
        var hash2 = PasswordUtils.HashPassword(password);

        hash1.ShouldNotBe(hash2, "BCrypt should use different salts for each hash");
    }

    [Fact]
    public void VerifyPassword_WithCorrectPassword_ShouldReturnTrue()
    {
        const string password = "TestPassword123!";
        var hashedPassword = PasswordUtils.HashPassword(password);

        var result = PasswordUtils.VerifyPassword(password, hashedPassword);

        result.ShouldBeTrue();
    }

    [Fact]
    public void VerifyPassword_WithIncorrectPassword_ShouldReturnFalse()
    {
        const string correctPassword = "CorrectPassword123!";
        const string incorrectPassword = "WrongPassword456!";
        var hashedPassword = PasswordUtils.HashPassword(correctPassword);

        var result = PasswordUtils.VerifyPassword(incorrectPassword, hashedPassword);

        result.ShouldBeFalse();
    }

    [Fact]
    public void VerifyPassword_WithEmptyPassword_ShouldReturnFalse()
    {
        const string password = "TestPassword123!";
        var hashedPassword = PasswordUtils.HashPassword(password);

        var result = PasswordUtils.VerifyPassword(string.Empty, hashedPassword);

        result.ShouldBeFalse();
    }

    [Theory]
    [InlineData("short")]
    [InlineData("verylongpasswordthatismorethan50characterslong12345")]
    [InlineData("P@ssw0rd!")]
    public void HashPassword_WithVariousPasswordLengths_ShouldSucceed(string password)
    {
        var hashedPassword = PasswordUtils.HashPassword(password);

        hashedPassword.ShouldNotBeNullOrEmpty();
        hashedPassword.ShouldStartWith("$2");
    }

    [Fact]
    public void HashPassword_WithSpecialCharacters_ShouldHandleCorrectly()
    {
        const string password = "P@$$w0rd!#%^&*()";

        var hashedPassword = PasswordUtils.HashPassword(password);

        hashedPassword.ShouldNotBeNullOrEmpty();
        var isValid = PasswordUtils.VerifyPassword(password, hashedPassword);
        isValid.ShouldBeTrue();
    }

    [Fact]
    public void HashPassword_WithUnicodeCharacters_ShouldHandleCorrectly()
    {
        const string password = "Пароль123!";

        var hashedPassword = PasswordUtils.HashPassword(password);

        hashedPassword.ShouldNotBeNullOrEmpty();
        var isValid = PasswordUtils.VerifyPassword(password, hashedPassword);
        isValid.ShouldBeTrue();
    }
}

