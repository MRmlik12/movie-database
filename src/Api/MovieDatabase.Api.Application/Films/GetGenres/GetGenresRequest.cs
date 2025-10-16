using MovieDatabase.Api.Core.Cqrs;
using MovieDatabase.Api.Core.Dtos;

namespace MovieDatabase.Api.Application.Films.GetGenres;

public record GetGenresRequest(string? Term) : IRequest<IEnumerable<GenreDto>>;