using MovieDatabase.Api.Core.Cqrs;
using MovieDatabase.Api.Core.Dtos;

namespace MovieDatabase.Api.Application.Films.GetFilms;

public record GetFilmsRequest(string? Title) : IRequest<IEnumerable<FilmDto>>;
