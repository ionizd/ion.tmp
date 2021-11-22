using Ion.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Ion.MicroServices.Api
{
    public static class IMicroServiceExtensions
    {
        public static IMicroService ConfigureApiPipeline(this IMicroService microservice, Action<Microsoft.AspNetCore.Routing.IEndpointRouteBuilder> action)
        {
            var service = (MicroService)microservice;

            service.ValidatePipelineModeNotSet();

            service.ConfigureActions.Add(IServiceCollectionExtensions.ConfigureMicroServiceLifetime);
            service.ConfigureActions.Add(svc =>
            {
                svc.AddEndpointsApiExplorer();
                svc.AddSwaggerGen();
            });

            service.UseDefaultMicroServicePipeline(developmentOnlyPipeline: app =>
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            });

            service.ConfigurePipelineActions.Add(app =>
            {               
                app.UseRouting();
                app.UseEndpoints(endpoints =>
                {
                    action(endpoints);
                });
            });

            service.PipelineMode = MicroServicePipelineMode.Api;

            return microservice;
        }
    }
}