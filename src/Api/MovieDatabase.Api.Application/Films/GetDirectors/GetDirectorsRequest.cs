using MovieDatabase.Api.Core.Cqrs;
using MovieDatabase.Api.Core.Dtos;

namespace MovieDatabase.Api.Application.Films.GetDirectors;

public record GetDirectorsRequest(string? Term) : IRequest<IEnumerable<DirectorDto>>;