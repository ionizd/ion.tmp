using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;
using FluentAssertions;
using Ion.MicroServices.ApiControllers;
using Ion.Testing;

namespace Ion.Tests;

public class MicroServiceTests
{
    public class Startup
    {
        private const string ServiceName = "microservice-tests-startup";

        [Fact]
        [UnitTest]
        public async void GivenConfigureWebApiControllersPipelineIsUsed_WhenRunAsyncIsInvoked_ThenServiceStartsInWebApiControllersMode()
        {
            var config = new ConfigurationBuilder().Build();            

            var service = new MicroService(ServiceName, new NullLogger<IMicroService>())
                .ConfigureApiControllerPipeline();

            service.CancellationTokenSource.CancelAfter(2000);
            await service.RunAsync(config);

            service.PipelineMode.Should().Be(MicroServicePipelineMode.ApiControllers);
        }
    }
}

