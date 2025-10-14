using MovieDatabase.Api.Application.Films.GetActors;
using MovieDatabase.Api.Application.Films.GetFilms;
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
    
    public async Task<IEnumerable<ActorDto>> GetActors([Service] IDispatcher dispatcher, string? searchTerm)
    {
        var request = new GetActorsRequest(searchTerm);

        var result = await dispatcher.Dispatch(request);

        return result;
    }
}