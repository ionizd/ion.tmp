using Ion.MicroServices.Configuration.Validation;
using Ion.Testing;
using Microsoft.Extensions.Options;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using FluentAssertions;
using Ion.MicroServices.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Ion.MicroServices.Tests.Configuration;

public partial class ConfigurationValidationTests
{
    public class Validator
    {
        public class Options
        {
            public const string SectionKey = "TestOptions";

            [Required]
            public string? Name { get; set; }

            [ValidateCollection, Required, MinLength(1)]
            public ChildOptions[] Children { get; set; } = Array.Empty<ChildOptions>();
        }

        public class ChildOptions
        {
            [Required]
            private string? Name { get; set; }
        }

        public class OptionsValidator : IValidateOptions<Options>
        {
            public ValidateOptionsResult Validate(string name, Options options)
            {
                var validationResults = global::Ion.MicroServices.Configuration.Validation.Validator.ValidateReturnValue(options);
                if (validationResults.Any())
                {
                    var builder = new StringBuilder();
                    foreach (var result in validationResults)
                    {
                        var pretty = PrettyPrint(result, string.Empty, true);
                        builder.Append(pretty);
                    }
                    return ValidateOptionsResult.Fail(builder.ToString());
                }

                return ValidateOptionsResult.Success;
            }

            private string PrettyPrint(MicroServices.Configuration.Validation.ValidationResult root, string indent, bool last)
            {
                // Based on https://stackoverflow.com/a/1649223
                var sb = new StringBuilder();
                sb.Append(indent);
                if (last)
                {
                    sb.Append("|-");
                    indent += "  ";
                }
                else
                {
                    sb.Append("|-");
                    indent += "| ";
                }

                sb.AppendLine(root.ToString());

                if (root.ValidationResults != null)
                {
                    for (var i = 0; i < root.ValidationResults.Length; i++)
                    {
                        var child = root.ValidationResults[i];
                        var pretty = PrettyPrint(child, indent, i == root.ValidationResults.Length - 1);
                        sb.Append(pretty);
                    }
                }

                return sb.ToString();
            }
        }

        [SmartTheory(Execute.Always, On.All)]
        [InlineData("test-validator-options01.json", true)]
        [InlineData("test-validator-options02.json", false)]
        [InlineData("test-validator-options03.json", false)]
        [InlineData("test-validator-options04.json", false)]
        [UnitTest]
        public void X(string config, bool shouldBeValid)
        {
            // Arrange
            var cfg = GetConfigurationRoot(config);

            var provider = new ServiceCollection()
                .ConfigureAndValidate<Options>(cfg, () => Options.SectionKey)
                .BuildServiceProvider();

            // Act
            var action = () =>
            {
                var options = provider.GetRequiredService<IOptions<Options>>().Value;
                options.GetType();
            };

            if (shouldBeValid)
            {
                action.Should().NotThrow();
            }
            else
            {
                action.Should().Throw<OptionsValidationException>();
            }
        }
    }
}
