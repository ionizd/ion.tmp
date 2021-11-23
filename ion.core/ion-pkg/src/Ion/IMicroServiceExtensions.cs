using Ion.Extensions;
using Ion.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Ion;

public static class IMicroServiceExtensions
{    
    public static IMicroService ConfigureServices(this IMicroService microservice, Action<IServiceCollection> action)
    {
        if(microservice == null) throw new ArgumentNullException(nameof(microservice));
        if(action == null) throw new ArgumentNullException(nameof(action));

        var service = (MicroService)microservice;

        service.ConfigureActions.Add(action);

        if(service.Environment.IsDevelopment())
        {
            service.ConfigureActions.Add(svc =>
            {
                svc.AddMiddlewareAnalysis();
            });
        }

        return microservice;
    }

    internal static IMicroService UseDefaultMicroServicePipeline(this IMicroService microservice, Action<IApplicationBuilder> developmentOnlyPipeline = null)
    {
        var service = (MicroService)microservice;

        service.ConfigurePipelineActions.Add(app =>
        {
            app.UseMiddleware<LivenessMiddleware>();
        });

        if(developmentOnlyPipeline != null && service.Environment.IsDevelopment())
        {
            service.ConfigurePipelineActions.Add(developmentOnlyPipeline);
        }

        service.ConfigurePipelineActions.Add(IApplicationBuilderExtensions.UseMicroServiceLifetimeMiddleware);

        service.ConfigurePipelineActions.Add(app =>
        {
            app.UseRouting();
            app.UseAuthorization();
        });

        return microservice;
    }

    internal static IMicroService ValidatePipelineModeNotSet(this IMicroService microservice)
    {
        var service = (MicroService)microservice;

        if (service.PipelineMode != MicroServicePipelineMode.NotSet)
        {
            throw new InvalidOperationException($"MicroService {nameof(service.PipelineMode)} is already set");
        }

        return microservice;
    }
}

