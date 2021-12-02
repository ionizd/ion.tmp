using Ion.Lifecycle;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Ion;

public partial class MicroService
{
    public static class ServiceCollection
    {
        public static Action<IServiceCollection> LifecycleServices = (svc) =>
        {
            svc.AddSingleton<IActiveRequestsService, ActiveRequestsService>();
            svc.AddHostedService<StartupService>();
            svc.AddHostedService<ShutdownService>();
        };
    }
}

