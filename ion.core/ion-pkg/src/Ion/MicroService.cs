using Ion.Exceptions;
using Ion.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Ion;

public class MicroService : MicroServiceBase, IMicroService
{
    public bool ExternalLogger = true;

    public MicroService(string name) : this(name, null) { }

    public MicroService(string name, ILogger<IMicroService> logger) : base()
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));

        if (logger != null)
        {
            ExternalLogger = true;
            Logger = logger;
        }

        ConfigureActions.Add((svc) => { 
            svc.AddSingleton<IMicroService>(this);
            svc.AddAuthorization();
            svc.AddLogging(logger => logger.AddConsole());
            svc.Configure<HostOptions>(options => options.ShutdownTimeout = 60.Seconds());
        });
    }

    public CancellationTokenSource CancellationTokenSource { get; } = new CancellationTokenSource();
    public IHost Host { get; private set; }
    public IMicroServiceLifetime Lifetime { get; } = new MicroServiceLifetime();
    public string Name { get; }
    public MicroServicePipelineMode PipelineMode { get; set; } = MicroServicePipelineMode.NotSet;
    internal List<Action<IServiceCollection>> ConfigureActions { get; } = new List<Action<IServiceCollection>>();
    internal List<Action<IApplicationBuilder>> ConfigurePipelineActions { get; } = new List<Action<IApplicationBuilder>>();
    private ILogger<IMicroService> Logger { get; set; }
    public Task InitializeAsync(IConfigurationRoot configuration = null, params string[] args)
    {
        Host = CreateHostBuilder(configuration, args);

        return Task.CompletedTask;
    }
    public async Task RunAsync(IConfigurationRoot configuration = null, params string[] args)
    {
        await InitializeAsync(configuration, args).ConfigureAwait(false);

        try
        {
            if(PipelineMode == MicroServicePipelineMode.NotSet) 
            {
                throw new ConfigurationException(Constants.Errors.PipelineNotSet);
            }

            await Host.RunAsync(CancellationTokenSource.Token);
        }
        catch (Exception ex)
        {
            Logger.LogCritical("Unhandled exception in {Service}: {@Exception}", Name, ex);
            throw;
        }
    }
    private IHost CreateHostBuilder(IConfigurationRoot configuration = null, params string[] args)
    {
        var host = global::Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseConsoleLifetime()                        
            .ConfigureAppConfiguration((cfg) =>
            {
                if (configuration != null)
                {
                    cfg.AddConfiguration(configuration);
                }
                else
                {
                    cfg
                        .AddJsonFile("appsettings.json", optional: false)
                        .AddJsonFile($"appsettings.{Environment}.json", optional: true)
                        .AddJsonFile("shared.json", optional: true)
                        .AddJsonFile($"shared.{Environment}.json", optional: true)
                        .AddEnvironmentVariables()
                        .AddCommandLine(args);
                }
            })
            .ConfigureServices(svc =>
            {
                ConfigureActions.ForEach(action => action(svc));
            })
            .ConfigureWebHostDefaults(app =>
            {
                app.UseStartup<Startup>();
            })
            .Build();        

        return host;
    }    
}