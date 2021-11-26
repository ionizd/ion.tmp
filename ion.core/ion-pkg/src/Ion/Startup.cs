using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace Ion;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        var microservice = (MicroService)services.BuildServiceProvider().GetService<IMicroService>();

        microservice.ConfigureActions.ForEach(action => action(services));
    }

    public void Configure(IApplicationBuilder app, DiagnosticListener diagnosticListener)
    {
        var svc = (MicroService)app.ApplicationServices.GetService<IMicroService>();
        
        var listener = new TestDiagnosticListener();
        diagnosticListener.SubscribeWithAdapter(listener);

        svc.ConfigurePipelineActions.ForEach(action => action(app));
    }


}

