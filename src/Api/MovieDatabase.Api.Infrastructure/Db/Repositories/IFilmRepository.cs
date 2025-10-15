using MovieDatabase.Api.Core.Documents.Films;
using MovieDatabase.Api.Core.Dtos;

namespace MovieDatabase.Api.Infrastructure.Db.Repositories;

public interface IFilmRepository
{
    Task Add(Film film);
    Task<Film?> GetByName(string name);
    Task<Film?> GetById(string id);
    Task<IEnumerable<Actor>> GetActors(string? searchTerm);
    Task<IEnumerable<Genre>> GetGenres(string? searchTerm);
    Task<IEnumerable<Film>> GetAll(string? title);
}