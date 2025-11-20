using MovieDatabase.Api.Core.Cqrs;
using MovieDatabase.Api.Core.Dtos.Users;
using MovieDatabase.Api.Core.Exceptions.Users;
using MovieDatabase.Api.Core.Services;
using MovieDatabase.Api.Core.Utils;
using MovieDatabase.Api.Infrastructure.Db.Repositories;

namespace MovieDatabase.Api.Application.Users.AuthenticateUser;

public sealed class AuthenticateUserRequestHandler(IUserRepository userRepository, IJwtService jwtService) : IRequestHandler<AuthenticateUserRequest, UserCredentialsDto>
{
    public async Task<UserCredentialsDto> HandleAsync(AuthenticateUserRequest request)
    {
        var user = await userRepository.GetByEmail(request.Email);

        if (user is null || string.IsNullOrWhiteSpace(request.Password))
        {
            throw new InvalidUserCredentialsApplicationException();
        }

        var isPasswordValid = PasswordUtils.VerifyPassword(request.Password, user.PasswordHash);
        if (!isPasswordValid)
        {
            throw new InvalidUserCredentialsApplicationException();
        }

        var credentials = jwtService.GenerateJwtToken(user);
        var userDto = UserCredentialsDto.From(user);

        userDto.Token = credentials.AccessToken.Token;
        userDto.ExpireTime = credentials.AccessToken.ExpireDate;
        
        userDto.RefreshToken = credentials.RefreshToken.Token;
        userDto.RefreshTokenExpireTime = credentials.RefreshToken.ExpireDate;
        
        return userDto;
    }
}