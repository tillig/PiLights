using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PiLights.Models
{
    public class GlobalConfigurationSettings
    {
        [DisplayName("Global Brightness")]
        [Range(1, 255)]
        [Required]
        public int GlobalBrightness { get; set; }

        [DisplayName("LED Count")]
        [Range(1, int.MaxValue)]
        [Required]
        public int LedCount { get; set; }

        [DisplayName("LED Type")]
        [Required]
        [DataType("Enum")]
        public LedType LedType { get; set; }

        [DisplayName("Server IP Address")]
        [Required]
        public string ServerIP { get; set; }

        [DisplayName("Server Port")]
        [Range(1, 32767)]
        [Required]
        public int ServerPort { get; set; }
    }
}
