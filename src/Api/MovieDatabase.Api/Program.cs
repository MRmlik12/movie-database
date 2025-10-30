using System.Text.Json;
using System.Text.Json.Serialization;

using MovieDatabase.Api;
using MovieDatabase.Api.Application;
using MovieDatabase.Api.Core;
using MovieDatabase.Api.Infrastructure;
using MovieDatabase.Api.Infrastructure.Db;
using MovieDatabase.Api.Mutations;

var builder = WebApplication.CreateBuilder(args);

builder.AddAzureCosmosClient(connectionName: "movies-database-cosmos", configureClientOptions: opt =>
{
    opt.UseSystemTextJsonSerializerWithOptions = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PreferredObjectCreationHandling = JsonObjectCreationHandling.Populate,
        Converters = { new JsonStringEnumConverter() }
    };
});

builder.Services.AddApplicationDefaults();
builder.Services.AddInfrastructureDefaults(builder.Configuration);
builder.Services.AddCoreDefaults(builder.Configuration);
builder.Services.AddHttpContextAccessor();
builder.Services
    .AddGraphQLServer()
    .AddAuthorization()
    .AddHttpRequestInterceptor<AppHttpRequestInterceptor>()
    .AddMutationType(d => d.Name("Mutation"))
    .AddTypeExtension<FilmMutations>()
    .AddTypeExtension<UserMutations>()
    .AddQueryType<Query>();

var app = builder.Build();

await CosmosInitializer.Initialize(app);

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapGraphQL();

app.Run();

public partial class Program { }