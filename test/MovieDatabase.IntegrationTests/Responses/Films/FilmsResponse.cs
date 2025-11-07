﻿using MovieDatabase.Api.Core.Dtos.Films;

namespace MovieDatabase.IntegrationTests.Responses.Films;

public class FilmsResponse
{
    public FilmsConnection Films { get; set; } = new();
}

public class FilmsConnection
{
    public List<FilmQueryDto> Nodes { get; set; } = new();
    public PageInfo? PageInfo { get; set; }
}

public class PageInfo
{
    public bool HasNextPage { get; set; }
    public bool HasPreviousPage { get; set; }
    public string? StartCursor { get; set; }
    public string? EndCursor { get; set; }
}

// Wrapper for GraphQL response that matches the query structure
public class FilmQueryDto
{
    public string? Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? ReleaseDate { get; set; }
    public List<ActorDto>? Actors { get; set; }
    public DirectorDto? Director { get; set; }
    public List<GenreDto>? Genres { get; set; }
    public ProducerDto? Producer { get; set; }
}