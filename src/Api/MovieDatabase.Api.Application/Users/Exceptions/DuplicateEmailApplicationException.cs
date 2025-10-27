using MovieDatabase.Api.Core.Exceptions;

namespace MovieDatabase.Api.Application.Users.Exceptions;

public class DuplicateEmailApplicationException(string message = "User with this email already exists") : BaseApplicationException(message);

