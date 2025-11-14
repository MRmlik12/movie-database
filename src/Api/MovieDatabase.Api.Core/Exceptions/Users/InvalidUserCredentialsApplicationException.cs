namespace MovieDatabase.Api.Core.Exceptions.Users;

public class InvalidUserCredentialsApplicationException(string message = "Wrong user credentials") : BaseApplicationException(message);