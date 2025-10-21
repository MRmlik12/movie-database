using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using MovieDatabase.Api.Core.Documents.Films;
using MovieDatabase.Api.Core.Documents.Users;

namespace MovieDatabase.Api.Infrastructure.Db;

public static class CosmosInitializer
{
    public static async Task Initialize(IApplicationBuilder builder)
    {
        using var serviceScope = builder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var wrapper = serviceScope.ServiceProvider.GetRequiredService<CosmosWrapper>();

        await wrapper.InitializeContainers();
        
        var userContainer = wrapper.Movies.GetContainer(nameof(User));
        var userIter = userContainer.GetItemQueryIterator<User>("SELECT TOP 1 * FROM c");
        var users = await userIter.ReadNextAsync();
        
        if (users.Count == 0)
        {
            await CosmosSeeder.SeedUsers(wrapper.Movies);
        }

        var filmContainer = wrapper.Movies.GetContainer(nameof(Film));
        var filmIter = filmContainer.GetItemQueryIterator<Film>("SELECT TOP 1 * FROM c");
        var films = await filmIter.ReadNextAsync();

        if (films.Count == 0)
        {
            await CosmosSeeder.SeedFilms(wrapper.Movies);
        }
    }
}