using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Ion.MicroServices.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureAndValidate<TOptions>(this IServiceCollection services, IConfigurationRoot configRoot, Func<string> sectionKeyProvider)
        where TOptions : class
    {
        var section = configRoot.GetExistingSection(sectionKeyProvider());

        return services.ConfigureAndValidate<TOptions>(section);
    }

    public static IServiceCollection ConfigureAndValidate<TOptions>(this IServiceCollection services, IConfigurationRoot configRoot, string name, Func<string> sectionKeyProvider)
        where TOptions : class
    {
        var section = configRoot.GetExistingSection(sectionKeyProvider());

        return services.ConfigureAndValidate<TOptions>(name, section);
    }
    public static IServiceCollection ConfigureAndValidate<TOptions>(this IServiceCollection services, IConfiguration config) where TOptions : class
        => services.ConfigureAndValidate<TOptions>(global::Microsoft.Extensions.Options.Options.DefaultName, config);

    public static IServiceCollection ConfigureAndValidate<TOptions>(this IServiceCollection services, string name, IConfiguration config) where TOptions : class
    {
        _ = config ?? throw new ArgumentNullException(nameof(config));
        services.Configure<TOptions>(name, config);
        services.AddDataAnnotationValidatedOptions<TOptions>(name);
        return services;
    }

    public static IServiceCollection ConfigureAndValidate<TOptions>(this IServiceCollection services, Action<TOptions> configureOptions) where TOptions : class
        => services.ConfigureAndValidate<TOptions>(global::Microsoft.Extensions.Options.Options.DefaultName, configureOptions);

    public static IServiceCollection ConfigureAndValidate<TOptions>(this IServiceCollection services, string name, Action<TOptions> configureOptions) where TOptions : class
    {
        services.Configure(name, configureOptions);
        services.AddDataAnnotationValidatedOptions<TOptions>(name);
        return services;
    }

    private static IServiceCollection AddDataAnnotationValidatedOptions<TOptions>(this IServiceCollection services, string name) where TOptions : class
    {
        services.TryAddEnumerable(ServiceDescriptor.Singleton<IValidateOptions<TOptions>>(new DataAnnotationValidateOptions<TOptions>(name)));
        return services;
    }
}
