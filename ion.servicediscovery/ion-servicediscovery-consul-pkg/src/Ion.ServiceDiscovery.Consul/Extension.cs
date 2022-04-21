using Ion.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ion.ServiceDiscovery.Consul;

internal sealed class Extension : MicroServiceExtension
{
    internal Extension(IMicroService service)
        : base(service)
    {
        this.ConfigureActions.Add((svc, configuration) =>
        {
            var options = svc.ConfigureOptions<Options>(configuration, () => Options.SectionKey);

            svc.AddHostedService<ConsulHostedService>();
        });
    }    
}
