using Microsoft.Extensions.DependencyInjection;

using MovieDatabase.Api.Infrastructure.Db;
using MovieDatabase.Api.Infrastructure.Db.Repositories;

namespace MovieDatabase.Api.Infrastructure;

public static class InfrastructureExtensions
{
    public static void AddInfrastructureDefaults(this IServiceCollection services)
        => services.AddCosmosDefaults()
            .AddRepositories();

    private static IServiceCollection AddCosmosDefaults(this IServiceCollection services)
    {
        services.AddTransient<CosmosWrapper>();

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddTransient<IFilmRepository, FilmRepository>();

        return services;
    }
}