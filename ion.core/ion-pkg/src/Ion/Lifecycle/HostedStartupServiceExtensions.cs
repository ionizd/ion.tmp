using Microsoft.Extensions.DependencyInjection;

namespace Ion.Lifecycle;

public static class HostedStartupServiceExtensions
    {
        public static IServiceCollection AddHostedStartupService<T>(this IServiceCollection services)
            where T : class, IHostedStartupService
        {
            return services.AddSingleton<IHostedStartupService, T>();
        }
    }

