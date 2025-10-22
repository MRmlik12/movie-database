using MovieDatabase.Api.Application.Users.Exceptions;
using MovieDatabase.Api.Core.Cqrs;
using MovieDatabase.Api.Core.Dtos.Users;
using MovieDatabase.Api.Core.Services;
using MovieDatabase.Api.Core.Utils;
using MovieDatabase.Api.Infrastructure.Db.Repositories;

namespace MovieDatabase.Api.Application.Users.AuthenticateUser;

public class AuthenticateUserRequestHandler(IUserRepository userRepository, IJwtService jwtService) : IRequestHandler<AuthenticateUserRequest, UserCredentialsDto>
{
    public async Task<UserCredentialsDto> HandleAsync(AuthenticateUserRequest request)
    {
        var user = await userRepository.GetByEmail(request.Email);

        if (user is null)
        {
            throw new InvalidUserCredentialsApplicationException();
        }

        var isPasswordValid = PasswordUtils.VerifyPassword(request.Password, user.PasswordHash);
        if (!isPasswordValid)
        {
            throw new InvalidUserCredentialsApplicationException();
        }

        var (token, expireDate) = jwtService.GenerateJwtToken(user);
        var userDto = UserCredentialsDto.From(user);

        userDto.Token = token;
        userDto.ExpireTime = expireDate;

        return userDto;
    }
}