using Ion.MicroServices.Lifecycle;
using Microsoft.Extensions.DependencyInjection;

namespace Ion.MicroServices;

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