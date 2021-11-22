using Ion.Lifecycle;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Ion.Extensions;

public static class IServiceCollectionExtensions
{    
    public static Action<IServiceCollection> ConfigureMicroServiceLifetime = (svc) => 
    {
        svc.AddSingleton<IActiveRequestsService, ActiveRequestsService>();
        svc.AddHostedService<StartupService>();
        svc.AddHostedService<ShutdownService>();
    };
}

