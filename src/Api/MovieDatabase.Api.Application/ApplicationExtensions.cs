using Microsoft.Extensions.DependencyInjection;

using MovieDatabase.Api.Application.Actors.VerifyActorCreated;
using MovieDatabase.Api.Application.Films.CreateFilm;
using MovieDatabase.Api.Application.Films.GetFilms;
using MovieDatabase.Api.Application.Genres.VerifyGenreCreated;
using MovieDatabase.Api.Core.Cqrs;
using MovieDatabase.Api.Core.Dtos;

namespace MovieDatabase.Api.Application;

public static class ApplicationExtensions
{
    public static void AddApplicationDefaults(this IServiceCollection services)
        => services
            .AddDispatcher()
            .RegisterHandlers();
    
    private static IServiceCollection AddDispatcher(this IServiceCollection services)
    {
        services.AddScoped<IDispatcher, Dispatcher>();

        return services;
    }
    
    private static IServiceCollection RegisterHandlers(this IServiceCollection services)
    {
        services.AddScoped<IRequestHandler<CreateFilmRequest, FilmDto>, CreateFilmRequestHandler>();
        services.AddScoped<IRequestHandler<GetFilmsRequest, IEnumerable<FilmDto>>, GetFilmsRequestHandler>();
        services.AddScoped<IRequestHandler<VerifyActorCreatedRequest, bool>, VerifyActorCreatedRequestHandler>();
        services.AddScoped<IRequestHandler<VerifyGenreCreatedRequest, bool>, VerifyGenreCreatedRequestHandler>();
        
        return services;
    }
}