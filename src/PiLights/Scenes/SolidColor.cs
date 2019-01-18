using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using PiLights.Services;

namespace PiLights.Scenes
{
    [DisplayName("Solid Color")]
    public class SolidColor : Scene
    {
        private readonly string template = @"fill 1,{0}
render";

        [DisplayName(nameof(Color))]
        [TypeConverter(typeof(HexColorConverter))]
        public string Color { get; set; }

        public override string GetSceneImplementation()
        {
            return string.Format(CultureInfo.InvariantCulture, this.template, this.Color);
        }
    }
}
