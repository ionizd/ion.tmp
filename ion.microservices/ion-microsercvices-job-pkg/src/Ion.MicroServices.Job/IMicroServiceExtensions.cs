﻿using Ion.MicroServices.Job.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;


namespace Ion.MicroServices.Job
{
    public static class IMicroServiceExtensions
    {
        
        public static IMicroService ConfigureJob(this IMicroService microservice)
        {
            if (microservice == null) throw new ArgumentNullException(nameof(microservice));
           
            var service = (MicroService)microservice;


            service.ValidatePipelineModeNotSet();

            service.ConfigureActions.Add(MicroService.ServiceCollection.LifecycleServices);
            service.ConfigureActions.Add(svc =>
            {
                svc.AddHostedService<JobHostedService>();
            });

            service
                .ConfigureExtensions()
                .ConfigurePipelineActions.Add(app =>
                {
                    app.UseRouting();
                    app.UseAuthorization();
                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapGet("/", () => "Communication with a Ion.MicroServices.Job is not possible. The service will execute all IHostedService(s) and shut down");
                    });
                });

            service.PipelineMode = MicroServicePipelineMode.None;

            return microservice;
        }
    }
}