using MovieDatabase.Api;
using MovieDatabase.Api.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.AddAzureCosmosClient(connectionName: "movies-db-cosmos");
builder.Services.AddCosmosDefaults();
builder.Services.AddGraphQLServer()
    .AddQueryType<Query>();

var app = builder.Build();

app.MapGraphQL();

app.UseHttpsRedirection();

app.Run();
