﻿namespace MovieDatabase.IntegrationTests.Responses.Directors;

public class DirectorsResponse
{
    public DirectorsConnection Directors { get; set; } = new();
}

public class DirectorsConnection
{
    public List<DirectorQueryDto> Nodes { get; set; } = new();
}

public class DirectorQueryDto
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
}