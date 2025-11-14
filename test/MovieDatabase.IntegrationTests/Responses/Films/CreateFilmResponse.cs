using MovieDatabase.Api.Core.Dtos.Films;

namespace MovieDatabase.IntegrationTests.Responses.Films;

public class CreateFilmResponse
{
    public FilmDto CreateFilm { get; set; } = null!;
}