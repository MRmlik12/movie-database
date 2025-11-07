﻿namespace MovieDatabase.IntegrationTests.Responses.Producers;

public class ProducersResponse
{
    public ProducersConnection Producers { get; set; } = new();
}

public class ProducersConnection
{
    public List<ProducerQueryDto> Nodes { get; set; } = new();
}

public class ProducerQueryDto
{
    public string? Id { get; set; }
    public string? Name { get; set; }
}