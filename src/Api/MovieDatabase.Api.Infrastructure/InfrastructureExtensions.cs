using Microsoft.Extensions.DependencyInjection;

using MovieDatabase.Api.Infrastructure.Db;
using MovieDatabase.Api.Infrastructure.Db.Repository;

namespace MovieDatabase.Api.Infrastructure;

public static class InfrastructureExtensions
{
    public static void AddCosmosDefaults(this IServiceCollection services)
    {
        services.AddTransient<CosmosWrapper>();
        
        AddRepositories(services);
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddTransient<IFilmRepository, FilmRepository>();
    }
}