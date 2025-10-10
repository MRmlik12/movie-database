using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace MovieDatabase.Api.Infrastructure.Db;

public static class CosmosInitializer
{
    public static async Task Initialize(IApplicationBuilder builder)
    {
        using var serviceScope = builder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var wrapper = serviceScope.ServiceProvider.GetRequiredService<CosmosWrapper>();

        await wrapper.InitializeContainers();
    }
}