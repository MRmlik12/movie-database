using MovieDatabase.Api.Core.Cqrs;
using MovieDatabase.Api.Infrastructure.Db.Repositories;

namespace MovieDatabase.Api.Application.Actors.VerifyActorCreated;

public class VerifyActorCreatedRequestHandler(IActorRepository actorRepository) : IRequestHandler<VerifyActorCreatedRequest, bool>
{
    public async Task<bool> Handle(VerifyActorCreatedRequest request)
    {
        var existsById = await actorRepository.GetByIdAsync(request.Id) != null;
        if (existsById)
        {
            return true;
        }
        
        var existsByName = await actorRepository.GetByNameSurname(request.Name, request.Surname) != null;
        
        return false;
    }
}