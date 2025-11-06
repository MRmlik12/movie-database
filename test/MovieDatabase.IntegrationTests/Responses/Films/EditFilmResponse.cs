using MovieDatabase.Api.Core.Dtos;
using MovieDatabase.Api.Core.Dtos.Films;

namespace MovieDatabase.IntegrationTests.Responses.Films;

public class EditFilmResponse
{
    public FilmDto EditFilm { get; set; } = null!;
}