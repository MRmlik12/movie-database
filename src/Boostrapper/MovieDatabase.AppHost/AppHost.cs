var builder = DistributedApplication.CreateBuilder(args);

var cosmos = builder.AddAzureCosmosDB("movies-db-cosmos");
var moviesDb = cosmos.AddCosmosDatabase("movie-db");
cosmos.RunAsPreviewEmulator();

var apiService = builder.AddProject<Projects.MovieDatabase_Api>("movies-db-api");

builder.Build().Run();
