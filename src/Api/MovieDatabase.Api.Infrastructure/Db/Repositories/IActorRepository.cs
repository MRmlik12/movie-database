using MovieDatabase.Api.Core.Documents;

namespace MovieDatabase.Api.Infrastructure.Db.Repositories;

public interface IActorRepository
{
    public Task<Actor?> GetByIdAsync(string id);
    public Task<Actor?> GetByNameSurname(string name, string surname);
}