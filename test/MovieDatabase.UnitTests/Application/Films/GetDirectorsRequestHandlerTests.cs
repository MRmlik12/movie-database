using Shouldly;
using MovieDatabase.Api.Application.Films.GetDirectors;
using MovieDatabase.Api.Core.Documents.Films;
using MovieDatabase.Api.Infrastructure.Db.Repositories;
using NSubstitute;

namespace MovieDatabase.UnitTests.Application.Films;

public class GetDirectorsRequestHandlerTests
{
    private readonly IFilmRepository _mockFilmRepository;
    private readonly GetDirectorsRequestHandler _handler;

    public GetDirectorsRequestHandlerTests()
    {
        _mockFilmRepository = Substitute.For<IFilmRepository>();
        _handler = new GetDirectorsRequestHandler(_mockFilmRepository);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnAllDirectors()
    {
        var directors = new List<DirectorInfo>
        {
            new() { Name = "Christopher", Surname = "Nolan" },
            new() { Name = "Steven", Surname = "Spielberg" },
            new() { Name = "Quentin", Surname = "Tarantino" }
        };
        var request = new GetDirectorsRequest(null);

        _mockFilmRepository.GetDirectors(null)
            .Returns(Task.FromResult<IEnumerable<DirectorInfo>>(directors));

        var result = await _handler.HandleAsync(request);

        result.ShouldNotBeNull();
        result.Count().ShouldBe(3);
        await _mockFilmRepository.Received(1).GetDirectors(null);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnDistinctDirectors()
    {
        var directors = new List<DirectorInfo>
        {
            new() { Name = "Christopher", Surname = "Nolan" },
            new() { Name = "Christopher", Surname = "Nolan" },
            new() { Name = "Steven", Surname = "Spielberg" },
            new() { Name = "Steven", Surname = "Spielberg" }
        };
        var request = new GetDirectorsRequest(null);

        _mockFilmRepository.GetDirectors(null)
            .Returns(Task.FromResult<IEnumerable<DirectorInfo>>(directors));

        var result = await _handler.HandleAsync(request);

        result.Count().ShouldBe(2, "duplicates should be filtered out");
        result.ShouldContain(d => d.Name == "Christopher" && d.Surname == "Nolan");
        result.ShouldContain(d => d.Name == "Steven" && d.Surname == "Spielberg");
    }

    [Fact]
    public async Task HandleAsync_WithEmptyResult_ShouldReturnEmptyList()
    {
        var request = new GetDirectorsRequest(null);

        _mockFilmRepository.GetDirectors(null)
            .Returns(Task.FromResult<IEnumerable<DirectorInfo>>(new List<DirectorInfo>()));

        var result = await _handler.HandleAsync(request);

        result.ShouldNotBeNull();
        result.ShouldBeEmpty();
    }

    [Fact]
    public async Task HandleAsync_WithSearchTerm_ShouldPassToRepository()
    {
        const string searchTerm = "Nolan";
        var directors = new List<DirectorInfo>
        {
            new() { Name = "Christopher", Surname = "Nolan" }
        };
        var request = new GetDirectorsRequest(searchTerm);

        _mockFilmRepository.GetDirectors(searchTerm)
            .Returns(Task.FromResult<IEnumerable<DirectorInfo>>(directors));

        var result = await _handler.HandleAsync(request);

        result.Count().ShouldBe(1);
        result.First().Surname.ShouldBe("Nolan");
        await _mockFilmRepository.Received(1).GetDirectors(searchTerm);
    }

    [Fact]
    public async Task HandleAsync_ShouldMapDirectorProperties()
    {
        var director = new DirectorInfo
        {
            Id = Guid.NewGuid(),
            Name = "Martin",
            Surname = "Scorsese"
        };
        var request = new GetDirectorsRequest(null);

        _mockFilmRepository.GetDirectors(null)
            .Returns(Task.FromResult<IEnumerable<DirectorInfo>>(new List<DirectorInfo> { director }));

        var result = await _handler.HandleAsync(request);

        var directorDto = result.First();
        directorDto.Id.ShouldBe(director.Id.ToString());
        directorDto.Name.ShouldBe("Martin");
        directorDto.Surname.ShouldBe("Scorsese");
    }
}

