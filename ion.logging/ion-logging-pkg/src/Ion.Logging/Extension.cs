﻿using Ion.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Serilog;
using Serilog.AspNetCore;
using Serilog.Extensions.Hosting;
using ILogger = Serilog.ILogger;

namespace Ion.Logging;

internal sealed class Extension : MicroServiceExtension
{
    private readonly Action<LoggingConfigurationBuilder> action;

    internal Extension(IMicroService service, Action<LoggingConfigurationBuilder> action)
        : base(service)
    {
        this.action = action ?? throw new ArgumentNullException(nameof(action));

        this.ConfigureActions.Add((svc, configuration) =>
        {
            var options = svc.ConfigureOptions<Options>(configuration, () => Options.SectionKey);

            var builder = new LoggingConfigurationBuilder(this);
            action(builder);

            ILogger logger = new LoggerConfiguration()
                .ConfigureSerilog(service, options, builder)
                .CreateLogger();

            ILoggerFactory loggerFactory = null;
            if (service.ExternalLogger)
            {
                loggerFactory = new NullLoggerFactory();
            }
            else
            {
                loggerFactory = new SerilogLoggerFactory((ILogger)logger, true);
            }

            svc.AddSingleton<ILoggerFactory>((Func<IServiceProvider, ILoggerFactory>)(provider => loggerFactory));
            svc.AddSingleton<ILogger>(logger);
            DiagnosticContext implementationInstance = new DiagnosticContext(logger);
            svc.AddSingleton<DiagnosticContext>(new DiagnosticContext(logger));
            svc.AddSingleton<IDiagnosticContext>((IDiagnosticContext)implementationInstance);
            svc.AddSingleton(new RequestLoggingOptions());
        });
    }

    public override IServiceCollection ConfigureServices(IServiceCollection services, IMicroService microservice)
    {
        


        
       
        //// Override the existing microservice.Logger with Serilog
        //((MicroService)microservice).Logger = microservice.ExternalLogger
        //    ? microservice.Logger
        //    : loggerFactory.CreateLogger<IMicroService>();

        return services;
    }

    public Action<IApplicationBuilder> ConfigureRequestLoggingMiddleware { get; set; }
}
