using Microsoft.AspNetCore.Builder;

namespace sharedmonitoring.Middlewares
{
    public static class MiddlewareExtensions
    { 

        public static IApplicationBuilder UseCorrelationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CorrelationMiddleware>();
        }
    }
}
