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
                new Artifact() { Name = "ion-abstractions-pkg", Project = "Ion.Abstractions", Type = ArtifactType.Package, Stability = Stability.Stable },
                new Artifact() { Name = "ion-testing-pkg", Project = "Ion.Testing", Type = ArtifactType.Package,  Stability = Stability.Stable },
                new Artifact() { Name = "ion-analyzers-pkg", Project = "Ion.Analyzers", Type = ArtifactType.Package,  Stability = Stability.Stable }
            }
        },
        new Module()
        {
            Name = "ion.microservices",
            Artifacts = new[]
            {
                new Artifact() { Name = "ion-microservices-pkg", Project = "Ion.MicroServices", Type = ArtifactType.Package, Stability = Stability.Stable },
                new Artifact() { Name = "ion-microservices-api-pkg", Project = "Ion.MicroServices.Api", Type = ArtifactType.Package,  Stability = Stability.Stable },
                new Artifact() { Name = "ion-microservices-grpc-pkg", Project = "Ion.MicroServices.Grpc", Type = ArtifactType.Package,  Stability = Stability.Stable },
                new Artifact() { Name = "ion-microservices-graphql-pkg", Project = "Ion.MicroServices.GraphQL", Type = ArtifactType.Package,  Stability = Stability.Stable },
                new Artifact() { Name = "ion-microservices-job-pkg", Project = "Ion.MicroServices.Job", Type = ArtifactType.Package,  Stability = Stability.Stable }
            }
        },        
    };
}