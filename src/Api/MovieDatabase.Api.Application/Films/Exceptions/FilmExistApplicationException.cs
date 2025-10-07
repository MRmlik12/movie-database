using MovieDatabase.Api.Core.Exceptions;

namespace MovieDatabase.Api.Application.Films.Exceptions;

public class FilmExistApplicationException(string message = "Film with the same title exists") : BaseApplicationException(message);