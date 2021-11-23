namespace Ion.MicroServices.GraphQL.Demo.WeatherForecasting;

public interface IWeatherForecastService
{
    IEnumerable<WeatherForecast> GetWeatherForecast();
}

