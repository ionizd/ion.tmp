using FluentAssertions;
using Ion.MicroServices.Configuration;
using Ion.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Ion.MicroServices.Tests.Configuration;

public partial class ConfigurationValidationTests
{
    public class DataAnnotated
    {
        public class Options
        {
            public const string SectionKey = "TestOptions";

            [Required] public string? Setting1 { get; set; }
            [Required] public string? Setting2 { get; set; }
            [Required, Url] public string? Setting3 { get; set; }
            [Required, EmailAddress] public string? Setting4 { get; set; }
        }

        [SmartTheory(Execute.Always, On.All)]
        [InlineData("test-options01.json", true, null, null)]
        [InlineData("test-options02.json", false, "Setting2", "required")]
        [InlineData("test-options03.json", false, "Setting3", "URL")]
        [InlineData("test-options04.json", false, "Setting4", "e-mail")]
        [UnitTest]
        public void
            GivenSectionExists_WhenConfigureAndValidateIsInvoked_ThenOptionsAreValidatedWhenResolvingFromContainer(
                string config, bool shouldBeValid, string? key, string? error)
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
            };

            // Assert
            if (shouldBeValid)
            {
                action.Should().NotThrow();
            }
            else
            {
                action.Should().Throw<OptionsValidationException>().And.Message.Should().ContainAll(new [] { key, error });
            }
        }
    }
}