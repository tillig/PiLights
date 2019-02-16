using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using PiLights.Models;

namespace PiLights
{
    public class GlobalConfiguration
    {
        public GlobalConfiguration(
            IConfiguration baseConfiguration,
            IConfiguration mergedConfiguration,
            IHostingEnvironment environment)
        {
            this.BaseConfiguration = baseConfiguration ?? throw new ArgumentNullException(nameof(baseConfiguration));
            this.MergedConfiguration = mergedConfiguration ?? throw new ArgumentNullException(nameof(mergedConfiguration));
            this.Environment = environment ?? throw new ArgumentNullException(nameof(environment));
        }

        public IConfiguration BaseConfiguration { get; private set; }

        public IHostingEnvironment Environment { get; private set; }

        public IConfiguration MergedConfiguration { get; private set; }

        public static GlobalConfiguration Load(IHostingEnvironment environment)
        {
            var baseConfig = new ConfigurationBuilder()
                .AddJsonFile("globalsettings.json", true, true)
                .Build();
            var mergedConfig = new ConfigurationBuilder()
                .AddJsonFile("globalsettings.json", true, true)
                .AddJsonFile($"globalsettings.{environment.EnvironmentName}.json", true, true)
                .Build();

            return new GlobalConfiguration(baseConfig, mergedConfig, environment);
        }

        public GlobalConfigurationSettings GetSettings()
        {
            return this.MergedConfiguration.Get<GlobalConfigurationSettings>();
        }

        public void Save(GlobalConfigurationSettings settings)
        {
            using (var writer = File.CreateText($"globalsettings.{this.Environment.EnvironmentName}.json"))
            {
                this.Save(writer, settings);
            }
        }

        public void Save(StreamWriter writer, GlobalConfigurationSettings settings)
        {
            // TODO: Calculate the differences between the base settings and the passed in settings.
            // TODO: Save the settings differences to the stream.
        }
    }
}
