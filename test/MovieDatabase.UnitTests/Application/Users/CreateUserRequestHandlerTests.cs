using Shouldly;
using MovieDatabase.Api.Application.Users.CreateUser;
using MovieDatabase.Api.Core.Documents.Users;
using MovieDatabase.Api.Core.Exceptions.Users;
using MovieDatabase.Api.Core.Jwt;
using MovieDatabase.Api.Core.Services;
using MovieDatabase.Api.Infrastructure.Db;
using MovieDatabase.Api.Infrastructure.Db.Repositories;
using MovieDatabase.UnitTests.Helpers;
using NSubstitute;

namespace MovieDatabase.UnitTests.Application.Users;

public class CreateUserRequestHandlerTests
{
    private const int ExpireDateToleranceSeconds = 10;
    
    private readonly IUserRepository _mockUserRepository;
    private readonly IUnitOfWork _mockUnitOfWork;
    private readonly IJwtService _mockJwtService;
    private readonly CreateUserRequestHandler _handler;

    public CreateUserRequestHandlerTests()
    {
        _mockUserRepository = Substitute.For<IUserRepository>();
        _mockUnitOfWork = Substitute.For<IUnitOfWork>();
        _mockJwtService = Substitute.For<IJwtService>();
        _handler = new CreateUserRequestHandler(_mockUserRepository, _mockUnitOfWork, _mockJwtService);
    }

    [Fact]
    public async Task HandleAsync_WithValidData_ShouldCreateUser()
    {
        var request = TestDataBuilder.CreateValidCreateUserRequest();
        
        var expectedJwtCredentials = new JwtCredential("test-token", DateTime.UtcNow.AddHours(1));
        
        _mockUserRepository.GetByEmail(Arg.Any<string>())
            .Returns(Task.FromResult<User?>(null));
        _mockJwtService.GenerateJwtToken(Arg.Any<User>())
            .Returns(expectedJwtCredentials);

        var result = await _handler.HandleAsync(request);

        result.ShouldNotBeNull();
        await _mockUserRepository.Received(1).Add(Arg.Is<User>(u => 
            u.Name == request.Username && 
            u.Email == request.Email));
        await _mockUnitOfWork.Received(1).Commit();
    }

    [Fact]
    public async Task HandleAsync_WithValidData_ShouldReturnUserCredentialsWithToken()
    {
        var request = TestDataBuilder.CreateValidCreateUserRequest();
        
        var expectedJwtModel = new JwtCredential("jwt-token-12345", DateTime.UtcNow.AddHours(1));
        
        _mockUserRepository.GetByEmail(Arg.Any<string>())
            .Returns(Task.FromResult<User?>(null));
        _mockJwtService.GenerateJwtToken(Arg.Any<User>())
            .Returns(expectedJwtModel);

        var result = await _handler.HandleAsync(request);

        result.ShouldNotBeNull();
        result.Token.ShouldBe(expectedJwtModel.Token);
        result.ExpireTime?.ShouldBe(expectedJwtModel.ExpireDate, TimeSpan.FromSeconds(ExpireDateToleranceSeconds));
        result.Username.ShouldBe(request.Username);
        result.Email.ShouldBe(request.Email);
        result.Role.ShouldBe(nameof(UserRoles.User));
        await _mockUnitOfWork.Received(1).Commit();
    }

    [Fact]
    public async Task HandleAsync_WithDuplicateEmail_ShouldThrowDuplicateEmailException()
    {
        var request = TestDataBuilder.CreateValidCreateUserRequest(email: "existing@example.com");
        var existingUser = TestDataBuilder.CreateValidUser(email: "existing@example.com");

        _mockUserRepository.GetByEmail(request.Email)
            .Returns(Task.FromResult<User?>(existingUser));

        await Assert.ThrowsAsync<DuplicateEmailApplicationException>(
            () => _handler.HandleAsync(request)
        );

        await _mockUserRepository.DidNotReceive().Add(Arg.Any<User>());
        await _mockUnitOfWork.DidNotReceive().Commit();
    }

    [Fact]
    public async Task HandleAsync_ShouldHashPassword()
    {
        const string plainPassword = "PlainPassword123!";
        var request = TestDataBuilder.CreateValidCreateUserRequest(password: plainPassword);

        var expectedJwtCredentials = new JwtCredential("token", DateTime.UtcNow.AddHours(1));
        
        _mockUserRepository.GetByEmail(Arg.Any<string>())
            .Returns(Task.FromResult<User?>(null));
        _mockJwtService.GenerateJwtToken(Arg.Any<User>())
            .Returns(expectedJwtCredentials);

        await _handler.HandleAsync(request);

        await _mockUserRepository.Received(1).Add(Arg.Is<User>(u =>
            u.PasswordHash != plainPassword &&
            u.PasswordHash.StartsWith("$2")));
        await _mockUnitOfWork.Received(1).Commit();
    }

    [Fact]
    public async Task HandleAsync_ShouldSetUserRoleToUser()
    {
        var request = TestDataBuilder.CreateValidCreateUserRequest();
        
        var expectedJwtCredentials = new JwtCredential("token", DateTime.UtcNow.AddHours(1));

        _mockUserRepository.GetByEmail(Arg.Any<string>())
            .Returns(Task.FromResult<User?>(null));
        _mockJwtService.GenerateJwtToken(Arg.Any<User>())
            .Returns(expectedJwtCredentials);

        var result = await _handler.HandleAsync(request);

        result.Role.ShouldBe(nameof(UserRoles.User));
        await _mockUserRepository.Received(1).Add(Arg.Is<User>(u => 
            u.Role == UserRoles.User));
        await _mockUnitOfWork.Received(1).Commit();
    }

    [Fact]
    public async Task HandleAsync_ShouldCallRepositoryAddOnce()
    {
        var request = TestDataBuilder.CreateValidCreateUserRequest();
        
        var expectedJwtCredentials = new JwtCredential("token", DateTime.UtcNow.AddHours(1));

        _mockUserRepository.GetByEmail(Arg.Any<string>())
            .Returns(Task.FromResult<User?>(null));
        _mockJwtService.GenerateJwtToken(Arg.Any<User>())
            .Returns(expectedJwtCredentials);

        await _handler.HandleAsync(request);

        await _mockUserRepository.Received(1).Add(Arg.Any<User>());
        await _mockUserRepository.Received(1).GetByEmail(request.Email);
        await _mockUnitOfWork.Received(1).Commit();
    }

    [Fact]
    public async Task HandleAsync_ShouldCallJwtServiceToGenerateToken()
    {
        var request = TestDataBuilder.CreateValidCreateUserRequest();
        
        var expectedJwtCredentials = new JwtCredential("token", DateTime.UtcNow.AddHours(1));

        _mockUserRepository.GetByEmail(Arg.Any<string>())
            .Returns(Task.FromResult<User?>(null));
        _mockJwtService.GenerateJwtToken(Arg.Any<User>())
            .Returns(expectedJwtCredentials);

        await _handler.HandleAsync(request);

        _mockJwtService.Received(1).GenerateJwtToken(Arg.Is<User>(u =>
            u.Name == request.Username &&
            u.Email == request.Email &&
            u.Role == UserRoles.User));
        await _mockUnitOfWork.Received(1).Commit();
    }

    [Theory]
    [InlineData("user@example.com")]
    [InlineData("test.user+tag@domain.co.uk")]
    [InlineData("admin@test-domain.com")]
    public async Task HandleAsync_WithVariousEmailFormats_ShouldSucceed(string email)
    {
        var request = TestDataBuilder.CreateValidCreateUserRequest(email: email);

        var expectedJwtCredentials = new JwtCredential("token", DateTime.UtcNow.AddHours(1));
        
        _mockUserRepository.GetByEmail(Arg.Any<string>())
            .Returns(Task.FromResult<User?>(null));
        _mockJwtService.GenerateJwtToken(Arg.Any<User>())
            .Returns(expectedJwtCredentials);

        var result = await _handler.HandleAsync(request);

        result.ShouldNotBeNull();
        result.Email.ShouldBe(email);
        await _mockUnitOfWork.Received(1).Commit();
    }
}

