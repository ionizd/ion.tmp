using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Ion;

public abstract class MicroServiceExtension
{
    public virtual IServiceCollection ConfigureServices(IServiceCollection services, IMicroService microservice)
    {
        return services;
    }

    public virtual IApplicationBuilder Configure(IApplicationBuilder app, IMicroService microservice)
    {
        return app;
    }

    public virtual IApplicationBuilder ConfigureBeforeReadinessProbe(IApplicationBuilder app, IWebHostEnvironment env)
    {
        return app;
    }

    public virtual IHealthChecksBuilder ConfigureHealthChecks(IHealthChecksBuilder builder)
    {
        return builder;
    }

    public virtual IEndpointRouteBuilder ConfigureEndpoints(IEndpointRouteBuilder builder)
    {
        return builder;
    }
}
