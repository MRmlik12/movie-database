using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MovieDatabase.Api.Infrastructure.Db;
using MovieDatabase.Api.Infrastructure.Db.Repositories;

namespace MovieDatabase.Api.Infrastructure;

public static class InfrastructureExtensions
{
    public static void AddInfrastructureDefaults(this IServiceCollection services, IConfiguration configuration)
        => services.AddCosmosDbContext(configuration)
            .AddRepositories()
            .AddJwtAuthenticationDefaults(configuration);

    private static IServiceCollection AddCosmosDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAzureCosmosClient(connectionName: "movies-database-cosmos", configureClientOptions: opt =>
        {
            opt.UseSystemTextJsonSerializerWithOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PreferredObjectCreationHandling = JsonObjectCreationHandling.Populate,
                Converters = { new JsonStringEnumConverter() }
            };
        });
        
        services.AddDbContext<AppDbContext>(options =>
        {
            if (!string.IsNullOrEmpty(connectionString))
            {
                options.UseCosmos(
                    "movies-database-cosmos",
                    databaseName: "Movies",
                    cosmosOptionsAction: cosmosOptions =>
                    {
                        cosmosOptions.ConnectionMode(Microsoft.Azure.Cosmos.ConnectionMode.Gateway);
                        cosmosOptions.RequestTimeout(TimeSpan.FromSeconds(30));
                    });
            }
            else
            {
                var cosmosEndpoint = configuration["Cosmos:Endpoint"] 
                    ?? "https://localhost:8081";
                var cosmosKey = configuration["Cosmos:Key"]
                    ?? "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
                
                options.UseCosmos(
                    cosmosEndpoint,
                    cosmosKey,
                    databaseName: "Movies",
                    cosmosOptionsAction: cosmosOptions =>
                    {
                        cosmosOptions.ConnectionMode(Microsoft.Azure.Cosmos.ConnectionMode.Gateway);
                        cosmosOptions.RequestTimeout(TimeSpan.FromSeconds(30));
                    });
            }
        });

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IFilmRepository, FilmRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }

    private static IServiceCollection AddJwtAuthenticationDefaults(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    RequireExpirationTime = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not configured"))),
                    RoleClaimType = ClaimTypes.Role
                };
            });

        services.AddAuthorization();

        return services;
    }
}

