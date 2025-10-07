using MovieDatabase.Api.Core.Documents;

namespace MovieDatabase.Api.Infrastructure.Db.Repositories;

public interface IGenreRepository
{
    Task<Genre?> GetById(string id);
    Task<Genre?> GetByName(string name);
}