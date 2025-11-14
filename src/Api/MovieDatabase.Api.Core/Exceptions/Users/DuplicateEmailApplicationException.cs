namespace MovieDatabase.Api.Core.Exceptions.Users;

public class DuplicateEmailApplicationException(string message = "User with this email already exists") : BaseApplicationException(message);