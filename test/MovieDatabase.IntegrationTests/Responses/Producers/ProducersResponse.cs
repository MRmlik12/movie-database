using MovieDatabase.Api.Core.Dtos;
using MovieDatabase.Api.Core.Dtos.Films;

namespace MovieDatabase.IntegrationTests.Responses.Producers;

public class ProducersResponse
{
    public List<ProducerDto> Producers { get; set; } = new();
}