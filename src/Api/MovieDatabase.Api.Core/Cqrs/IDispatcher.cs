namespace MovieDatabase.Api.Core.Cqrs;

public interface IDispatcher
{
    Task<TResponse> Dispatch<TResponse>(IRequest<TResponse> request);
}