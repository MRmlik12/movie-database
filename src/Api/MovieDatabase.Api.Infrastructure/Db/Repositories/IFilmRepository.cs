using MovieDatabase.Api.Core.Documents;

namespace MovieDatabase.Api.Infrastructure.Db.Repositories;

public interface IFilmRepository
{
    Task Add(Film film);
    Task<Film?> GetByName(string name);
    Task<IEnumerable<Film>> GetAll(string? title = "");
}