using Ion.Microservices.Demo.Services;
using Ion.MicroServices;
using Ion.MicroServices.Demo.WeatherForecasting;

var service = new MicroService("ion-microservices-demo")
    .ConfigureServices(services => 
    {
        services.AddSingleton<IWeatherForecastService, WeatherForecastService>();
        services.AddHostedService<WeatherForecastingService>();
    })
    .ConfigureDefaultServicePipeline();

await service.RunAsync();