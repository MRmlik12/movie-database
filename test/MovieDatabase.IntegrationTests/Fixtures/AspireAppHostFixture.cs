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
        // Load configuration and set as environment variables before creating the app host
        var config = LoadIntegrationTestConfiguration();
        SetJwtEnvironmentVariables(config);
        
        var appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.MovieDatabase_AppHost>();
        
        _app = await appHost.BuildAsync();
        await _app.StartAsync();

        await Task.Delay(TimeSpan.FromSeconds(5));
    }

    private static IConfigurationRoot LoadIntegrationTestConfiguration()
    {
        var configBuilder = new ConfigurationBuilder();
        using var stream = GetIntegrationTestConfigStream();
        configBuilder.AddJsonStream(stream);
        return configBuilder.Build();
    }

    private static void SetJwtEnvironmentVariables(IConfiguration config)
    {
        var jwtKey = config["Jwt:Key"];
        var jwtIssuer = config["Jwt:Issuer"];
        var jwtAudience = config["Jwt:Audience"];
        
        if (!string.IsNullOrEmpty(jwtKey))
        {
            Environment.SetEnvironmentVariable("Jwt__Key", jwtKey);
        }
        if (!string.IsNullOrEmpty(jwtIssuer))
        {
            Environment.SetEnvironmentVariable("Jwt__Issuer", jwtIssuer);
        }
        if (!string.IsNullOrEmpty(jwtAudience))
        {
            Environment.SetEnvironmentVariable("Jwt__Audience", jwtAudience);
        }
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