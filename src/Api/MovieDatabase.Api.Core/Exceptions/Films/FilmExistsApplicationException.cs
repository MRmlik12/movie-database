namespace MovieDatabase.Api.Core.Exceptions.Films;

public class FilmExistsApplicationException(string message = "Film with the same title exists") : BaseApplicationException(message);