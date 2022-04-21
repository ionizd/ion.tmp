using Microsoft.Extensions.Configuration;
using System.Net;

namespace Ion;

public interface IMicroService
{
    CancellationTokenSource CancellationTokenSource { get; }
    IConfigurationRoot ConfigurationRoot { get; }
    string Environment { get; }
    IReadOnlyDictionary<string, string> EnvironmentVariables { get; }
    IList<MicroServiceExtension> Extensions { get; }
    bool ExternalLogger { get; }

    /// <summary>
    /// Enum indicating how the service is hosted (autodected)
    /// </summary>
    MicroServiceHostingMode HostingMode { get; }
    
    /// <summary>
    /// Globally unique ID of the service instance
    /// </summary>
    string Id { get; }

    /// <summary>
    /// Flag indicating that the service's readiness probe is returning a positive result, and the service is ready to accept traffic
    /// </summary>
    bool IsReady { get; }

    /// <summary>
    /// Flag indicating that the service's startup probe is returning a positive result, and the service has completed all of its startup cycles.
    /// </summary>
    bool IsStarted { get; }

    IMicroServiceLifetime Lifetime { get; }
    
    /// <summary>
    /// Name of the service
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Kubernetes namespace of the service (defaults to 'default', provided by K8S_POD_NAMESPACE)
    /// </summary>
    string Namespace { get; }

    /// <summary>
    /// IP Address of the service (defaults to '127.0.0.1', provided by K8S_POD_IPADDRESS)
    /// </summary>
    IPAddress Address { get; }
    
    /// <summary>
    /// Enum describing the type of pipeline used by the service
    /// </summary>
    MicroServicePipelineMode PipelineMode { get; }

    IMicroService RegisterExtension<TExtension>() where TExtension : MicroServiceExtension, new();
    Task RunAsync(IConfigurationRoot configuration = null, params string[] args);
}