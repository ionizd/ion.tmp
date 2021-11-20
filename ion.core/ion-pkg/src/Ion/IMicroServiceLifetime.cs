using System.Threading;

namespace Ion;

public interface IMicroServiceLifetime
{
    CancellationToken ServiceStarted { get; }
}