using System.ComponentModel.DataAnnotations;

namespace Ion.ServiceDiscovery.Consul;

public class Options
{
    public const string SectionKey = "Ion:ServiceDiscovery:Consul";

    public bool Enabled { get; set; } = false;

    public string Address { get; set; }
}
