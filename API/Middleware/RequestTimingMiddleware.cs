using System;
using System.Diagnostics;

namespace API.Middleware;

public class RequestTimingMiddleware
{
     private readonly RequestDelegate _next;
    private readonly ILogger<RequestTimingMiddleware> _logger;
    public RequestTimingMiddleware(RequestDelegate next, ILogger<RequestTimingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

        public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();

        try
        {
            await _next(context);
        }
        finally
        {
           
            stopwatch.Stop();
            _logger.LogInformation(
                "Request {Path} took {ElapsedMilliseconds}ms Ticks {ElapsedTicks} Elapsed {Elapsed}",
                context.Request.Path,
                stopwatch.ElapsedMilliseconds,
                stopwatch.ElapsedTicks,
                 stopwatch.Elapsed
                );
        }
    }
}
