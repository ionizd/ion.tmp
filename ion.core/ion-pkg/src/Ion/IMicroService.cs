using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
namespace Ion;

public interface IMicroService
{
    string Name { get; }
    string Id { get; }
    
    MicroServicePipelineMode PipelineMode { get; }
    MicroServiceHostingMode HostingMode { get;  }

    Task RunAsync(IConfigurationRoot configuration = null, params string[] args);
}