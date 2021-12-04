using Ion;
using Ion.MicroServices;
using Ion.MicroServices.Demo.GrpcCodeFirst.Services;
using Ion.MicroServices.Demo.WeatherForecasting;
using Ion.MicroServices.Grpc;
using Microsoft.Extensions.Logging.Abstractions;

var service = new MicroService("ion-microservices-grpc-code1st-demo", new NullLogger<IMicroService>())
    .ConfigureServices(services =>
    {
        services.AddSingleton<IWeatherForecastService, WeatherForecastService>();
    })
    .ConfigureGrpcCodeFirstPipeline(endpoints =>
    {
        endpoints.MapGrpcService<WeatherForecastingService>();
    });

await service.RunAsync();