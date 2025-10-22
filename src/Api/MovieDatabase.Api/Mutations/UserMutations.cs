using HotChocolate.Authorization;

using MovieDatabase.Api.Application.Users.AuthenticateUser;
using MovieDatabase.Api.Application.Users.CreateUser;
using MovieDatabase.Api.Core.Cqrs;
using MovieDatabase.Api.Core.Dtos.Users;

namespace MovieDatabase.Api.Mutations;

[ExtendObjectType("Mutation")]
public class UserMutations
{
    [AllowAnonymous]
    public async Task<UserCredentialsDto> LoginUser(AuthenticateUserRequest request, [Service] IDispatcher dispatcher)
    {
        var result = await dispatcher.Dispatch(request);

        return result;
    }

    [AllowAnonymous]
    public async Task<UserCredentialsDto> RegisterUser(CreateUserRequest request, [Service] IDispatcher dispatcher)
    {
        var result = await dispatcher.Dispatch(request);

        return result;
    }
}