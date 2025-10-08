using MovieDatabase.Api.Core.Exceptions;

namespace MovieDatabase.Api.Application.Films.Exceptions;

public class GenreNotFoundApplicationException(string message) : BaseApplicationException(message);
