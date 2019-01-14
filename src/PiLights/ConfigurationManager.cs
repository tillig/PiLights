using System;
using System.IO;
using Newtonsoft.Json;
using PiLights.Models;

namespace PiLights
{
    public static class ConfigurationManager
    {
        /// <summary>
        /// The name of the file holding configuration data.
        /// </summary>
        private const string ConfigurationFile = "pilights-config.txt";

        private static readonly Lazy<ConfigurationModel> LazyConfiguration = new Lazy<ConfigurationModel>(() => GetConfiguration());

        public static ConfigurationModel Configuration
        {
            get
            {
                return LazyConfiguration.Value;
            }
        }

        public static void WriteConfiguration(ConfigurationModel model)
        {
            var serializedModel = JsonConvert.SerializeObject(model);
            using (StreamWriter writer = System.IO.File.CreateText(ConfigurationFile))
            {
                writer.WriteLine(serializedModel);
            }
        }

        private static ConfigurationModel GetConfiguration()
        {
            var model = new ConfigurationModel();
            if (System.IO.File.Exists(ConfigurationFile))
            {
                var serializedModel = System.IO.File.ReadAllText(ConfigurationFile);
                model = JsonConvert.DeserializeObject<ConfigurationModel>(serializedModel);
            }

            return model;
        }
    }
}
