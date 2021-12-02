using FluentAssertions;
using Ion.Extensions;
using Ion.MicroServices;
using Ion.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Ion.Tests;

public class MicroServiceTests
{
    public class Startup
    {
        private const string ServiceName = "microservice-tests-startup";

        [Fact]
        [UnitTest]
        public async void GivenConfigureDefaultServicePipelineIsUsed_WhenRunAsyncIsInvoked_ThenServiceStartsInNoneMode()
        {
            // Arrange
            var config = new ConfigurationBuilder().Build();            

            var service = new MicroService(ServiceName, new NullLogger<IMicroService>())
                .InTestClass<MicroServiceTests>()
                .ConfigureDefaultServicePipeline();

            service.CancellationTokenSource.CancelAfter(1000);

            // Act
            await service.RunAsync(config);

            // Assert
            service.PipelineMode.Should().Be(MicroServicePipelineMode.None);
        }

        [Fact]
        [UnitTest]
        public async void GiveRunAsyncIsInvoked_WhenNoIHostedStartupServicesAreUsed_ThenServiceShouldStartImmediately()
        {
            // Arrange
            var config = new ConfigurationBuilder().Build();

            var service = new MicroService(ServiceName, new NullLogger<IMicroService>())
                .InTestClass<MicroServiceTests>()
                .ConfigureDefaultServicePipeline();

            service.CancellationTokenSource.CancelAfter(1000);

            

            // Act & Assert
            var task = service.RunAsync(config);
            
            service.ShouldStart(500.Milliseconds());

            await task;
        }
    }
}

