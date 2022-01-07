﻿using Ion.Extensions;
using Ion.MicroServices.Demo.WeatherForecasting;

namespace Ion.Microservices.Demo.Services;

public class WeatherForecastingService : BackgroundService
{
    private readonly ILogger<WeatherForecastingService> logger;
    private readonly IWeatherForecastService service;

    public WeatherForecastingService(ILogger<WeatherForecastingService> logger, IWeatherForecastService service)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.service = service ?? throw new ArgumentNullException(nameof(service));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var forecast = service.GetWeatherForecast();

            logger.LogInformation(forecast.ToString());
            await Task.Delay(10.Seconds(), stoppingToken);
        }
    }
}