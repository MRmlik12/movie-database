using MovieDatabase.Api.Core.Documents.Users;

namespace MovieDatabase.Api.Core.Services;

public interface IJwtService
{
    (string? token, DateTime expireDate) GenerateJwtToken(User user);
}