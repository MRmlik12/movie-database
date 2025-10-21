using MovieDatabase.Api.Core.Cqrs;
using MovieDatabase.Api.Core.Dtos.Users;

namespace MovieDatabase.Api.Application.Users.CreateUser;

public record CreateUserRequest(string Username, string Email, string Password) : IRequest<UserCredentialsDto>;