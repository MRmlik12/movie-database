namespace MovieDatabase.Api.Core.Cqrs;

public class Dispatcher : IDispatcher
{
    public Task<TResponse> Dispatch<TResponse>(IRequest<TResponse> request)
    {
        throw new NotImplementedException();
    }
}