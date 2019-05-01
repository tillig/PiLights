using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Globalization;
using System.Linq;
using PiLights.Configuration;

namespace PiLights.Scenes
{
    [DisplayName("Solid Color")]
    public class SolidColor : Scene
    {
        private readonly string template = @"fill 1,{0}
render";

        public SolidColor(LedSettings settings)
            : base(settings)
        {
            this.Color = Color.White;
        }

        [DisplayName(nameof(Color))]
        [Required]
        public Color Color { get; set; }

        public override string GetSceneImplementation()
        {
            return string.Format(CultureInfo.InvariantCulture, this.template, this.Color.ToLedColor());
        }
    }
}
