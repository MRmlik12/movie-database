using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using MovieDatabase.Api.Core.Documents.Users;
using MovieDatabase.Api.Core.Jwt;
using MovieDatabase.Api.Core.Services;
using Shouldly;

namespace MovieDatabase.UnitTests.Core.Services;

public class JwtServiceTests
{
    private readonly JwtSettings _jwtSettings;
    private readonly JwtService _jwtService;

    public JwtServiceTests()
    {
        _jwtSettings = new JwtSettings
        {
            Issuer = "TestIssuer",
            Audience = "TestAudience",
            Key = "ThisIsAVerySecureKeyThatIsAtLeast32CharactersLong!",
            ExpirationMinutes = 60
        };

        var options = Options.Create(_jwtSettings);
        _jwtService = new JwtService(options);
    }

    [Fact]
    public void GenerateJwtToken_ShouldReturnValidToken()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "TestUser",
            Email = "test@example.com",
            Role = UserRoles.User
        };

        // Act
        var (token, expireDate) = _jwtService.GenerateJwtToken(user);

        // Assert
        token.ShouldNotBeNullOrEmpty();
        expireDate.ShouldBeGreaterThan(DateTime.UtcNow);
    }

    [Fact]
    public void GenerateJwtToken_ShouldIncludeUserClaims()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            Name = "TestUser",
            Email = "test@example.com",
            Role = UserRoles.User
        };

        // Act
        var (token, _) = _jwtService.GenerateJwtToken(user);

        // Assert
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        jwtToken.Claims.ShouldContain(c => c.Type == JwtRegisteredClaimNames.Jti && c.Value == userId.ToString());
        jwtToken.Claims.ShouldContain(c => c.Type == JwtRegisteredClaimNames.Sub && c.Value == "TestUser");
        jwtToken.Claims.ShouldContain(c => c.Type == JwtRegisteredClaimNames.Email && c.Value == "test@example.com");
        jwtToken.Claims.ShouldContain(c => c.Type == JwtExtendedClaimTypes.Kid);
        jwtToken.Claims.ShouldContain(c => c.Type == ClaimTypes.Role && c.Value == "User");
    }

    [Fact]
    public void GenerateJwtToken_ShouldSetCorrectExpirationTime()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "TestUser",
            Email = "test@example.com",
            Role = UserRoles.User
        };
        var beforeGeneration = DateTime.UtcNow;

        // Act
        var (_, expireDate) = _jwtService.GenerateJwtToken(user);

        // Assert
        var afterGeneration = DateTime.UtcNow;
        var expectedExpiration = beforeGeneration.AddMinutes(_jwtSettings.ExpirationMinutes);
        
        expireDate.ShouldBe(expectedExpiration, TimeSpan.FromSeconds(5));
        expireDate.ShouldBeGreaterThan(afterGeneration);
    }

    [Fact]
    public void GenerateJwtToken_TokenShouldBeDecodable()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "TestUser",
            Email = "test@example.com",
            Role = UserRoles.User
        };

        // Act
        var (token, _) = _jwtService.GenerateJwtToken(user);

        // Assert
        var handler = new JwtSecurityTokenHandler();
        var canRead = handler.CanReadToken(token);
        canRead.ShouldBeTrue();

        var jwtToken = handler.ReadJwtToken(token);
        jwtToken.ShouldNotBeNull();
        jwtToken.Issuer.ShouldBe(_jwtSettings.Issuer);
        jwtToken.Audiences.ShouldContain(_jwtSettings.Audience);
    }

    [Fact]
    public void GenerateJwtToken_ShouldIncludeRoleClaim()
    {
        // Arrange
        var testCases = new[]
        {
            (UserRoles.User, "User"),
            (UserRoles.Moderator, "Moderator"),
            (UserRoles.Administrator, "Administrator")
        };

        // Act
        foreach (var (role, expectedRoleName) in testCases)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = "TestUser",
                Email = "test@example.com",
                Role = role
            };

            var (token, _) = _jwtService.GenerateJwtToken(user);

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            // Assert
            jwtToken.Claims.ShouldContain(c => 
                c.Type == ClaimTypes.Role && 
                c.Value == expectedRoleName,
                $"token should contain role claim for {expectedRoleName}");
        }
    }

    [Fact]
    public void GenerateJwtToken_ShouldGenerateDifferentTokensForSameUser()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "TestUser",
            Email = "test@example.com",
            Role = UserRoles.User
        };

        // Act
        var (token1, _) = _jwtService.GenerateJwtToken(user);
        Thread.Sleep(10);
        var (token2, _) = _jwtService.GenerateJwtToken(user);

        // Assert
        token1.ShouldNotBe(token2, "each token generation should produce a unique token");
    }

    [Fact]
    public void GenerateJwtToken_WithDifferentUsers_ShouldGenerateDifferentTokens()
    {
        // Arrange
        var user1 = new User
        {
            Id = Guid.NewGuid(),
            Name = "User1",
            Email = "user1@example.com",
            Role = UserRoles.User
        };

        var user2 = new User
        {
            Id = Guid.NewGuid(),
            Name = "User2",
            Email = "user2@example.com",
            Role = UserRoles.Administrator
        };

        // Act
        var (token1, _) = _jwtService.GenerateJwtToken(user1);
        var (token2, _) = _jwtService.GenerateJwtToken(user2);

        // Assert
        token1.ShouldNotBe(token2);

        var handler = new JwtSecurityTokenHandler();
        var jwtToken1 = handler.ReadJwtToken(token1);
        var jwtToken2 = handler.ReadJwtToken(token2);

        jwtToken1.Subject.ShouldBe("User1");
        jwtToken2.Subject.ShouldBe("User2");
    }
}
