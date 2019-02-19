using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using HandlebarsDotNet;
using PiLights.Configuration;

namespace PiLights.Scenes
{
    [DisplayName(nameof(Breathing))]
    public class Breathing : Scene
    {
        private const string Template = @"fill 1,{{color}}
render
do
  fade 1,{{defaultBrightness}},0,{{breathSpeed}},1
  render
  fade 1,0,{{defaultBrightness}},{{breathSpeed}},1
  render
loop";

        private static readonly Func<object, string> CompiledTemplate = Handlebars.Compile(Template);

        public Breathing(GlobalConfigurationSettings settings)
            : base(settings)
        {
            this.BreathSpeed = 20;
            this.Color = Color.Red;
        }

        [DisplayName("Breath Speed")]
        [Range(1, 1000)]
        public int BreathSpeed { get; set; }

        [DisplayName(nameof(Color))]
        [Required]
        public Color Color { get; set; }

        public override string GetSceneImplementation()
        {
            var data = new
            {
                color = this.Color.ToLedColor(),
                breathSpeed = this.BreathSpeed,
                defaultBrightness = this.Settings.GlobalBrightness,
            };

            var generated = CompiledTemplate(data);
            return generated;
        }
    }
}
