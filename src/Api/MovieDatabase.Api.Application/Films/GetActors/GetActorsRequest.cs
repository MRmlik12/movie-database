using MovieDatabase.Api.Core.Cqrs;
using MovieDatabase.Api.Core.Dtos;

namespace MovieDatabase.Api.Application.Films.GetActors;

public record GetActorsRequest(string? SearchTerm) : IRequest<IEnumerable<ActorDto>>;
