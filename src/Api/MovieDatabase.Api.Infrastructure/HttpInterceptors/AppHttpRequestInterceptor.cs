using HotChocolate.AspNetCore;
using HotChocolate.AspNetCore.Subscriptions.Protocols;
using HotChocolate.Execution;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;

namespace MovieDatabase.Api.Infrastructure.HttpInterceptors;

public class AppHttpRequestInterceptor : DefaultHttpRequestInterceptor
{
    public override async ValueTask OnCreateAsync(HttpContext context, IRequestExecutor requestExecutor, OperationRequestBuilder requestBuilder,
        CancellationToken cancellationToken)
    {
        var result = await context.AuthenticateAsync(JwtBearerDefaults.AuthenticationScheme);
        if (!result.Succeeded)
        {
            context.Abort();
        }
        
        context.User = result.Principal;

        await base.OnCreateAsync(context, requestExecutor, requestBuilder, cancellationToken);
    }
}