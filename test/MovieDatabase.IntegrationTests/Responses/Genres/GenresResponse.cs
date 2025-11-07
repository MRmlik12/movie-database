﻿namespace MovieDatabase.IntegrationTests.Responses.Genres;

public class GenresResponse
{
    public GenresConnection Genres { get; set; } = new();
}

public class GenresConnection
{
    public List<GenreQueryDto> Nodes { get; set; } = new();
}

public class GenreQueryDto
{
    public string? Id { get; set; }
    public string? Name { get; set; }
}