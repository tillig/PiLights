using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PiLights.Models;

namespace PiLights.Configuration
{
    public class GlobalConfiguration
    {
        private const string SettingsFileBaseName = "globalsettings";

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
                .SetBasePath(environment.ContentRootPath)
                .AddJsonFile($"{SettingsFileBaseName}.json", false, true)
                .Build();
            var mergedConfig = new ConfigurationBuilder()
                .SetBasePath(environment.ContentRootPath)
                .AddJsonFile($"{SettingsFileBaseName}.json", false, true)
                .AddJsonFile($"{SettingsFileBaseName}.{environment.EnvironmentName}.json", true, true)
                .Build();

            return new GlobalConfiguration(baseConfig, mergedConfig, environment);
        }

        public GlobalConfigurationSettings GetSettings()
        {
            return this.MergedConfiguration.Get<GlobalConfigurationSettings>();
        }

        public void Save(GlobalConfigurationSettings settings)
        {
            using (var writer = File.CreateText($"{SettingsFileBaseName}.{this.Environment.EnvironmentName}.json"))
            {
                this.Save(writer, settings);
            }
        }

        public void Save(StreamWriter writer, GlobalConfigurationSettings settings)
        {
            var settingsDictionary = settings
                .GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .ToDictionary(prop => prop.Name, prop => prop.GetValue(settings, null).ToString());
            var mergedConfig = new ConfigurationBuilder().AddInMemoryCollection(settingsDictionary).Build();
            var diff = ConfigurationDiff.Calculate(this.BaseConfiguration, mergedConfig);
            var serialized = JsonConvert.SerializeObject(diff, Formatting.Indented);
            writer.Write(serialized);
            writer.Flush();
        }
    }
}
