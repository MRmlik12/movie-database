using MovieDatabase.Api.Core.Cqrs;
using MovieDatabase.Api.Core.Documents;

namespace MovieDatabase.Api.Application.Genres.CreateGenreIfNotExist;

public record CreateGenreIfNotExistRequest(string Id, string Name) : IRequest<Genre>;