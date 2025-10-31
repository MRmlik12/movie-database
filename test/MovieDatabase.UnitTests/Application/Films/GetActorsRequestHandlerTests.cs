using Shouldly;
using MovieDatabase.Api.Application.Films.GetActors;
using MovieDatabase.Api.Core.Documents.Films;
using MovieDatabase.Api.Infrastructure.Db.Repositories;
using MovieDatabase.UnitTests.Helpers;
using NSubstitute;

namespace MovieDatabase.UnitTests.Application.Films;

public class GetActorsRequestHandlerTests
{
    private readonly IFilmRepository _mockFilmRepository;
    private readonly GetActorsRequestHandler _handler;

    public GetActorsRequestHandlerTests()
    {
        _mockFilmRepository = Substitute.For<IFilmRepository>();
        _handler = new GetActorsRequestHandler(_mockFilmRepository);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnAllActors()
    {
        var actors = TestDataBuilder.CreateActors(
            ("John", "Doe"),
            ("Jane", "Smith"),
            ("Bob", "Johnson")
        );
        var request = new GetActorsRequest(null);

        _mockFilmRepository.GetActors(null)
            .Returns(Task.FromResult<IEnumerable<Actor>>(actors));

        var result = await _handler.HandleAsync(request);

        result.ShouldNotBeNull();
        result.Count().ShouldBe(3);
        await _mockFilmRepository.Received(1).GetActors(null);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnDistinctActors()
    {
        var actors = new List<Actor>
        {
            new() { Name = "John", Surname = "Doe" },
            new() { Name = "John", Surname = "Doe" },
            new() { Name = "Jane", Surname = "Smith" },
            new() { Name = "Jane", Surname = "Smith" }
        };
        var request = new GetActorsRequest(null);

        _mockFilmRepository.GetActors(null)
            .Returns(Task.FromResult<IEnumerable<Actor>>(actors));

        var result = await _handler.HandleAsync(request);

        result.Count().ShouldBe(2, "duplicates should be filtered out");
        result.ShouldContain(a => a.Name == "John" && a.Surname == "Doe");
        result.ShouldContain(a => a.Name == "Jane" && a.Surname == "Smith");
    }

    [Fact]
    public async Task HandleAsync_WithEmptyResult_ShouldReturnEmptyList()
    {
        var request = new GetActorsRequest(null);

        _mockFilmRepository.GetActors(null)
            .Returns(Task.FromResult<IEnumerable<Actor>>(new List<Actor>()));

        var result = await _handler.HandleAsync(request);

        result.ShouldNotBeNull();
        result.ShouldBeEmpty();
    }

    [Fact]
    public async Task HandleAsync_WithSearchTerm_ShouldPassToRepository()
    {
        const string searchTerm = "John";
        var actors = new List<Actor> { new() { Name = "John", Surname = "Doe" } };
        var request = new GetActorsRequest(searchTerm);

        _mockFilmRepository.GetActors(searchTerm)
            .Returns(Task.FromResult<IEnumerable<Actor>>(actors));

        var result = await _handler.HandleAsync(request);

        result.Count().ShouldBe(1);
        result.First().Name.ShouldBe("John");
        await _mockFilmRepository.Received(1).GetActors(searchTerm);
    }

    [Fact]
    public async Task HandleAsync_ShouldMapActorProperties()
    {
        var actor = new Actor
        {
            Id = Guid.NewGuid(),
            Name = "Robert",
            Surname = "Downey Jr."
        };
        var request = new GetActorsRequest(null);

        _mockFilmRepository.GetActors(null)
            .Returns(Task.FromResult<IEnumerable<Actor>>(new List<Actor> { actor }));

        var result = await _handler.HandleAsync(request);

        var actorDto = result.First();
        actorDto.Id.ShouldBe(actor.Id.ToString());
        actorDto.Name.ShouldBe("Robert");
        actorDto.Surname.ShouldBe("Downey Jr.");
    }
}

