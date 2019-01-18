using System;
using System.Linq;

namespace PiLights.Models
{
    public class GlobalConfigurationSettings
    {
        public int GlobalBrightness { get; set; }

        public int LedCount { get; set; }

        public LedType LedType { get; set; }

        public string ServerIP { get; set; }

        public int ServerPort { get; set; }
    }
}
