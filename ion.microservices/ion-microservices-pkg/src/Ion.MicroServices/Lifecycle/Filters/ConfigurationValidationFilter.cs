using Ion.Extensions;
using Ion.MicroServices.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace Ion.MicroServices.Lifecycle.Filters;

public class ConfigurationValidationFilter : IStartupFilter
{
    readonly IEnumerable<IValidatable> _validatableObjects;
    public ConfigurationValidationFilter(IEnumerable<IValidatable> validatableObjects)
    {
        _validatableObjects = validatableObjects;
    }

    public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
    {
        _validatableObjects.ForEach(obj => obj.Validate());
        

        //don't alter the configuration
        return next;
    }
}
