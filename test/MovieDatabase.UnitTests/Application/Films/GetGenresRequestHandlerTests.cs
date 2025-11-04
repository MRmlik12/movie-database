using Shouldly;
using MovieDatabase.Api.Application.Films.GetGenres;
using MovieDatabase.Api.Core.Documents.Films;
using MovieDatabase.Api.Infrastructure.Db.Repositories;
using MovieDatabase.UnitTests.Helpers;
using NSubstitute;

namespace MovieDatabase.UnitTests.Application.Films;

public class GetGenresRequestHandlerTests
{
    private readonly IFilmRepository _mockFilmRepository;
    private readonly GetGenresRequestHandler _handler;

    public GetGenresRequestHandlerTests()
    {
        _mockFilmRepository = Substitute.For<IFilmRepository>();
        _handler = new GetGenresRequestHandler(_mockFilmRepository);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnAllGenres()
    {
        var genres = TestDataBuilder.CreateGenres("Action", "Drama", "Comedy", "Thriller");
        var request = new GetGenresRequest();

        _mockFilmRepository.GetGenres(Arg.Any<string>(), Arg.Any<int?>())
            .Returns(Task.FromResult<IEnumerable<Genre>>(genres));

        var result = await _handler.HandleAsync(request);

        result.ShouldNotBeNull();
        result.Count().ShouldBe(4);
        await _mockFilmRepository.Received(1).GetGenres(Arg.Any<string>(), Arg.Any<int?>());
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnDistinctGenres()
    {
        var genres = new List<Genre>
        {
            new() { Name = "Action" },
            new() { Name = "Drama" },
            new() { Name = "Comedy" }
        };
        var request = new GetGenresRequest();

        _mockFilmRepository.GetGenres(Arg.Any<string>(), Arg.Any<int?>())
            .Returns(Task.FromResult<IEnumerable<Genre>>(genres));

        var result = await _handler.HandleAsync(request);

        result.Count().ShouldBe(3, "duplicates should be filtered out");
        result.ShouldContain(g => g.Name == "Action");
        result.ShouldContain(g => g.Name == "Drama");
        result.ShouldContain(g => g.Name == "Comedy");
    }

    [Fact]
    public async Task HandleAsync_WithEmptyResult_ShouldReturnEmptyList()
    {
        var request = new GetGenresRequest();

        _mockFilmRepository.GetGenres(Arg.Any<string>(), Arg.Any<int?>())
            .Returns(Task.FromResult<IEnumerable<Genre>>(new List<Genre>()));

        var result = await _handler.HandleAsync(request);

        result.ShouldNotBeNull();
        result.ShouldBeEmpty();
    }

    [Fact]
    public async Task HandleAsync_WithSearchTerm_ShouldPassToRepository()
    {
        const string searchTerm = "Act";
        var genres = new List<Genre> { new() { Name = "Action" } };
        var request = new GetGenresRequest(searchTerm);

        _mockFilmRepository.GetGenres(searchTerm, Arg.Any<int?>())
            .Returns(Task.FromResult<IEnumerable<Genre>>(genres));

        var result = await _handler.HandleAsync(request);

        result.Count().ShouldBe(1);
        result.First().Name.ShouldBe("Action");
        await _mockFilmRepository.Received(1).GetGenres(searchTerm, Arg.Any<int?>());
    }

    [Fact]
    public async Task HandleAsync_ShouldMapGenreProperties()
    {
        var genre = new Genre
        {
            Id = Guid.NewGuid(),
            Name = "Science Fiction"
        };
        var request = new GetGenresRequest();

        _mockFilmRepository.GetGenres(Arg.Any<string>(), Arg.Any<int?>())
            .Returns(Task.FromResult<IEnumerable<Genre>>(new List<Genre> { genre }));

        var result = await _handler.HandleAsync(request);

        var genreDto = result.First();
        genreDto.Id.ShouldBe(genre.Id.ToString());
        genreDto.Name.ShouldBe("Science Fiction");
    }
}

