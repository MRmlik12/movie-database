using MovieDatabase.Api.Core.Documents;
using MovieDatabase.Api.Core.Documents.Films;

namespace MovieDatabase.Api.Infrastructure.Db.Repositories;

public interface IFilmRepository
{
    Task Add(Film film);
    Task<Film?> GetByName(string name);
    Task<Film?> GetById(string id);
    Task<IEnumerable<Film>> GetAll(string? title);
}