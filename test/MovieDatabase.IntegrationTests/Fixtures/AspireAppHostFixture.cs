using Aspire.Hosting;
using Aspire.Hosting.Testing;

using Microsoft.Extensions.Configuration;

using System.Reflection;

namespace MovieDatabase.IntegrationTests.Fixtures;

public class AspireAppHostFixture : IAsyncLifetime
{
    private DistributedApplication? _app;

    public DistributedApplication App => _app ?? throw new InvalidOperationException("App not initialized");
    
    public async Task InitializeAsync()
    {
        var appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.MovieDatabase_AppHost>();

        using var stream = GetIntegrationTestConfigStream();
        appHost.Configuration.AddJsonStream(stream);
        
        _app = await appHost.BuildAsync();
        await _app.StartAsync();

        await Task.Delay(TimeSpan.FromSeconds(5));
    }

    private static Stream GetIntegrationTestConfigStream()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = "MovieDatabase.IntegrationTests.appsettings.IntegrationTest.json";
        
        var stream = assembly.GetManifestResourceStream(resourceName);
        
        if (stream == null)
        {
            var availableResources = string.Join(", ", assembly.GetManifestResourceNames());
            throw new FileNotFoundException(
                $"Embedded resource '{resourceName}' not found. " +
                $"Available resources: {availableResources}");
        }
        
        return stream;
    }

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