using FluentAssertions.Extensions;
using Ion.Lifecycle;
using System.Threading;
using System.Threading.Tasks;

namespace Ion.Tests;

internal static class TestData
{
    internal class Sec2DelayStartupService : IHostedStartupService
    {
        public bool Completed { get; set; } = false;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await Task.Delay(2.Seconds());

            Completed = true;
        }
    }

    internal class FailingSec2DelayStartupService : IHostedStartupService
    {
        public bool Completed { get; set; } = false;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await Task.Delay(2.Seconds());

            throw new Ion.Exceptions.ConfigurationException("test configuration exception");
        }
    }
}

