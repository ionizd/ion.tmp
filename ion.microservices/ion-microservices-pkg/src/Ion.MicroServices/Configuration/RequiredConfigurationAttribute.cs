namespace Ion.MicroServices.Configuration;

/// <summary>
/// ConfigurationSections decorated by this attribute are required to be present and bound correctly
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class RequiredConfigurationAttribute : Attribute
{
}
