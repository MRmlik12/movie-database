using MovieDatabase.Api.Core.Cqrs;

namespace MovieDatabase.Api.Application.Actors.VerifyActorCreated;

public record VerifyActorCreatedRequest(string Id, string Name, string Surname) : IRequest<bool>;