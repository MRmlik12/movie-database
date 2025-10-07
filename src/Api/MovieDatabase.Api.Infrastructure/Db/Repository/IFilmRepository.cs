using Microsoft.Azure.Cosmos;

using MovieDatabase.Api.Core.Documents;

namespace MovieDatabase.Api.Infrastructure.Db.Repository;

public interface IFilmRepository
{
    Task Add(Film film);
}