using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Ion;

public interface IMicroService
{
    CancellationTokenSource CancellationTokenSource { get; }
    string Environment { get; }
    IReadOnlyDictionary<string, string> EnvironmentVariables { get; }
    MicroServiceHostingMode HostingMode { get; }
    string Id { get; }
    bool IsReady { get; }
    bool IsStarted { get; }
    IMicroServiceLifetime Lifetime { get; }
    string Name { get; }
    MicroServicePipelineMode PipelineMode { get; }
    Task RunAsync(IConfigurationRoot configuration = null, params string[] args);
}