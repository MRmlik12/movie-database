using MovieDatabase.Api.Core.Cqrs;

namespace MovieDatabase.Api.Application.Genres.VerifyGenreCreated;

public record VerifyGenreCreatedRequest(string Id, string Name) : IRequest<bool>; 
