﻿using System;
using Xunit.Sdk;

namespace Ion.Testing
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    [TraitDiscoverer(SmokeTestDiscoverer.TypeName, AssemblyInfo.Name)]
    public class SmokeTestAttribute : Attribute, ITraitAttribute
    {
        public const string Category = "SmokeTests";
    }
}
