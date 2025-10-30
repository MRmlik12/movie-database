using Shouldly;
using MovieDatabase.Api.Application.Films.DeleteFilm;
using MovieDatabase.Api.Application.Films.Exceptions;
using MovieDatabase.Api.Core.Documents.Films;
using MovieDatabase.Api.Infrastructure.Db.Repositories;
using MovieDatabase.UnitTests.Helpers;
using NSubstitute;

namespace MovieDatabase.UnitTests.Application.Films;

public class DeleteFilmRequestHandlerTests
{
    private readonly IFilmRepository _mockFilmRepository;
    private readonly DeleteFilmRequestHandler _handler;

    public DeleteFilmRequestHandlerTests()
    {
        _mockFilmRepository = Substitute.For<IFilmRepository>();
        _handler = new DeleteFilmRequestHandler(_mockFilmRepository);
    }

    [Fact]
    public async Task HandleAsync_WithExistingFilm_ShouldDeleteFilm()
    {
        var filmId = Guid.NewGuid();
        var filmIdString = filmId.ToString();
        var userId = Guid.NewGuid().ToString();
        var existingFilm = TestDataBuilder.CreateValidFilm(id: filmId);
        var request = new DeleteFilmRequest(filmIdString, userId);

        _mockFilmRepository.GetById(filmIdString)
            .Returns(Task.FromResult<Film?>(existingFilm));

        var result = await _handler.HandleAsync(request);

        result.ShouldBe(filmIdString);
        await _mockFilmRepository.Received(1).Delete(Arg.Is<Film>(f => f.Id == filmId));
    }

    [Fact]
    public async Task HandleAsync_WithNonExistentFilm_ShouldThrowFilmNotFoundException()
    {
        var filmIdString = Guid.NewGuid().ToString();
        var userId = Guid.NewGuid().ToString();
        var request = new DeleteFilmRequest(filmIdString, userId);

        _mockFilmRepository.GetById(filmIdString)
            .Returns(Task.FromResult<Film?>(null));

        await Assert.ThrowsAsync<FilmNotExistsApplicationException>(
            () => _handler.HandleAsync(request)
        );

        await _mockFilmRepository.DidNotReceive().Delete(Arg.Any<Film>());
    }

    [Fact]
    public async Task HandleAsync_ShouldCallRepositoryDeleteOnce()
    {
        var filmId = Guid.NewGuid();
        var filmIdString = filmId.ToString();
        var userId = Guid.NewGuid().ToString();
        var existingFilm = TestDataBuilder.CreateValidFilm(id: filmId);
        var request = new DeleteFilmRequest(filmIdString, userId);

        _mockFilmRepository.GetById(filmIdString)
            .Returns(Task.FromResult<Film?>(existingFilm));

        await _handler.HandleAsync(request);

        await _mockFilmRepository.Received(1).Delete(Arg.Any<Film>());
        await _mockFilmRepository.Received(1).GetById(filmIdString);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnFilmId()
    {
        var filmId = Guid.NewGuid();
        var filmIdString = filmId.ToString();
        var userId = Guid.NewGuid().ToString();
        var existingFilm = TestDataBuilder.CreateValidFilm(id: filmId);
        var request = new DeleteFilmRequest(filmIdString, userId);

        _mockFilmRepository.GetById(filmIdString)
            .Returns(Task.FromResult<Film?>(existingFilm));

        var result = await _handler.HandleAsync(request);

        result.ShouldNotBeNullOrEmpty();
        result.ShouldBe(filmIdString);
        Guid.TryParse(result, out _).ShouldBeTrue("result should be a valid GUID string");
    }

    [Theory]
    [InlineData("00000000-0000-0000-0000-000000000001")]
    [InlineData("12345678-1234-1234-1234-123456789012")]
    [InlineData("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee")]
    public async Task HandleAsync_WithVariousFilmIds_ShouldHandleCorrectly(string guidString)
    {
        var filmId = Guid.Parse(guidString);
        var userId = Guid.NewGuid().ToString();
        var existingFilm = TestDataBuilder.CreateValidFilm(id: filmId);
        var request = new DeleteFilmRequest(guidString, userId);

        _mockFilmRepository.GetById(guidString)
            .Returns(Task.FromResult<Film?>(existingFilm));

        var result = await _handler.HandleAsync(request);

        result.ShouldBe(guidString);
        await _mockFilmRepository.Received(1).Delete(Arg.Is<Film>(f => f.Id == filmId));
    }
}

