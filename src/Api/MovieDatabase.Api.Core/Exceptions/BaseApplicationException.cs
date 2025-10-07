namespace MovieDatabase.Api.Core.Exceptions;

public class BaseApplicationException(string message = "Application error") : Exception(message);