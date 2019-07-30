using System.ComponentModel;

namespace PiLights.Configuration
{
    public class LedSettings
    {
        [DisplayName("Global Brightness")]
        public int GlobalBrightness { get; set; }

        [DisplayName("LED Count")]
        public int LedCount { get; set; }

        [DisplayName("LED Type")]
        public LedType LedType { get; set; }

        [DisplayName("Server IP Address")]
        public string IPAddress { get; set; }

        [DisplayName("Server Port")]
        public int Port { get; set; }

        [DisplayName("Smart Plug IP Address")]
        public int SmartPlugIPAddress { get; set; }
    }
}
