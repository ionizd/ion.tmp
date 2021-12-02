using Ion;
using Ion.MicroServices;

var service = new MicroService("ion-microservices-demo")
    .ConfigureServices(services => { })
    .ConfigureDefaultServicePipeline();

await service.RunAsync();