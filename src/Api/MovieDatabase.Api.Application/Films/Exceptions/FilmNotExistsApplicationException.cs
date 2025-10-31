namespace MovieDatabase.Api.Application.Films.Exceptions;

public class FilmNotExistsApplicationException(string message = "Film not exist in database") : Exception(message);