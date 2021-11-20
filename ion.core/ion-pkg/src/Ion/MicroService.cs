using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ion;

public class MicroService : IMicroService
{
    public string Name { get; }
    public string Id { get; }
    public MicroServicePipelineMode PipelineMode { get; }
    public MicroServiceHostingMode HostingMode { get; }
    
    public Task RunAsync(IConfigurationRoot configuration = null, params string[] args)
    {
        throw new System.NotImplementedException();
    }
    
    internal List<Action<IServiceCollection>> ConfigureActions { get; } = new List<Action<IServiceCollection>>();
}