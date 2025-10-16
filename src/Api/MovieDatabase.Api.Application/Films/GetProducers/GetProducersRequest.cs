using MovieDatabase.Api.Core.Cqrs;
using MovieDatabase.Api.Core.Dtos;

namespace MovieDatabase.Api.Application.Films.GetProducers;

public record GetProducersRequest(string? SearchTerm) : IRequest<IEnumerable<ProducerDto>>;