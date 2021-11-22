using Ion;
using Ion.MicroServices.ApiControllers;

var service = new MicroService("ion-microservices-apicontrollers-demo")
    .ConfigureServices(services => { })
    .ConfigureApiControllerPipeline();

await service.RunAsync();