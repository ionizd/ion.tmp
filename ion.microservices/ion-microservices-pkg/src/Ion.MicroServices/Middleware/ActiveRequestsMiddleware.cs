﻿using Ion.MicroServices.Lifecycle;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Ion.Middleware;

public class ActiveRequestsMiddleware
{
    private readonly IActiveRequestsService service;
    private readonly RequestDelegate next;

    public ActiveRequestsMiddleware(IActiveRequestsService service, RequestDelegate next, ILogger<ReadinessMiddleware> logger)
    {
        this.service = service ?? throw new ArgumentNullException(nameof(service));
        this.next = next ?? throw new ArgumentNullException(nameof(next));
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            service.Increment();
            await next.Invoke(context);
        }
        finally
        {
            service.Decrement();
        }
    }
}