using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using PiLights.Services;
using Xunit;

namespace PiLights.Test.Services
{
    public class ConfigurationDiffTest
    {
        [Fact]
        public void Calculate_Difference()
        {
            var baseConfig = GetBaseConfiguration();
            var mergedConfig = GetMergedConfiguration();
            var diff = ConfigurationDiff.Calculate(baseConfig, mergedConfig);
            Assert.Equal(2, diff.Count);
            Assert.Equal("override2", diff["b:c"]);
            Assert.Equal("add4", diff["b:e"]);
        }

        [Fact]
        public void Calculate_NoDifference()
        {
            var baseConfig = GetBaseConfiguration();
            var mergedConfig = GetBaseConfiguration();
            var diff = ConfigurationDiff.Calculate(baseConfig, mergedConfig);
            Assert.Empty(diff);
        }

        [Fact]
        public void Calculate_NullBase()
        {
            var config = GetBaseConfiguration();
            Assert.Throws<ArgumentNullException>(() => ConfigurationDiff.Calculate(null, config));
        }

        [Fact]
        public void Calculate_NullMerged()
        {
            var config = GetBaseConfiguration();
            Assert.Throws<ArgumentNullException>(() => ConfigurationDiff.Calculate(config, null));
        }

        private static IConfiguration GetBaseConfiguration()
        {
            var settings = new Dictionary<string, string>()
            {
                { "a", "1" },
                { "b:c", "2" },
                { "b:d", "3" },
            };
            return new ConfigurationBuilder().AddInMemoryCollection(settings).Build();
        }

        private static IConfiguration GetMergedConfiguration()
        {
            var settings = new Dictionary<string, string>()
            {
                { "a", "1" },
                { "b:c", "override2" },
                { "b:d", "3" },
                { "b:e", "add4" },
            };
            return new ConfigurationBuilder().AddInMemoryCollection(settings).Build();
        }
    }
}
