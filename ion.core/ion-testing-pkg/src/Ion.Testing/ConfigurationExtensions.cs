using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Ion.Testing;

public static class ConfigurationExtensions
{
    public static IConfigurationBuilder UseDefaultLoggingConfiguration(this IConfigurationBuilder builder)
    {
        builder.AddInMemoryCollection(new Dictionary<string, string>()
        {
            { "Ion:Logging:Level", "Information" }
        });

        return builder;
    }
}
