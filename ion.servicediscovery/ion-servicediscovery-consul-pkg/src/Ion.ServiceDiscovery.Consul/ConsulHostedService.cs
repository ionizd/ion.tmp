using Consul;
using Ion;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Ion.ServiceDiscovery.Consul
{
    public class ConsulHostedService : IHostedService
    {
        private readonly IMicroService microservice;
        private readonly IHostApplicationLifetime app;
        private readonly ConsulClient client;
        private readonly Options options;
        private readonly ILogger<ConsulHostedService> logger;

        public ConsulHostedService(IMicroService microservice, Options options, IHostApplicationLifetime app, ILogger<ConsulHostedService> logger)
        {
            this.microservice = microservice ?? throw new ArgumentNullException(nameof(microservice));
            this.app = app ?? throw new ArgumentNullException(nameof(app));

            client = new ConsulClient((cfg) =>
            {
                cfg.Address = new Uri(options.Address);
            });

            this.logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _ = app.ApplicationStarted.Register(async (state) =>
              {
                  var svc = (IMicroService)state;
                  var registration = new AgentServiceRegistration()
                  {
                      ID = svc.Id,
                      Name = svc.Name,
                      Address = svc.Address.ToString(),
                      Port = 80,
                      Checks = new[]
                      {
                            new AgentServiceCheck()
                            {
                                DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(1),
                                Interval = TimeSpan.FromSeconds(10),
                                TCP = $"{svc.Address.ToString()}:80"
                            },
                            new AgentServiceCheck()
                            {
                                DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(1),
                                Interval = TimeSpan.FromSeconds(10),
                                HTTP = $"http://{svc.Address.ToString()}:80/status/liveness"
                            }
                      }
                  };

                  logger.LogInformation("Registering service {@registration}", registration);

                  var result = await client.Agent.ServiceRegister(registration, default(CancellationToken));
              }, microservice);

            app.ApplicationStopping.Register(async (state) =>
            {
                var svc = (IMicroService)state;
                await client.Agent.ServiceDeregister(svc.Id, default(CancellationToken));
            }, microservice);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}