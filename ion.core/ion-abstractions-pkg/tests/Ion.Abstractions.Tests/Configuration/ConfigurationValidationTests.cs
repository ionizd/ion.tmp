﻿using FluentAssertions;
using Ion.Configuration;
using Ion.Exceptions;
using Ion.Testing;
using Microsoft.Extensions.Configuration;
using System;
using Xunit;

namespace Ion.Tests.Configuration;

public partial class ConfigurationValidationTests
{
    [SmartTheory(Execute.Always, On.All)]
    [InlineData("test-dataannotations-options00.json")]
    [UnitTest]
    public void GivenSectionDoesNotExist_WhenGettingExistingSection_ThenConfigurationException(string config)
    {
        // Arrange
        var cfg = GetConfigurationRoot(config);

        // Act & Assert
        var action = new Action(() => cfg.GetExistingSection(DataAnnotated.Options.SectionKey));

        action.Should()
            .Throw<ConfigurationException>()
            .And.Key.Should().Be(DataAnnotated.Options.SectionKey);
    }

    private static IConfigurationRoot GetConfigurationRoot(string config)
    {
        var stream =
            typeof(ConfigurationValidationTests).Assembly.GetManifestResourceStream(
                $"Ion.Tests.Configuration.{config}");
        return new ConfigurationBuilder().AddJsonStream(stream).Build();
    }
}