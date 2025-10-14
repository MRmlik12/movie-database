using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using MovieDatabase.Api.Core.Documents;
using MovieDatabase.Api.Core.Documents.Films;

namespace MovieDatabase.Api.Infrastructure.Db;

public static class CosmosInitializer
{
    public static async Task Initialize(IApplicationBuilder builder)
    {
        using var serviceScope = builder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var wrapper = serviceScope.ServiceProvider.GetRequiredService<CosmosWrapper>();

        await wrapper.InitializeContainers();

        var filmContainer = wrapper.Movies.GetContainer(nameof(Film));
        var iter = filmContainer.GetItemQueryIterator<Film>("SELECT TOP 1 * FROM c");
        var films = await iter.ReadNextAsync();

        if (films.Count == 0)
        {
            await CosmosSeeder.SeedFilms(wrapper.Movies);
        }
    }
}