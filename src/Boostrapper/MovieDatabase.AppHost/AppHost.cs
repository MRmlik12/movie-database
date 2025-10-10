using Microsoft.Extensions.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

const string cosmosResourceName = "movies-database-cosmos";

var isDevelopment = builder.Environment.IsDevelopment();
var cosmos = builder.AddAzureCosmosDB(cosmosResourceName);

if (isDevelopment)
{
    cosmos.RunAsPreviewEmulator();
}

cosmos.AddCosmosDatabase("movies-db", "Movies");

var apiService = builder.AddProject<Projects.MovieDatabase_Api>("movies-db-api")
    .WithReference(cosmos);

builder.Build().Run();
