using Microsoft.EntityFrameworkCore;
using User = MovieDatabase.Api.Core.Documents.Users.User;

namespace MovieDatabase.Api.Infrastructure.Db.Repositories;

public class UserRepository(AppDbContext context) : IUserRepository
{
    public async Task Add(User user)
    {
        context.Users.Add(user);
        await context.SaveChangesAsync();
    }

    public async Task<User?> GetByEmail(string email)
    {
        return await context.Users
            .FirstOrDefaultAsync(u => u.Email == email);
    }
}

