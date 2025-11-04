using MovieDatabase.Api.Core.Documents.Films;

namespace MovieDatabase.Api.Infrastructure.Db.Repositories;

public interface IFilmRepository
{
    Task Add(Film film);
    Task<Film?> GetByTitle(string title);
    Task<Film?> GetById(string id);
    Task Delete(Film film);
    Task Update(Film film);
}