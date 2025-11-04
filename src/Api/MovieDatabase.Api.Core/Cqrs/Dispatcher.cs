using MovieDatabase.Api.Core.Exceptions;
using MovieDatabase.Api.Core.Exceptions.Cqrs;

namespace MovieDatabase.Api.Core.Cqrs;

public class Dispatcher(IServiceProvider provider) : IDispatcher
{
    public async Task<TResponse> Dispatch<TResponse>(IRequest<TResponse> request)
    {
        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));
        dynamic handler = provider.GetService(handlerType);

        if (handler is null)
        {
            throw new RequestHandlerNotFoundException();
        }

        return await handler.HandleAsync((dynamic)request);
    }
}