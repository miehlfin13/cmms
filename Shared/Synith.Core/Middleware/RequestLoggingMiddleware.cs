﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Synith.Core.Middleware;
public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        _logger.LogInformation("Request has been received: {Method} {Path}", context.Request.Method, context.Request.Path);
        await _next(context);
    }
}
