using Shouldly;
using MovieDatabase.Api.Application.Films.GetFilms;
using MovieDatabase.Api.Core.Documents.Films;
using MovieDatabase.Api.Infrastructure.Db.Repositories;
using MovieDatabase.UnitTests.Helpers;
using NSubstitute;

namespace MovieDatabase.UnitTests.Application.Films;

public class GetFilmsRequestHandlerTests
{
    private readonly IFilmRepository _mockFilmRepository;
    private readonly GetFilmsRequestHandler _handler;

    public GetFilmsRequestHandlerTests()
    {
        _mockFilmRepository = Substitute.For<IFilmRepository>();
        _handler = new GetFilmsRequestHandler(_mockFilmRepository);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnAllFilms()
    {
        var films = TestDataBuilder.CreateFilms(3);
        var request = new GetFilmsRequest(null);

        _mockFilmRepository.GetAll(null)
            .Returns(Task.FromResult<IEnumerable<Film>>(films));

        var result = await _handler.HandleAsync(request);

        result.ShouldNotBeNull();
        result.Count().ShouldBe(3);
        await _mockFilmRepository.Received(1).GetAll(null);
    }

    [Fact]
    public async Task HandleAsync_WithEmptyDatabase_ShouldReturnEmptyList()
    {
        var request = new GetFilmsRequest(null);

        _mockFilmRepository.GetAll(null)
            .Returns(Task.FromResult<IEnumerable<Film>>(new List<Film>()));

        var result = await _handler.HandleAsync(request);

        result.ShouldNotBeNull();
        result.ShouldBeEmpty();
        await _mockFilmRepository.Received(1).GetAll(null);
    }

    [Fact]
    public async Task HandleAsync_ShouldMapToFilmDto()
    {
        var film = TestDataBuilder.CreateValidFilm(
            title: "Test Film",
            description: "Test Description"
        );
        var films = new List<Film> { film };
        var request = new GetFilmsRequest(null);

        _mockFilmRepository.GetAll(null)
            .Returns(Task.FromResult<IEnumerable<Film>>(films));

        var result = await _handler.HandleAsync(request);

        var filmDto = result.First();
        filmDto.ShouldNotBeNull();
        filmDto.Title.ShouldBe("Test Film");
        filmDto.Description.ShouldBe("Test Description");
        filmDto.Id.ShouldBe(film.Id.ToString());
    }

    [Fact]
    public async Task HandleAsync_ShouldCallRepositoryGetAllOnce()
    {
        var films = TestDataBuilder.CreateFilms(2);
        var request = new GetFilmsRequest(null);

        _mockFilmRepository.GetAll(null)
            .Returns(Task.FromResult<IEnumerable<Film>>(films));

        await _handler.HandleAsync(request);

        await _mockFilmRepository.Received(1).GetAll(null);
    }

    [Fact]
    public async Task HandleAsync_WithSearchTerm_ShouldPassToRepository()
    {
        const string searchTerm = "Matrix";
        var films = new List<Film> { TestDataBuilder.CreateValidFilm(title: "The Matrix") };
        var request = new GetFilmsRequest(searchTerm);

        _mockFilmRepository.GetAll(searchTerm)
            .Returns(Task.FromResult<IEnumerable<Film>>(films));

        var result = await _handler.HandleAsync(request);

        result.Count().ShouldBe(1);
        result.First().Title.ShouldBe("The Matrix");
        await _mockFilmRepository.Received(1).GetAll(searchTerm);
    }

    [Fact]
    public async Task HandleAsync_ShouldMapAllFilmProperties()
    {
        var film = TestDataBuilder.CreateValidFilm(
            title: "Complete Film",
            releaseDate: new DateOnly(2024, 5, 15),
            actors: TestDataBuilder.CreateActors(("John", "Doe"), ("Jane", "Smith")),
            genres: TestDataBuilder.CreateGenres("Action", "Drama")
        );
        var request = new GetFilmsRequest(null);

        _mockFilmRepository.GetAll(null)
            .Returns(Task.FromResult<IEnumerable<Film>>(new List<Film> { film }));

        var result = await _handler.HandleAsync(request);

        var filmDto = result.First();
        filmDto.Title.ShouldBe("Complete Film");
        filmDto.ReleaseDate.ShouldBe(new DateOnly(2024, 5, 15));
        filmDto.Actors.Length.ShouldBe(2);
        filmDto.Genres.Length.ShouldBe(2);
    }
}

