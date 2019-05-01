using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using PiLights.Configuration;
using Xunit;

namespace PiLights.Test.Configuration
{
    public class LedSettingsTest
    {
        [Fact]
        public void Get_DeserializesLedSettings()
        {
            var configuration = GetConfiguration();
            var settings = configuration.GetSection("ws281x").Get<LedSettings>();

            Assert.Equal(64, settings.GlobalBrightness);
            Assert.Equal(540, settings.LedCount);
            Assert.Equal(LedType.WS2811_STRIP_GBR, settings.LedType);
            Assert.Equal("127.0.0.1", settings.IPAddress);
            Assert.Equal(9999, settings.Port);
        }

        private static IConfiguration GetConfiguration()
        {
            var settings = new Dictionary<string, string>()
            {
                { "ws281x:globalBrightness", "64" },
                { "ws281x:ledCount", "540" },
                { "ws281x:ledType", "WS2811_STRIP_GBR" },
                { "ws281x:ipAddress", "127.0.0.1" },
                { "ws281x:port", "9999" },
            };

            return new ConfigurationBuilder().AddInMemoryCollection(settings).Build();
        }
    }
}
