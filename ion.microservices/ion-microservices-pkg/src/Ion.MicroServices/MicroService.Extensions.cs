using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ion.Extensions;

namespace Ion.MicroServices;

public partial class MicroService
{
    internal IList<MicroServiceExtension> Extensions { get; set; } = new List<MicroServiceExtension>();

    internal MicroService ConfigureExtensions()
    {
        this.Extensions.ForEach(extension =>
        {
            ConfigureActions.Add((services) => extension.ConfigureServices(services, this));
            ConfigurePipelineActions.Add((app) => extension.Configure(app, this));
        });


        return this;
    }
}
