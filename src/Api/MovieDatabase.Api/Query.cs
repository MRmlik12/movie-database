using MovieDatabase.Api.Application.Films.GetActors;
using MovieDatabase.Api.Application.Films.GetDirectors;
using MovieDatabase.Api.Application.Films.GetFilms;
using MovieDatabase.Api.Application.Films.GetGenres;
using MovieDatabase.Api.Core.Cqrs;
using MovieDatabase.Api.Core.Dtos;

namespace MovieDatabase.Api;

public class Query
{
    public async Task<IEnumerable<FilmDto>> GetFilms([Service] IDispatcher dispatcher, string? title = null)
    {
        var request = new GetFilmsRequest(title);

        var result = await dispatcher.Dispatch(request);

        return result;
    }

    public async Task<IEnumerable<ActorDto>> GetActors([Service] IDispatcher dispatcher, string? term)
    {
        var request = new GetActorsRequest(term);

        var result = await dispatcher.Dispatch(request);

        return result;
    }

    public async Task<IEnumerable<GenreDto>> GetGenres([Service] IDispatcher dispatcher, string? term)
    {
        var request = new GetGenresRequest(term);

        var result = await dispatcher.Dispatch(request);

        return result;
    }

    public async Task<IEnumerable<DirectorDto>> GetDirectors([Service] IDispatcher dispatcher, string? term)
    {
        var request = new GetDirectorsRequest(term);

        var result = await dispatcher.Dispatch(request);

        return result;
    }
}