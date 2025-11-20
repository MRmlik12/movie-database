using MovieDatabase.Api.Core.Cqrs;
using MovieDatabase.Api.Core.Documents.Users;
using MovieDatabase.Api.Core.Dtos.Users;
using MovieDatabase.Api.Core.Exceptions.Users;
using MovieDatabase.Api.Core.Services;
using MovieDatabase.Api.Core.Utils;
using MovieDatabase.Api.Infrastructure.Db;
using MovieDatabase.Api.Infrastructure.Db.Repositories;

namespace MovieDatabase.Api.Application.Users.CreateUser;

public sealed class CreateUserRequestHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    IJwtService jwtService
) : IRequestHandler<CreateUserRequest, UserCredentialsDto>
{
    public async Task<UserCredentialsDto> HandleAsync(CreateUserRequest request)
    {
        var existingUser = await userRepository.GetByEmail(request.Email);
        if (existingUser != null)
        {
            throw new DuplicateEmailApplicationException();
        }

        var user = new User
        {
            Name = request.Username,
            Email = request.Email,
            PasswordHash = PasswordUtils.HashPassword(request.Password),
            Role = UserRoles.User
        };

        userRepository.Add(user);

        var credentials = jwtService.GenerateJwtToken(user);

        var userDto = UserCredentialsDto.From(user);

        userDto.Token = credentials.AccessToken.Token;
        userDto.ExpireTime = credentials.AccessToken.ExpireDate;
        
        userDto.RefreshToken = credentials.RefreshToken.Token;
        userDto.RefreshTokenExpireTime = credentials.RefreshToken.ExpireDate;

        await unitOfWork.Commit();

        return userDto;
    }
}