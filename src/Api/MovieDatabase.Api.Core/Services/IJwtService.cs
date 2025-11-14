using MovieDatabase.Api.Core.Documents.Users;
using MovieDatabase.Api.Core.Jwt;

namespace MovieDatabase.Api.Core.Services;

public interface IJwtService
{
    JwtCredential GenerateJwtToken(User user);
}