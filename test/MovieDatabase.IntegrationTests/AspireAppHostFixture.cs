using Aspire.Hosting;
using Aspire.Hosting.Testing;

namespace MovieDatabase.IntegrationTests;

public class AspireAppHostFixture : IAsyncLifetime
{
    private DistributedApplication? _app;

    public DistributedApplication App => _app ?? throw new InvalidOperationException("App not initialized");

    public async Task InitializeAsync()
    {
        var appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.MovieDatabase_AppHost>();

        _app = await appHost.BuildAsync();
        await _app.StartAsync();
        
        // Give the Cosmos DB emulator extra time to fully initialize
        await Task.Delay(TimeSpan.FromSeconds(5));
    }

    public HttpClient CreateHttpClient(string resourceName)
    {
        var client = App.CreateHttpClient(resourceName);
        // Increase timeout to 5 minutes for integration tests with Cosmos DB emulator
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
