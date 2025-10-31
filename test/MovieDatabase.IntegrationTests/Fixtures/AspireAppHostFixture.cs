using Aspire.Hosting;
using Aspire.Hosting.Testing;

using Microsoft.Extensions.Configuration;

namespace MovieDatabase.IntegrationTests.Fixtures;

public class AspireAppHostFixture : IAsyncLifetime
{
    private DistributedApplication? _app;

    public DistributedApplication App => _app ?? throw new InvalidOperationException("App not initialized");
    
    public async Task InitializeAsync()
    {
        var appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.MovieDatabase_AppHost>();

        var configPath = GetIntegrationTestConfigPath();
        appHost.Configuration.AddJsonFile(configPath);
        
        _app = await appHost.BuildAsync();
        await _app.StartAsync();

        await Task.Delay(TimeSpan.FromSeconds(5));
    }

    private static string GetIntegrationTestConfigPath()
        => Path.Combine(AppContext.BaseDirectory, "appsettings.IntegrationTest.json");

    public HttpClient CreateHttpClient(string resourceName)
    {
        var client = App.CreateHttpClient(resourceName);

        client.Timeout = TimeSpan.FromMinutes(5);
        return client;
    }

    public async Task DisposeAsync()
    {
        if (_app != null)
        {
            await _app.DisposeAsync();
        }
    }
}