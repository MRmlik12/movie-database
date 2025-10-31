using Shouldly;
using MovieDatabase.Api.Application.Films.GetProducers;
using MovieDatabase.Api.Core.Documents.Films;
using MovieDatabase.Api.Infrastructure.Db.Repositories;
using NSubstitute;

namespace MovieDatabase.UnitTests.Application.Films;

public class GetProducersRequestHandlerTests
{
    private readonly IFilmRepository _mockFilmRepository;
    private readonly GetProducersRequestHandler _handler;

    public GetProducersRequestHandlerTests()
    {
        _mockFilmRepository = Substitute.For<IFilmRepository>();
        _handler = new GetProducersRequestHandler(_mockFilmRepository);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnAllProducers()
    {
        var producers = new List<ProducerInfo>
        {
            new() { Name = "Warner Bros" },
            new() { Name = "Universal Pictures" },
            new() { Name = "Paramount Pictures" }
        };
        var request = new GetProducersRequest(null);

        _mockFilmRepository.GetProducers(null)
            .Returns(Task.FromResult<IEnumerable<ProducerInfo>>(producers));

        var result = await _handler.HandleAsync(request);

        result.ShouldNotBeNull();
        result.Count().ShouldBe(3);
        await _mockFilmRepository.Received(1).GetProducers(null);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnDistinctProducers()
    {
        var producers = new List<ProducerInfo>
        {
            new() { Name = "Warner Bros" },
            new() { Name = "Warner Bros" },
            new() { Name = "Universal Pictures" },
            new() { Name = "Universal Pictures" },
            new() { Name = "Disney" }
        };
        var request = new GetProducersRequest(null);

        _mockFilmRepository.GetProducers(null)
            .Returns(Task.FromResult<IEnumerable<ProducerInfo>>(producers));

        var result = await _handler.HandleAsync(request);

        result.Count().ShouldBe(3, "duplicates should be filtered out");
        result.ShouldContain(p => p.Name == "Warner Bros");
        result.ShouldContain(p => p.Name == "Universal Pictures");
        result.ShouldContain(p => p.Name == "Disney");
    }

    [Fact]
    public async Task HandleAsync_WithEmptyResult_ShouldReturnEmptyList()
    {
        var request = new GetProducersRequest(null);

        _mockFilmRepository.GetProducers(null)
            .Returns(Task.FromResult<IEnumerable<ProducerInfo>>(new List<ProducerInfo>()));

        var result = await _handler.HandleAsync(request);

        result.ShouldNotBeNull();
        result.ShouldBeEmpty();
    }

    [Fact]
    public async Task HandleAsync_WithSearchTerm_ShouldPassToRepository()
    {
        const string searchTerm = "Warner";
        var producers = new List<ProducerInfo>
        {
            new() { Name = "Warner Bros" }
        };
        var request = new GetProducersRequest(searchTerm);

        _mockFilmRepository.GetProducers(searchTerm)
            .Returns(Task.FromResult<IEnumerable<ProducerInfo>>(producers));

        var result = await _handler.HandleAsync(request);

        result.Count().ShouldBe(1);
        result.First().Name.ShouldBe("Warner Bros");
        await _mockFilmRepository.Received(1).GetProducers(searchTerm);
    }

    [Fact]
    public async Task HandleAsync_ShouldMapProducerProperties()
    {
        var producer = new ProducerInfo
        {
            Id = Guid.NewGuid(),
            Name = "20th Century Fox"
        };
        var request = new GetProducersRequest(null);

        _mockFilmRepository.GetProducers(null)
            .Returns(Task.FromResult<IEnumerable<ProducerInfo>>(new List<ProducerInfo> { producer }));

        var result = await _handler.HandleAsync(request);

        var producerDto = result.First();
        producerDto.Id.ShouldBe(producer.Id.ToString());
        producerDto.Name.ShouldBe("20th Century Fox");
    }
}

