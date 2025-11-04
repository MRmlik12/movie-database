using Shouldly;
using MovieDatabase.Api.Application.Films.CreateFilm;
using MovieDatabase.Api.Core.Documents.Films;
using MovieDatabase.Api.Core.Exceptions.Films;
using MovieDatabase.Api.Infrastructure.Db.Repositories;
using MovieDatabase.UnitTests.Helpers;
using NSubstitute;

namespace MovieDatabase.UnitTests.Application.Films;

public class CreateFilmRequestHandlerTests
{
    private readonly IFilmRepository _mockFilmRepository;
    private readonly CreateFilmRequestHandler _handler;

    public CreateFilmRequestHandlerTests()
    {
        _mockFilmRepository = Substitute.For<IFilmRepository>();
        _handler = new CreateFilmRequestHandler(_mockFilmRepository);
    }

    [Fact]
    public async Task HandleAsync_WithValidData_ShouldCreateFilm()
    {
        var request = CreateValidRequest();
        _mockFilmRepository.GetByTitle(Arg.Any<string>())
            .Returns(Task.FromResult<Film?>(null));

        var result = await _handler.HandleAsync(request);

        result.ShouldNotBeNull();
        await _mockFilmRepository.Received(1).Add(Arg.Is<Film>(f =>
            f.Title == request.Title &&
            f.ReleaseDate == request.ReleaseDate));
    }

    [Fact]
    public async Task HandleAsync_WithValidData_ShouldReturnFilmDto()
    {
        var request = CreateValidRequest();
        _mockFilmRepository.GetByTitle(Arg.Any<string>())
            .Returns(Task.FromResult<Film?>(null));

        var result = await _handler.HandleAsync(request);

        result.ShouldNotBeNull();
        result.Title.ShouldBe(request.Title);
        result.ReleaseDate.ShouldBe(request.ReleaseDate);
        result.Description.ShouldBe(request.Description);
        result.Actors.Length.ShouldBe(request.Actors.Length);
        result.Genres.Length.ShouldBe(request.Genres.Length);
        result.Director.ShouldNotBeNull();
        result.Producer.ShouldNotBeNull();
    }

    [Fact]
    public async Task HandleAsync_WithDuplicateTitle_ShouldThrowFilmExistsException()
    {
        var request = CreateValidRequest();
        var existingFilm = TestDataBuilder.CreateValidFilm(title: request.Title);

        _mockFilmRepository.GetByTitle(request.Title)
            .Returns(Task.FromResult<Film?>(existingFilm));

        await Assert.ThrowsAsync<FilmExistsApplicationException>(
            () => _handler.HandleAsync(request)
        );

        await _mockFilmRepository.DidNotReceive().Add(Arg.Any<Film>());
    }

    [Fact]
    public async Task HandleAsync_ShouldTrimTitleWhitespace()
    {
        var request = new CreateFilmRequest(
            Title: "  Test Film  ",
            ReleaseDate: new DateOnly(2024, 1, 1),
            Description: "Test",
            Actors: new[] { new CreateFilmRequest.ActorPlaceholder(null, "John", "Doe") },
            Genres: new[] { new CreateFilmRequest.GenrePlaceholder(null, "Drama") },
            Director: new CreateFilmRequest.DirectorPlaceholder(null, "Jane", "Smith"),
            Producer: new CreateFilmRequest.ProducerPlaceholder(null, "Test Studios")
        ) { CreatorId = Guid.NewGuid().ToString() };

        _mockFilmRepository.GetByTitle(Arg.Any<string>())
            .Returns(Task.FromResult<Film?>(null));

        await _handler.HandleAsync(request);

        await _mockFilmRepository.Received(1).Add(Arg.Is<Film>(f =>
            f.Title == "Test Film" &&
            !f.Title.StartsWith(" ") &&
            !f.Title.EndsWith(" ")));
    }

    [Fact]
    public async Task HandleAsync_ShouldTrimDescriptionWhitespace()
    {
        var request = CreateValidRequest() with { Description = "  Test Description  " };
        _mockFilmRepository.GetByTitle(Arg.Any<string>())
            .Returns(Task.FromResult<Film?>(null));

        await _handler.HandleAsync(request);

        await _mockFilmRepository.Received(1).Add(Arg.Is<Film>(f =>
            f.Description == "Test Description" &&
            !f.Description.StartsWith(" ") &&
            !f.Description.EndsWith(" ")));
    }

    [Fact]
    public async Task HandleAsync_WithNullDescription_ShouldSetEmptyString()
    {
        var request = CreateValidRequest() with { Description = null };
        _mockFilmRepository.GetByTitle(Arg.Any<string>())
            .Returns(Task.FromResult<Film?>(null));

        await _handler.HandleAsync(request);

        await _mockFilmRepository.Received(1).Add(Arg.Is<Film>(f =>
            f.Description == string.Empty));
    }

    [Fact]
    public async Task HandleAsync_ShouldMapActorsCorrectly()
    {
        var request = new CreateFilmRequest(
            Title: "Test Film",
            ReleaseDate: new DateOnly(2024, 1, 1),
            Description: "Test",
            Actors: new[]
            {
                new CreateFilmRequest.ActorPlaceholder(null, "John", "Doe"),
                new CreateFilmRequest.ActorPlaceholder(null, "Jane", "Smith")
            },
            Genres: new[] { new CreateFilmRequest.GenrePlaceholder(null, "Drama") },
            Director: new CreateFilmRequest.DirectorPlaceholder(null, "Director", "Name"),
            Producer: new CreateFilmRequest.ProducerPlaceholder(null, "Producer")
        ) { CreatorId = Guid.NewGuid().ToString() };

        _mockFilmRepository.GetByTitle(Arg.Any<string>())
            .Returns(Task.FromResult<Film?>(null));

        await _handler.HandleAsync(request);

        await _mockFilmRepository.Received(1).Add(Arg.Is<Film>(f =>
            f.Actors.Count == 2 &&
            f.Actors[0].Name == "John" &&
            f.Actors[0].Surname == "Doe" &&
            f.Actors[1].Name == "Jane" &&
            f.Actors[1].Surname == "Smith"));
    }

    [Fact]
    public async Task HandleAsync_ShouldMapGenresCorrectly()
    {
        var request = new CreateFilmRequest(
            "Test Film",
            new DateOnly(2024, 1, 1),
             "Test",
            Actors: [new CreateFilmRequest.ActorPlaceholder(null, "John", "Doe")],
            Genres:
            [
                new CreateFilmRequest.GenrePlaceholder(null, "Drama"),
                new CreateFilmRequest.GenrePlaceholder(null, "Thriller")
            ],
            Director: new CreateFilmRequest.DirectorPlaceholder(null, "Director", "Name"),
            Producer: new CreateFilmRequest.ProducerPlaceholder(null, "Producer")
        ) { CreatorId = Guid.NewGuid().ToString() };

        _mockFilmRepository.GetByTitle(Arg.Any<string>())
            .Returns(Task.FromResult<Film?>(null));

        await _handler.HandleAsync(request);

        await _mockFilmRepository.Received(1).Add(Arg.Is<Film>(f =>
            f.Genres.Count == 2 &&
            f.Genres[0].Name == "Drama" &&
            f.Genres[1].Name == "Thriller"));
    }

    [Fact]
    public async Task HandleAsync_ShouldMapDirectorCorrectly()
    {
        var request = CreateValidRequest();
        _mockFilmRepository.GetByTitle(Arg.Any<string>())
            .Returns(Task.FromResult<Film?>(null));

        await _handler.HandleAsync(request);

        await _mockFilmRepository.Received(1).Add(Arg.Is<Film>(f =>
            f.Director.Name == request.Director.Name &&
            f.Director.Surname == request.Director.Surname));
    }

    [Fact]
    public async Task HandleAsync_ShouldMapProducerCorrectly()
    {
        var request = CreateValidRequest();
        _mockFilmRepository.GetByTitle(Arg.Any<string>())
            .Returns(Task.FromResult<Film?>(null));

        await _handler.HandleAsync(request);

        await _mockFilmRepository.Received(1).Add(Arg.Is<Film>(f =>
            f.Producer.Name == request.Producer.Name));
    }

    [Fact]
    public async Task HandleAsync_ShouldSetCreatorId()
    {
        var creatorId = Guid.NewGuid().ToString();
        var request = CreateValidRequest() with { CreatorId = creatorId };
        _mockFilmRepository.GetByTitle(Arg.Any<string>())
            .Returns(Task.FromResult<Film?>(null));

        await _handler.HandleAsync(request);

        await _mockFilmRepository.Received(1).Add(Arg.Is<Film>(f =>
            f.CreatorId == creatorId));
    }

    [Fact]
    public async Task HandleAsync_ShouldCallRepositoryAddOnce()
    {
        var request = CreateValidRequest();
        _mockFilmRepository.GetByTitle(Arg.Any<string>())
            .Returns(Task.FromResult<Film?>(null));

        await _handler.HandleAsync(request);

        await _mockFilmRepository.Received(1).Add(Arg.Any<Film>());
        await _mockFilmRepository.Received(1).GetByTitle(Arg.Any<string>());
    }

    [Theory]
    [InlineData("The Matrix")]
    [InlineData("Inception")]
    [InlineData("The Shawshank Redemption")]
    public async Task HandleAsync_WithVariousTitles_ShouldSucceed(string title)
    {
        var request = CreateValidRequest() with { Title = title };
        _mockFilmRepository.GetByTitle(Arg.Any<string>())
            .Returns(Task.FromResult<Film?>(null));

        var result = await _handler.HandleAsync(request);

        result.ShouldNotBeNull();
        result.Title.ShouldBe(title);
    }

    [Fact]
    public async Task HandleAsync_WithEmptyActorsList_ShouldCreateFilmWithNoActors()
    {
        var request = CreateValidRequest() with
        {
            Actors = []
        };
        _mockFilmRepository.GetByTitle(Arg.Any<string>())
            .Returns(Task.FromResult<Film?>(null));

        var result = await _handler.HandleAsync(request);

        result.ShouldNotBeNull();
        result.Actors.ShouldBeEmpty();
    }

    private static CreateFilmRequest CreateValidRequest()
    {
        return new CreateFilmRequest(
            Title: "Test Film",
            ReleaseDate: new DateOnly(2024, 1, 1),
            Description: "Test Description",
            Actors:
            [
                new CreateFilmRequest.ActorPlaceholder(null, "John", "Doe")
            ],
            Genres:
            [
                new CreateFilmRequest.GenrePlaceholder(null, "Drama")
            ],
            Director: new CreateFilmRequest.DirectorPlaceholder(null, "Jane", "Smith"),
            Producer: new CreateFilmRequest.ProducerPlaceholder(null, "Test Studios")
        )
        {
            CreatorId = Guid.NewGuid().ToString()
        };
    }
}

