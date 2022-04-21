namespace Ion.ServiceDiscovery.Consul;

public static class Startup
{
    public static IMicroService WithConsulServiceDiscovery(this IMicroService service)
    {
        service.Extensions.Add(new Extension(service));

        return service;   
    }
}