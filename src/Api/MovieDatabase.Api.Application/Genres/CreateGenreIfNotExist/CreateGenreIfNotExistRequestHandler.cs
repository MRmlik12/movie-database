using MovieDatabase.Api.Core.Cqrs;
using MovieDatabase.Api.Core.Documents;
using MovieDatabase.Api.Infrastructure.Db.Repositories;

namespace MovieDatabase.Api.Application.Genres.CreateGenreIfNotExist;

internal class CreateGenreIfNotExistRequestHandler(IGenreRepository genreRepository)
    : IRequestHandler<CreateGenreIfNotExistRequest, Genre>
{
    public async Task<Genre> Handle(CreateGenreIfNotExistRequest request)
    {
        var genre = await genreRepository.GetById(request.Id);
        if (genre is not null)
        {
            return genre;
        }

        genre = await genreRepository.GetByName(request.Name);
        if (genre is not null)
        {
            return genre;
        }

        genre = new Genre(request.Name);

        return genre;
    }
}