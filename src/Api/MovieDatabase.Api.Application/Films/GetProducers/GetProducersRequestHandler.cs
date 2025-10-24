using MovieDatabase.Api.Core.Cqrs;
using MovieDatabase.Api.Core.Dtos;
using MovieDatabase.Api.Infrastructure.Db.Repositories;

namespace MovieDatabase.Api.Application.Films.GetProducers;

public class GetProducersRequestHandler(IFilmRepository filmRepository) : IRequestHandler<GetProducersRequest, IEnumerable<ProducerDto>>
{
    public async Task<IEnumerable<ProducerDto>> HandleAsync(GetProducersRequest request)
    {
        var producers = await filmRepository.GetProducers(request.SearchTerm);

        return producers.Select(ProducerDto.From).DistinctBy(x => x.Name).ToArray();
    }
}