namespace Ion.MicroServices.Configuration;

/// <summary>
/// ConfigurationSections decorated by this attribute are not subject to automatic validation on startup
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class DisableValidationAttribute : Attribute
{
}