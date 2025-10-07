using Microsoft.Extensions.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var isDevelopment = builder.Environment.IsDevelopment();
var cosmos = builder.AddAzureCosmosDB("movies-database-cosmos");

cosmos.AddCosmosDatabase("movies-db", "Movies");

if (isDevelopment)
{
    cosmos.RunAsPreviewEmulator();
}

var apiService = builder.AddProject<Projects.MovieDatabase_Api>("movies-db-api")
    .WithReference(cosmos);

builder.Build().Run();
