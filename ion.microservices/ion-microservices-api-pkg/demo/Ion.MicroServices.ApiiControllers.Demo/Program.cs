using Ion.MicroServices;
using Ion.MicroServices.Api;

var service = new MicroService("ion-microservices-apicontrollers-demo")
    .ConfigureServices((services, configuration) => { })
    .ConfigureApiControllerPipeline();

await service.RunAsync();