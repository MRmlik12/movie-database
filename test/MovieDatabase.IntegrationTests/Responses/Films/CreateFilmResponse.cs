using MovieDatabase.Api.Core.Dtos;

namespace MovieDatabase.IntegrationTests.Responses.Films;

public class CreateFilmResponse
{
    public FilmDto CreateFilm { get; set; } = null!;
}