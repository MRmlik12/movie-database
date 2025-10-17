namespace MovieDatabase.Api.Core.Exceptions;

public class RequestHandlerNotFoundException(string message = "Can't find registered request handler") : Exception(message);
