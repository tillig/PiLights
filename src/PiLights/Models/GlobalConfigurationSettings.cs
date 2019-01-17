using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PiLights.Models
{
    public class GlobalConfigurationSettings
    {
        public int LedCount { get; set; }

        public LedType LedType { get; set; }

        public string ServerIP { get; set; }

        public int ServerPort { get; set; }
    }
}
