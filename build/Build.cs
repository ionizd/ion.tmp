using CloudTek.BuildSystem;
using Nuke.Common.CI;
using Nuke.Common.Execution;

[CheckBuildProjectConfigurations]
[ShutdownDotNetAfterServerBuild]
internal class Build : SmartBuild
{
    public Build() : base(Modules)
    {
    }

    public static int Main() => Execute<Build>(x => x.Compile);

    public static Module[] Modules { get; } = new[]
    {
        new Module()
        {
            Name = "ion.core",
            Artifacts = new[]
            {
                new Artifact() { Name = "ion-pkg", Project = "Ion", Type = ArtifactType.Package,  Stability = Stability.Stable },
                new Artifact() { Name = "ion-testing-pkg", Project = "Ion.Testing", Type = ArtifactType.Package,  Stability = Stability.Stable },
                new Artifact() { Name = "ion-analyzers-pkg", Project = "Ion.Analyzers", Type = ArtifactType.Package,  Stability = Stability.Stable }
            }
        },
        //new Module()
        //{
        //    Name = "ion.logging",
        //    Artifacts = new[]
        //    {
        //        new Artifact() { Name = "ion-logging-pkg", Project = "Ion.Logging", Type = ArtifactType.Package,  Stability = Stability.Stable },
        //        new Artifact() { Name = "ion-logging-elasticsearch-pkg", Project = "Ion.Logging.Elasticsearch", Type = ArtifactType.Package,  Stability = Stability.Stable },
        //        new Artifact() { Name = "ion-logging-logzio-pkg", Project = "Ion.Logging.LogzIo", Type = ArtifactType.Package,  Stability = Stability.Stable }
        //    }
        //},
        //new Module()
        //{
        //    Name = "ion.microservices",
        //    Artifacts = new[]
        //    {
        //        new Artifact() { Name = "ion-microservices-pkg", Project = "Ion.MicroServices", Type = ArtifactType.Package,  Stability = Stability.Stable },
        //        new Artifact() { Name = "ion-microservices-api-pkg", Project = "Ion.MicroServices.Api", Type = ArtifactType.Package,  Stability = Stability.Stable },
        //        new Artifact() { Name = "ion-microservices-mvc-pkg", Project = "Ion.MicroServices.Mvc", Type = ArtifactType.Package,  Stability = Stability.Stable },
        //        new Artifact() { Name = "ion-microservices-testing-pkg", Project = "Ion.MicroServices.Testing", Type = ArtifactType.Package,  Stability = Stability.Stable }
        //    }
        //},
        //new Module()
        //{
        //    Name = "ion.extensions",
        //    Artifacts = new[]
        //    {
        //        new Artifact() { Name = "ion-dapr-pkg", Project = "Ion.Dapr", Type = ArtifactType.Package,  Stability = Stability.Stable },
        //        new Artifact() { Name = "ion-metrics-pkg", Project = "Ion.Metrics", Type = ArtifactType.Package,  Stability = Stability.Stable },
        //        new Artifact() { Name = "ion-telemetry-pkg", Project = "Ion.Telemetry", Type = ArtifactType.Package,  Stability = Stability.Stable },
        //    }
        //},
    };
}