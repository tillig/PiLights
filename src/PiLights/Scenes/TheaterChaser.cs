using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using HandlebarsDotNet;
using PiLights.Configuration;

namespace PiLights.Scenes
{
    [DisplayName("Theater Chaser")]
    public class TheaterChaser : Scene
    {
        private const string Template = @"{{#each chasers}}fill 1,{{../color}},{{this}},1
{{/each}}
render
do
  delay {{chaseSpeed}}
  rotate 1, 1, {{direction}}
  render
loop";

        private static readonly Func<object, string> CompiledTemplate = Handlebars.Compile(Template);

        public TheaterChaser(GlobalConfigurationSettings settings)
            : base(settings)
        {
            this.Color = Color.White;
            this.ChaseQuantity = settings.LedCount / 20;
            this.ChaseSpeed = 20;
            this.Reverse = false;
        }

        // Ideally there'd be a way to limit this to something that's evenly
        // divisible into the LED quantity. 600 LEDs, 20 chasers works; 600 LEDs
        // with 17 chasers doesn't. Depending on how the template works, I was able
        // to actually crash the ws2812svr process and hang the whole RPi.
        [DisplayName("Chase Quantity")]
        [Range(1, 1000)]
        public int ChaseQuantity { get; set; }

        [DisplayName("Chase Speed")]
        [Range(1, 1000)]
        public int ChaseSpeed { get; set; }

        [DisplayName(nameof(Color))]
        [Required]
        public Color Color { get; set; }

        [DisplayName("Reverse Direction")]
        public bool? Reverse { get; set; }

        public override string GetSceneImplementation()
        {
            // This is how much space should be between each chaser.
            // 600 LEDs, 20 chasers = every 30 lights is a chaser light.
            var chaseSpacer = this.Settings.LedCount / this.ChaseQuantity;

            // Generate the sequence of light indexes that should be on
            // to start with.
            var chasers = Enumerable.Range(0, this.Settings.LedCount - 1)
                .Where(i => i % chaseSpacer == 0)
                .ToArray();

            var data = new
            {
                color = this.Color.ToLedColor(),
                chaseSpeed = this.ChaseSpeed,
                direction = this.Reverse.HasValue && this.Reverse.Value ? 0 : 1,
                chasers,
            };

            var generated = CompiledTemplate(data);
            return generated;
        }
    }
}
