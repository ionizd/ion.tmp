using Ion;
using Ion.MicroServices.GraphQL;
using Ion.MicroServices.GraphQL.Demo.Graph;
using Ion.MicroServices.GraphQL.Demo.WeatherForecasting;
using Microsoft.Extensions.Logging.Abstractions;

var service = new MicroService("ion-microservices-graphql-demo", new NullLogger<IMicroService>())
    .ConfigureServices(services => 
    {
        services.AddSingleton<IWeatherForecastService, WeatherForecastService>();
    })
    .ConfigureGraphQLPipeline(schema =>
    {
        schema
              .AddQueryType<QueryType>();
    });

await service.RunAsync();