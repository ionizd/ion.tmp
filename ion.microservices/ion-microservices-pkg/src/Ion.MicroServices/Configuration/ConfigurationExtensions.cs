using Microsoft.Extensions.Configuration;

namespace Ion.MicroServices.Configuration;

public static class ConfigurationExtensions
{
    public static IConfigurationSection GetExistingSection(this IConfiguration configuration, string key)
    {
        var configurationSection = configuration.GetSection(key);

        if (!configurationSection.Exists())
        {
            throw configuration switch
            {
                IConfigurationRoot configurationIsRoot => new ArgumentException($"Section with key '{key}' does not exist. Existing values are: {configurationIsRoot.GetDebugView()}", nameof(key)),
                IConfigurationSection configurationIsSection => new ArgumentException($"Section with key '{key}' does not exist at '{configurationIsSection.Path}'. Expected configuration path is '{configurationSection.Path}'", nameof(key)),
                _ => new ArgumentException($"Failed to find configuration at '{configurationSection.Path}'", nameof(key))
            };
        }

        return configurationSection;
    }
}
