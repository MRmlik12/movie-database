using Microsoft.Extensions.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var isDevelopment = builder.Environment.IsDevelopment();
var cosmos = builder.AddAzureCosmosDB("movies-db-cosmos");
// var moviesDb = cosmos.AddCosmosDatabase("movie-db");

if (isDevelopment)
{
    cosmos.RunAsPreviewEmulator();
}


var apiService = builder.AddProject<Projects.MovieDatabase_Api>("movies-db-api");

builder.Build().Run();
