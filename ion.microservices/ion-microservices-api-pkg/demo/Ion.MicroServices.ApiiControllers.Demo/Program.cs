using Ion.Logging;
using Ion.MicroServices;
using Ion.MicroServices.Api;
using Ion.ServiceDiscovery.Consul;

var service = new MicroService("ion-microservices-apicontrollers-demo")
        .WithLogging(log =>
        {
            log
                .ToConsole();
        })
        .WithConsulServiceDiscovery()
        .ConfigureServices((services, configuration) => { })
        .ConfigureApiControllerPipeline()
    ;

await service.RunAsync();