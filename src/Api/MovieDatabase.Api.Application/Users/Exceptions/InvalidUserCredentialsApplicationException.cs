using MovieDatabase.Api.Core.Exceptions;

namespace MovieDatabase.Api.Application.Users.Exceptions;

public class InvalidUserCredentialsApplicationException(string message = "Wrong user credentials") : BaseApplicationException(message);