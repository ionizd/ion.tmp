using Ion.Middleware;
using Microsoft.AspNetCore.Builder;
using System;

namespace Ion;

public partial class MicroService
{
    public static class Middleware
    {
        public static Action<IApplicationBuilder> MicroServiceLifecycleMiddlewares = (app) =>
        {
            app.UseMiddleware<StartupMiddleware>();
            app.UseMiddleware<ReadinessMiddleware>();
            app.UseMiddleware<ActiveRequestsMiddleware>();
        };
    }
}

