using Microsoft.Azure.Cosmos;

using MovieDatabase.Api;
using MovieDatabase.Api.Application;
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
builder.Services.AddInfrastructureDefaults();
builder.Services.AddGraphQLServer()
    .AddMutationType<FilmMutations>()
    .AddQueryType<Query>();

var app = builder.Build();

app.MapGraphQL();

await CosmosInitializer.Initialize(app);

app.UseHttpsRedirection();

app.Run();