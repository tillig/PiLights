using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PiLights.Configuration;
using Xunit;

namespace PiLights.Test.Configuration
{
    public class GlobalConfigurationTest
    {
        [Fact]
        public void Save_SavesDiff()
        {
            var globalConf = new GlobalConfiguration(GetBaseConfiguration(), GetMergedConfiguration(), new HostingEnvironment() { EnvironmentName = "Test" });
            var updatedSettings = new GlobalConfigurationSettings
            {
                GlobalBrightness = 64,
                LedCount = 540,
                LedType = LedType.WS2811_STRIP_GBR,
                ServerIP = "111.222.000.111",
                ServerPort = 9999,
            };

            string serialized;
            using (var stream = new MemoryStream())
            using (var writer = new StreamWriter(stream, Encoding.UTF8, 10, true))
            using (var reader = new StreamReader(stream, Encoding.UTF8, false, 10, true))
            {
                globalConf.Save(writer, updatedSettings);
                stream.Position = 0;
                serialized = reader.ReadToEnd();
            }

            var deserialized = JsonConvert.DeserializeObject<Dictionary<string, string>>(serialized);
            Assert.Single(deserialized);
            Assert.Equal("111.222.000.111", deserialized["ServerIP"]);
        }

        private static IConfiguration GetBaseConfiguration()
        {
            var settings = new Dictionary<string, string>()
            {
                { "globalBrightness", "64" },
                { "ledCount", "540" },
                { "ledType", "WS2811_STRIP_GBR" },
                { "serverIp", "127.0.0.1" },
                { "serverPort", "9999" },
            };

            return new ConfigurationBuilder().AddInMemoryCollection(settings).Build();
        }

        private static IConfiguration GetMergedConfiguration()
        {
            var settings = new Dictionary<string, string>()
            {
                { "globalBrightness", "64" },
                { "ledCount", "540" },
                { "ledType", "WS2811_STRIP_GBR" },
                { "serverIp", "192.168.1.208" },
                { "serverPort", "9999" },
            };

            return new ConfigurationBuilder().AddInMemoryCollection(settings).Build();
        }
    }
}
