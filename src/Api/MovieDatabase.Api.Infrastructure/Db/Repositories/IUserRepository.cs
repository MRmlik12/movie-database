using MovieDatabase.Api.Core.Documents.Users;

namespace MovieDatabase.Api.Infrastructure.Db.Repositories;

public interface IUserRepository
{
    Task Add(User user);
    Task<User?> GetByEmail(string email);
}