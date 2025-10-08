using MovieDatabase.Api.Core.Cqrs;
using MovieDatabase.Api.Infrastructure.Db.Repositories;

namespace MovieDatabase.Api.Application.Genres.VerifyGenreCreated;

public class VerifyGenreCreatedRequestHandler(IGenreRepository genreRepository) : IRequestHandler<VerifyGenreCreatedRequest, bool>
{
    public async Task<bool> Handle(VerifyGenreCreatedRequest request)
    {
        var existsById = await genreRepository.GetById(request.Id) != null;
        if (existsById)
        {
            return true;
        }
        
        var existByName = await genreRepository.GetByName(request.Name) != null;
        if (existByName)
        {
            return true;
        }

        return false;
    }
}