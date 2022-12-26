using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace sharedmonitoring.Middlewares
{
    public class CorrelationMiddleware
    {

        private readonly RequestDelegate _next;
        private readonly string CorrelationHeaderName = "X-Correlation-Id";

        public CorrelationMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var headers = context.Request.Headers;
            string correlationId = string.Empty;

            if (headers.TryGetValue(CorrelationHeaderName, out StringValues correlationList))
            {
                correlationId = correlationList.FirstOrDefault(x => x.Equals(this.CorrelationHeaderName));
            }
            else
            {
                correlationId = Guid.NewGuid().ToString();
                context.Request.Headers.Add(this.CorrelationHeaderName, correlationId);
            }

            context.Response.OnStarting(() =>
            {
                if (!context.Response.Headers.TryGetValue(CorrelationHeaderName, out correlationList))
                {
                    context.Response.Headers.Add(CorrelationHeaderName, correlationId);
                }
                return Task.CompletedTask;
            });
            await _next(context).ConfigureAwait(false);
        }

    }
}
