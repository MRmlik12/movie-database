using Shouldly;
using MovieDatabase.Api.Application.Users.AuthenticateUser;
using MovieDatabase.Api.Core.Documents.Users;
using MovieDatabase.Api.Core.Exceptions.Users;
using MovieDatabase.Api.Core.Services;
using MovieDatabase.Api.Core.Utils;
using MovieDatabase.Api.Infrastructure.Db.Repositories;
using MovieDatabase.UnitTests.Helpers;
using NSubstitute;

namespace MovieDatabase.UnitTests.Application.Users;

public class AuthenticateUserRequestHandlerTests
{
    private readonly IUserRepository _mockUserRepository;
    private readonly IJwtService _mockJwtService;
    private readonly AuthenticateUserRequestHandler _handler;

    public AuthenticateUserRequestHandlerTests()
    {
        _mockUserRepository = Substitute.For<IUserRepository>();
        _mockJwtService = Substitute.For<IJwtService>();
        _handler = new AuthenticateUserRequestHandler(_mockUserRepository, _mockJwtService);
    }

    [Fact]
    public async Task HandleAsync_WithValidCredentials_ShouldReturnUserCredentials()
    {
        const string password = "TestPassword123!";
        var passwordHash = PasswordUtils.HashPassword(password);
        var user = TestDataBuilder.CreateValidUser(
            email: "test@example.com",
            passwordHash: passwordHash
        );

        var request = new AuthenticateUserRequest(
            user.Email,
            password
        );

        const string expectedToken = "jwt-token-12345";
        var expectedExpireTime = DateTime.UtcNow.AddHours(1);

        _mockUserRepository.GetByEmail(request.Email)
            .Returns(Task.FromResult<User?>(user));
        _mockJwtService.GenerateJwtToken(user)
            .Returns((expectedToken, expectedExpireTime));

        var result = await _handler.HandleAsync(request);

        result.ShouldNotBeNull();
        result.Token.ShouldBe(expectedToken);
        result.ExpireTime.ShouldBe(expectedExpireTime);
        result.Email.ShouldBe(user.Email);
        result.Username.ShouldBe(user.Name);
        result.Role.ShouldBe(Enum.GetName(user.Role));
    }

    [Fact]
    public async Task HandleAsync_WithInvalidEmail_ShouldThrowInvalidUserCredentialsException()
    {
        var request = new AuthenticateUserRequest(
            "nonexistent@example.com",
             "SomePassword123!"
        );

        _mockUserRepository.GetByEmail(request.Email)
            .Returns(Task.FromResult<User?>(null));

        await Assert.ThrowsAsync<InvalidUserCredentialsApplicationException>(
            () => _handler.HandleAsync(request)
        );

        _mockJwtService.DidNotReceive().GenerateJwtToken(Arg.Any<User>());
    }

    [Fact]
    public async Task HandleAsync_WithInvalidPassword_ShouldThrowInvalidUserCredentialsException()
    {
        const string correctPassword = "CorrectPassword123!";
        const string incorrectPassword = "WrongPassword456!";
        var passwordHash = PasswordUtils.HashPassword(correctPassword);

        var user = TestDataBuilder.CreateValidUser(
            email: "test@example.com",
            passwordHash: passwordHash
        );

        var request = new AuthenticateUserRequest(
            Email: user.Email,
            Password: incorrectPassword
        );

        _mockUserRepository.GetByEmail(request.Email)
            .Returns(Task.FromResult<User?>(user));

        await Assert.ThrowsAsync<InvalidUserCredentialsApplicationException>(
            () => _handler.HandleAsync(request)
        );

        _mockJwtService.DidNotReceive().GenerateJwtToken(Arg.Any<User>());
    }

    [Fact]
    public async Task HandleAsync_ShouldGenerateJwtToken()
    {
        const string password = "TestPassword123!";
        var passwordHash = PasswordUtils.HashPassword(password);
        var user = TestDataBuilder.CreateValidUser(passwordHash: passwordHash);

        var request = new AuthenticateUserRequest(
            Email: user.Email,
            Password: password
        );

        _mockUserRepository.GetByEmail(request.Email)
            .Returns(Task.FromResult<User?>(user));
        _mockJwtService.GenerateJwtToken(user)
            .Returns(("token", DateTime.UtcNow.AddHours(1)));

        await _handler.HandleAsync(request);

        _mockJwtService.Received(1).GenerateJwtToken(Arg.Is<User>(u =>
            u.Id == user.Id &&
            u.Email == user.Email &&
            u.Name == user.Name));
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnTokenWithExpirationTime()
    {
        const string password = "TestPassword123!";
        var passwordHash = PasswordUtils.HashPassword(password);
        var user = TestDataBuilder.CreateValidUser(passwordHash: passwordHash);

        var request = new AuthenticateUserRequest(
            Email: user.Email,
            Password: password
        );

        var expectedToken = "jwt-token-abc123";
        var expectedExpireTime = DateTime.UtcNow.AddMinutes(60);

        _mockUserRepository.GetByEmail(request.Email)
            .Returns(Task.FromResult<User?>(user));
        _mockJwtService.GenerateJwtToken(user)
            .Returns((expectedToken, expectedExpireTime));

        var result = await _handler.HandleAsync(request);

        result.Token.ShouldBe(expectedToken);
        result.ExpireTime.Value.ShouldBe(expectedExpireTime, TimeSpan.FromSeconds(1));
    }

    [Theory]
    [InlineData(UserRoles.User)]
    [InlineData(UserRoles.Moderator)]
    [InlineData(UserRoles.Administrator)]
    public async Task HandleAsync_WithDifferentUserRoles_ShouldReturnCorrectRole(UserRoles role)
    {
        var password = "TestPassword123!";
        var passwordHash = PasswordUtils.HashPassword(password);
        var user = TestDataBuilder.CreateValidUser(
            passwordHash: passwordHash,
            role: role
        );

        var request = new AuthenticateUserRequest(
            Email: user.Email,
            Password: password
        );

        _mockUserRepository.GetByEmail(request.Email)
            .Returns(Task.FromResult<User?>(user));
        _mockJwtService.GenerateJwtToken(user)
            .Returns(("token", DateTime.UtcNow.AddHours(1)));

        var result = await _handler.HandleAsync(request);

        result.Role.ShouldBe(Enum.GetName(role));
    }

    [Fact]
    public async Task HandleAsync_WithEmptyPassword_ShouldThrowInvalidUserCredentialsException()
    {
        var user = TestDataBuilder.CreateValidUser();
        var request = new AuthenticateUserRequest(
            Email: user.Email,
            Password: string.Empty
        );

        _mockUserRepository.GetByEmail(request.Email)
            .Returns(Task.FromResult<User?>(user));

        await Assert.ThrowsAsync<InvalidUserCredentialsApplicationException>(
            () => _handler.HandleAsync(request)
        );
    }

    [Fact]
    public async Task HandleAsync_ShouldCallRepositoryGetByEmailOnce()
    {
        const string password = "TestPassword123!";
        var passwordHash = PasswordUtils.HashPassword(password);
        var user = TestDataBuilder.CreateValidUser(passwordHash: passwordHash);

        var request = new AuthenticateUserRequest(
            Email: user.Email,
            Password: password
        );

        _mockUserRepository.GetByEmail(request.Email)
            .Returns(Task.FromResult<User?>(user));
        _mockJwtService.GenerateJwtToken(user)
            .Returns(("token", DateTime.UtcNow.AddHours(1)));

        await _handler.HandleAsync(request);

        await _mockUserRepository.Received(1).GetByEmail(request.Email);
    }
}

