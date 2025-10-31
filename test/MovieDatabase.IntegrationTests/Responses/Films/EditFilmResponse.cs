using MovieDatabase.Api.Core.Dtos;

namespace MovieDatabase.IntegrationTests.Responses.Films;

public class EditFilmResponse
{
    public FilmDto EditFilm { get; set; } = null!;
}