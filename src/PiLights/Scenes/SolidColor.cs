using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace PiLights.Scenes
{
    [DisplayName("Solid Color")]
    public class SolidColor : Scene
    {
        private readonly string template = @"fill 1,{0}
render";

        [DisplayName(nameof(Color))]
        public string Color { get; set; }

        public override string GenerateScript()
        {
            return string.Format(CultureInfo.InvariantCulture, this.template, this.Color);
        }
    }
}
