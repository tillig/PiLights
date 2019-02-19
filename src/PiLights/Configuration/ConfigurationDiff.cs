using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace PiLights.Configuration
{
    public static class ConfigurationDiff
    {
        /// <summary>
        /// Calculates the differences/overrides between a base configuration object and
        /// a fully merged configuration object.
        /// </summary>
        /// <param name="baseConfiguration">The base configuration. Does not contain any overrides.</param>
        /// <param name="mergedConfiguration">The merged configuration. Contains overrides and additions the base doesn't have.</param>
        /// <returns>A key/value collection of configuration that, if added to the <paramref name="baseConfiguration" /> would result in the <paramref name="mergedConfiguration" />.</returns>
        public static IDictionary<string, string> Calculate(IConfiguration baseConfiguration, IConfiguration mergedConfiguration)
        {
            if (baseConfiguration == null)
            {
                throw new ArgumentNullException(nameof(baseConfiguration));
            }

            if (mergedConfiguration == null)
            {
                throw new ArgumentNullException(nameof(mergedConfiguration));
            }

            var diff = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var pair in mergedConfiguration.AsEnumerable())
            {
                var baseValue = baseConfiguration.GetSection(pair.Key).Value;
                if (!string.IsNullOrEmpty(pair.Value) &&
                    (string.IsNullOrEmpty(baseValue) || !pair.Value.Equals(baseValue, StringComparison.Ordinal)))
                {
                    diff[pair.Key] = pair.Value;
                }
            }

            return diff;
        }
    }
}
