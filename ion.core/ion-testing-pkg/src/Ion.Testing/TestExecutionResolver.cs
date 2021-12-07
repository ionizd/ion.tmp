using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Ion.Testing
{
    internal static class TestExecutionResolver
    {
        internal static IDictionary<Execute, string> SkipReasonForExecute = new Dictionary<Execute, string>()
        {
            { Execute.InGithubActions, "Test to be executed only in Github Actions." },
            { Execute.InAzureDevOps, "Test to be executed only in Azure DevOps." },
            { Execute.InContainer, "Test to be executed only in a .NET Core Container." },
            { Execute.InDebug, "Test to be executed only in DEBUG configuration." }
        };

        internal static IDictionary<On, string> SkipReasonForOn = new Dictionary<On, string>()
        {
            { On.Windows, "Test to be executed on Windows." },
            { On.Linux, "Test to be executed on Linux." },
            { On.MacOS, "Test to be executed on MacOS." },
        };

        public static string Resolve(Execute execute, On on)
        {
            var exError = Resolve(execute);
            var onError = Resolve(on);

            if (exError != null && onError != null)
            {
                return new StringBuilder(exError).Append(" & ").AppendLine(onError).ToString();
            }
            else if (exError != null)
            {
                return exError;
            }
            else if (onError != null)
            {
                return onError;
            }

            return null;
        }

        internal static string Resolve(Execute execute)
        {
            if ((Execute.InGithubActions & execute) != 0)
            {
                var err = ValidateEnvVariablesExists(new[] { "GITHUB_ACTIONS" }, () => SkipReasonForExecute[Execute.InGithubActions]);
                if (err != null) return err;
            }

            if ((Execute.InAzureDevOps & execute) != 0)
            {
                var err = ValidateEnvVariablesExists(new[] { "AGENT_ID", "BUILD_BUILDID" }, () => SkipReasonForExecute[Execute.InAzureDevOps]);
                if (err != null) return err;
            }

            if ((Execute.InContainer & execute) != 0)
            {
                var err = ValidateEnvVariableValue("DOTNET_RUNNING_IN_CONTAINER", "true",
                    () => SkipReasonForExecute[Execute.InContainer]);

                if (err != null) return err;
            }

            if ((Execute.InDebug & execute) != 0)
            {
#if DEBUG
#else
                return SkipReasonForExecute[Execute.InDebug];
#endif
            }

            return null;
        }

        internal static string Resolve(On on)
        {
            if ((On.Windows & on) == On.Windows && !RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return SkipReasonForOn[On.Windows];
            }

            if ((On.Linux & on) == On.Linux && !RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return SkipReasonForOn[On.Linux];
            }

            if ((On.MacOS & on) == On.MacOS && !RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return SkipReasonForOn[On.MacOS];
            }

            return null;
        }

        private static string ValidateEnvVariablesExists(IEnumerable<string> names, Func<string> errorSelector)
        {
            foreach (var name in names)
            {
                var val = Environment.GetEnvironmentVariable(name);
                if (string.IsNullOrEmpty(val))
                {
                    return errorSelector();
                }
            }

            return null;
        }

        private static string ValidateEnvVariableValue(string name, string value, Func<string> errorSelector)
        {
            var val = Environment.GetEnvironmentVariable(name);
            if (string.IsNullOrEmpty(val) || val.Equals(value, StringComparison.InvariantCultureIgnoreCase))
            {
                return SkipReasonForExecute[Execute.InContainer];
            }

            return null;
        }
    }
}
