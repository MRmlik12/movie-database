using MovieDatabase.Api.Core.Dtos;

namespace MovieDatabase.IntegrationTests.Responses.Producers;

public class ProducersResponse
{
    public List<ProducerDto> Producers { get; set; } = new();
}