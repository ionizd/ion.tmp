using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Ion;

public class Startup
{
    public void Configure(IApplicationBuilder app)
    {
        var svc = (MicroService)app.ApplicationServices.GetService<IMicroService>();

        svc.ConfigurePipelineActions.ForEach(action => action(app));
    }
}

