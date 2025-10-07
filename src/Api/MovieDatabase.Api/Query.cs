using MovieDatabase.Api.Application.Films.GetFilms;
using MovieDatabase.Api.Core.Cqrs;
using MovieDatabase.Api.Core.Dtos;

namespace MovieDatabase.Api;

public class Query
{
    public async Task<IEnumerable<FilmDto>> GetFilms([Service] IDispatcher dispatcher, string? title)
    {
        var request = new GetFilmsRequest(title);

        var result = await dispatcher.Dispatch(request);

        return result;
    }
}