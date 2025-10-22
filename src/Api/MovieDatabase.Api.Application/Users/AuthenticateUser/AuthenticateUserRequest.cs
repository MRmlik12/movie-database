using MovieDatabase.Api.Core.Cqrs;
using MovieDatabase.Api.Core.Dtos.Users;

namespace MovieDatabase.Api.Application.Users.AuthenticateUser;

public record AuthenticateUserRequest(string Email, string Password) : IRequest<UserCredentialsDto>;