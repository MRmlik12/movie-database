using Microsoft.Extensions.DependencyInjection;

using MovieDatabase.Api.Application.Films.CreateFilm;
using MovieDatabase.Api.Application.Films.DeleteFilm;
using MovieDatabase.Api.Application.Films.EditFilm;
using MovieDatabase.Api.Application.Films.GetActors;
using MovieDatabase.Api.Application.Films.GetDirectors;
using MovieDatabase.Api.Application.Films.GetFilms;
using MovieDatabase.Api.Application.Films.GetGenres;
using MovieDatabase.Api.Application.Films.GetProducers;
using MovieDatabase.Api.Application.Users.AuthenticateUser;
using MovieDatabase.Api.Application.Users.CreateUser;
using MovieDatabase.Api.Core.Cqrs;
using MovieDatabase.Api.Core.Dtos;
using MovieDatabase.Api.Core.Dtos.Users;

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
        services.AddScoped<IRequestHandler<GetActorsRequest, IEnumerable<ActorDto>>, GetActorsRequestHandler>();
        services.AddScoped<IRequestHandler<GetGenresRequest, IEnumerable<GenreDto>>, GetGenresRequestHandler>();
        services.AddScoped<IRequestHandler<GetDirectorsRequest, IEnumerable<DirectorDto>>, GetDirectorsRequestHandler>();
        services.AddScoped<IRequestHandler<GetProducersRequest, IEnumerable<ProducerDto>>, GetProducersRequestHandler>();
        services.AddScoped<IRequestHandler<CreateUserRequest, UserCredentialsDto>, CreateUserRequestHandler>();
        services.AddScoped<IRequestHandler<AuthenticateUserRequest, UserCredentialsDto>, AuthenticateUserRequestHandler>();
        services.AddScoped<IRequestHandler<DeleteFilmRequest, string>, DeleteFilmRequestHandler>();
        services.AddScoped<IRequestHandler<EditFilmRequest, FilmDto>, EditFilmRequestHandler>();

        return services;
    }
}