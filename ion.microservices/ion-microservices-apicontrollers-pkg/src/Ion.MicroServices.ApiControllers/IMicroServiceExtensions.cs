using Ion.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Ion.MicroServices.ApiControllers;

public static class IMicroServiceExtensions
{
    public static IMicroService ConfigureApiControllerPipeline(this IMicroService microservice)
    {
        var service = (MicroService)microservice;

        service.ValidatePipelineModeNotSet();

        service.ConfigureActions.Add(IServiceCollectionExtensions.ConfigureMicroServiceLifetime);
        service.ConfigureActions.Add(svc =>
        {
            svc.AddControllers();
            svc.AddEndpointsApiExplorer();
            svc.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = microservice.Name, Version = "v1" });
            });
        });

        service.UseDefaultMicroServicePipeline(developmentOnlyPipeline: app => 
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        });
       
        service.ConfigurePipelineActions.Add(app =>
        {
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        });

        service.PipelineMode = MicroServicePipelineMode.ApiControllers;

        return microservice;
    }
}