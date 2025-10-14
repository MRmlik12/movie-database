using MovieDatabase.Api.Application.Films.CreateFilm;
using MovieDatabase.Api.Core.Cqrs;
using MovieDatabase.Api.Core.Dtos;

namespace MovieDatabase.Api.Mutations;

public class FilmMutations
{
    public async Task<FilmDto> CreateFilm(CreateFilmRequest input, [Service] IDispatcher dispatcher)
    {
        var result = await dispatcher.Dispatch(input);

        return result;
    }
}