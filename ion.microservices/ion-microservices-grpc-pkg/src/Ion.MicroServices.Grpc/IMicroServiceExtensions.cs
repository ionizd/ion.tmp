﻿using Ion.MicroServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using ProtoBuf.Grpc.Server;

namespace Ion.MicroServices.Grpc
{
    public static class IMicroServiceExtensions
    {
        public static IMicroService ConfigureGrpcCodeFirstPipeline(this IMicroService microservice, Action<IEndpointRouteBuilder> endpointsBuilder)
        {
            return microservice.ConfigureGrpcPipelineInternal(endpointsBuilder, configureGrpc: (services) => services.AddGrpc());
        }

        public static IMicroService ConfigureCodeFirstGrpcPipeline(this IMicroService microservice, Action<IEndpointRouteBuilder> endpointsBuilder)
        {
            return microservice.ConfigureGrpcPipelineInternal(endpointsBuilder, configureGrpc: (services) => services.AddCodeFirstGrpc());
        }

        private static IMicroService ConfigureGrpcPipelineInternal(this IMicroService microservice, Action<IEndpointRouteBuilder> endpointsBuilder, Action<IServiceCollection> configureGrpc)
        {
            if (microservice == null) throw new ArgumentNullException(nameof(microservice));
            if (endpointsBuilder == null) throw new ArgumentException(nameof(endpointsBuilder));
            if (configureGrpc == null) throw new ArgumentException(nameof(configureGrpc));

            var service = (MicroService)microservice;


            service.ValidatePipelineModeNotSet();

            service.ConfigureActions.Add(MicroService.ServiceCollection.LifecycleServices);

            service.ConfigureActions.Add(services =>
            {
                configureGrpc(services);
            });

            service.UseCoreMicroServicePipeline();

            service.ConfigurePipelineActions.Add(app =>
            {
                app.UseRouting();
                app.UseAuthorization();
                app.UseEndpoints(endpoints =>
                {
                    endpointsBuilder(endpoints);
                    endpoints.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                });
            });

            service.PipelineMode = MicroServicePipelineMode.Grpc;

            return microservice;
        }
    }
}