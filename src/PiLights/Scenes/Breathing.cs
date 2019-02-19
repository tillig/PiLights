using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using HandlebarsDotNet;
using Microsoft.AspNetCore.Mvc;
using PiLights.Configuration;
using PiLights.Services;

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
        }

        [DisplayName("Breath Speed")]
        [Range(1, 1000)]
        public int BreathSpeed { get; set; }

        [DisplayName(nameof(Color))]
        [DataType(nameof(Color))]
        [ModelBinder(BinderType = typeof(ColorModelBinder))]
        public string Color { get; set; }

        public override string GetSceneImplementation()
        {
            var data = new
            {
                color = this.Color,
                breathSpeed = this.BreathSpeed,
                defaultBrightness = this.Settings.GlobalBrightness,
            };

            var generated = CompiledTemplate(data);
            return generated;
        }
    }
}
