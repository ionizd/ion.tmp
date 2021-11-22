using Ion.Middleware;
using Microsoft.AspNetCore.Builder;
using System;

namespace Ion.Extensions;

public static class IApplicationBuilderExtensions
{
    public static Action<IApplicationBuilder> UseMicroServiceLifetimeMiddleware = (app) =>
    {
        app.UseMiddleware<StartupMiddleware>();
        app.UseMiddleware<ReadinessMiddleware>();
        app.UseMiddleware<ActiveRequestsMiddleware>();
    };
}

