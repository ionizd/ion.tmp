using System.Threading;
using System.Threading.Tasks;

namespace Ion.Lifecycle;

public interface IHostedStartupService
{
    bool Completed { get; }

    Task StartAsync(CancellationToken cancellationToken);
}
