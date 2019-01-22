using System;
using System.ComponentModel;
using System.Linq;
using HandlebarsDotNet;
using PiLights.Services;

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
  rotate
  render
loop";

        private static readonly Func<object, string> CompiledTemplate = Handlebars.Compile(Template);

        // Ideally there'd be a way to limit this to something that's evenly
        // divisible into the LED quantity. 600 LEDs, 20 chasers works; 600 LEDs
        // with 17 chasers doesn't. Depending on how the template works, I was able
        // to actually crash the ws2812svr process and hang the whole RPi.
        [DisplayName("Chase Quantity")]
        [Max(10)]
        public int ChaseQuantity { get; set; }

        [DisplayName("Chase Speed")]
        [Max(10)]
        public int ChaseSpeed { get; set; }

        [DisplayName(nameof(Color))]
        [TypeConverter(typeof(HexColorConverter))]
        public string Color { get; set; }

        public override string GetSceneImplementation()
        {
            // This is how much space should be between each chaser.
            // 600 LEDs, 20 chasers = every 30 lights is a chaser light.
            var chaseSpacer = ConfigurationManager.Configuration.LedCount / this.ChaseQuantity;

            // Generate the sequence of light indexes that should be on
            // to start with.
            var chasers = Enumerable.Range(0, ConfigurationManager.Configuration.LedCount - 1)
                .Where(i => i % chaseSpacer == 0)
                .ToArray();

            var data = new
            {
                color = this.Color,
                chaseSpeed = this.ChaseSpeed,
                chasers,
            };

            var generated = CompiledTemplate(data);
            return generated;
        }
    }
}
