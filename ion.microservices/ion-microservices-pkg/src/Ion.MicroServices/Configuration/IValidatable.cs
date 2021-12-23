namespace Ion.MicroServices.Configuration;

/// <summary>
/// ConfigurationSections implementing this interface use custom validation logic and are exempt from DataAnnotations-based validation
/// </summary>
public interface IValidatable
{
    IEnumerable<string> Validate(string key);
}
