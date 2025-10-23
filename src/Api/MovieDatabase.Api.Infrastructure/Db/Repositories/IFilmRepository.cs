using MovieDatabase.Api.Core.Documents.Films;

namespace MovieDatabase.Api.Infrastructure.Db.Repositories;

public interface IFilmRepository
{
    Task Add(Film film);
    Task<Film?> GetByTitle(string title);
    Task<Film?> GetById(string id);
    Task<IEnumerable<Actor>> GetActors(string? searchTerm);
    Task<IEnumerable<Genre>> GetGenres(string? searchTerm);
    Task<IEnumerable<DirectorInfo>> GetDirectors(string? searchTerm);
    Task<IEnumerable<ProducerInfo>> GetProducers(string? searchTerm);
    Task<IEnumerable<Film>> GetAll(string? title);
    Task Delete(Film film);
    Task Update(Film film);
}