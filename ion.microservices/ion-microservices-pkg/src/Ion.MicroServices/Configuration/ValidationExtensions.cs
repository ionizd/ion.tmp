using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Ion.MicroServices.Configuration;

public static class ValidationExtensions
{
    public static ICollection<string> ValidationErrors(this object @this, string key)
    {
        var results = new List<string>();

        results.AddRange(@this is IValidatable validatable
            ? ValidateIValidatable(validatable, key)
            : ValidateDataAnnotations(@this, key));

        return results;
    }

    private static IEnumerable<string> ValidateIValidatable(this IValidatable @this, string key)
    {
        return @this.Validate(key);
    }

    private static IEnumerable<string> ValidateDataAnnotations(this object @this, string key)
    {
        var context = new ValidationContext(@this, serviceProvider: null, items: null);
        var validationResults = new List<ValidationResult>();
        var results = new List<string>();

        Validator.TryValidateObject(@this, context, validationResults, true);

        results.AddRange(validationResults.Select(x => $"{key}:{x.MemberNames.First()} : {x.ErrorMessage}"));

        foreach (var property in @this.GetType().GetProperties().Where(p => p.GetIndexParameters().Length == 0))
        {
            var propertyKey = $"{key}:{property.Name}";
            var value = property.GetValue(@this);

            // Properties that are reference types but not collections
            if (property.PropertyType.IsClass && value != default && !property.PropertyType.GetInterfaces().Contains(typeof(IEnumerable)))
            {
                results.AddRange(property.GetValue(@this).ValidationErrors(propertyKey));
            }
            // Properties that are collections
            else if (property.PropertyType.IsClass && value != default && property.PropertyType.GetInterfaces().Contains(typeof(IEnumerable)))
            {
                if (value is IDictionary @dictionary)
                {
                    foreach (var k in @dictionary.Keys)
                    {
                        results.AddRange(@dictionary[k].ValidateDataAnnotations($"{propertyKey}:{k}"));
                    }
                }
                else if (value is IEnumerable @enumerable)
                {
                    var idx = 0;
                    foreach (var item in @enumerable)
                    {
                        results.AddRange(item.ValidateDataAnnotations($"{propertyKey}:{idx}"));
                        idx++;
                    }
                }
            }
        }

        return results;
    }
}
