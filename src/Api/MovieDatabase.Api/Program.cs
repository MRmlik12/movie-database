using Microsoft.Azure.Cosmos;

using MovieDatabase.Api;
using MovieDatabase.Api.Application;
using MovieDatabase.Api.Core;
using MovieDatabase.Api.Infrastructure;
using MovieDatabase.Api.Infrastructure.Db;
using MovieDatabase.Api.Infrastructure.HttpInterceptors;
using MovieDatabase.Api.Mutations;

var builder = WebApplication.CreateBuilder(args);

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
    .AddAuthorization()
    // .AddHttpRequestInterceptor<AppHttpRequestInterceptor>()
    .AddMutationType(d => d.Name("Mutation"))
    .AddTypeExtension<FilmMutations>()
    .AddTypeExtension<UserMutations>()
    .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = builder.Environment.IsDevelopment())
    .AddQueryType<Query>();

var app = builder.Build();

await CosmosInitializer.Initialize(app);

// Minimal API: make sure authentication middleware runs and MapGraphQL is used
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapGraphQL();

app.Run();