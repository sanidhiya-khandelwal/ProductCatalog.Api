using System.Diagnostics;

namespace ProductCatalog.Api.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            _logger.LogInformation("Handling request: {method} {url}",
                   context.Request.Method, context.Request.Path);

            //Call the next middleware in the pipeline
            await _next(context);

            stopwatch.Stop();
            _logger.LogInformation("Finished handling request. Time Taken: {time} ms",
                stopwatch.ElapsedMilliseconds);
        }
    }
}
