using MovieDatabase.Api.Core.Cqrs;
using MovieDatabase.Api.Infrastructure.Db.Repositories;

namespace MovieDatabase.Api.Application.Actors.VerifyActorCreated;

internal class VerifyActorCreatedRequestHandler(IActorRepository actorRepository) : IRequestHandler<VerifyActorCreatedRequest, bool>
{
    public async Task<bool> HandleAsync(VerifyActorCreatedRequest request)
    {
        var existsById = await actorRepository.GetByIdAsync(request.Id) != null;
        if (existsById)
        {
            return true;
        }
        
        var existsByNameSurname = await actorRepository.GetByNameSurname(request.Name, request.Surname) != null;
        if (existsByNameSurname)
        {
            return true;
        }
        
        return false;
    }
}