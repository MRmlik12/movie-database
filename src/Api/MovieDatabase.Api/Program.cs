using Microsoft.Azure.Cosmos;

using MovieDatabase.Api;
using MovieDatabase.Api.Application;
using MovieDatabase.Api.Core;
using MovieDatabase.Api.Infrastructure;
using MovieDatabase.Api.Infrastructure.Db;
using MovieDatabase.Api.Mutations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.AddAzureCosmosClient(connectionName: "movies-database-cosmos", configureClientOptions: opt =>
{
    opt.SerializerOptions = new CosmosSerializationOptions
    {
        PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
    };
});

builder.Services.AddApplicationDefaults();
builder.Services.AddInfrastructureDefaults(builder.Configuration);
builder.Services.AddCoreDefaults(builder.Configuration);
builder.Services.AddGraphQLServer()
    .AddMutationType(d => d.Name("Mutation"))
    .AddTypeExtension<FilmMutations>()
    .AddTypeExtension<UserMutations>()
    .AddQueryType<Query>()
    .AddAuthorization();

var app = builder.Build();

await CosmosInitializer.Initialize(app);

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(endpoints => endpoints.MapGraphQL());

app.UseHttpsRedirection();

app.Run();