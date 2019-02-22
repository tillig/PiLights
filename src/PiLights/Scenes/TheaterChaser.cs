using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Text;
using HandlebarsDotNet;
using PiLights.Configuration;

namespace PiLights.Scenes
{
    [DisplayName("Theater Chaser")]
    public class TheaterChaser : Scene
    {
        public TheaterChaser(GlobalConfigurationSettings settings)
            : base(settings)
        {
            this.PrimaryColor = Color.White;
            this.SecondaryColor = Color.White;
            this.ChaseQuantity = settings.LedCount / 20;
            this.ChaseSpeed = 20;
            this.Gradient = false;
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

        [DisplayName("Gradient Between Chasers")]
        public bool? Gradient { get; set; }

        [DisplayName("Primary Color")]
        [Required]
        public Color PrimaryColor { get; set; }

        [DisplayName("Reverse Direction")]
        public bool? Reverse { get; set; }

        [DisplayName("Secondary Color")]
        [Required]
        public Color SecondaryColor { get; set; }

        public override string GetSceneImplementation()
        {
            // This is how much space should be between each chaser.
            // 600 LEDs, 20 chasers = every 30 lights is a chaser light.
            var chaseSpacer = this.Settings.LedCount / this.ChaseQuantity;

            // Generate the sequence of light indexes that should be on
            // to start with.
            var allChasers = Enumerable.Range(0, this.Settings.LedCount - 1)
                .Where(i => i % chaseSpacer == 0)
                .ToArray();

            var script = new StringBuilder();
            if (this.Gradient.HasValue && this.Gradient.Value)
            {
                // Use gradients to fill the initial lights.
                var currentColor = this.PrimaryColor;
                var nextColor = this.SecondaryColor;
                for (var chaserIndex = 0; chaserIndex < allChasers.Length; chaserIndex++)
                {
                    var currentLed = allChasers[chaserIndex];
                    int nextLed;
                    if (chaserIndex + 1 == allChasers.Length)
                    {
                        // TODO: If there's no next chaser the gradient will be off - calculate a partial gradient.
                        // There is no next chaser LED - look at the end of the strip instead.
                        nextLed = this.Settings.LedCount - 1;
                    }
                    else
                    {
                        // There's another chaser coming, gradient between current and next.
                        nextLed = allChasers[chaserIndex + 1];
                    }

                    if (currentLed == nextLed)
                    {
                        // It's possible the last chaser is the last light in the string.
                        break;
                    }

                    script.AppendLine($"gradient 1,R,{currentColor.R},{nextColor.R},{currentLed},{nextLed - currentLed}");
                    script.AppendLine($"gradient 1,G,{currentColor.G},{nextColor.G},{currentLed},{nextLed - currentLed}");
                    script.AppendLine($"gradient 1,B,{currentColor.B},{nextColor.B},{currentLed},{nextLed - currentLed}");

                    var tempColor = currentColor;
                    currentColor = nextColor;
                    nextColor = tempColor;
                }
            }
            else
            {
                // Simply render the solid color in each light.
                var primary = this.PrimaryColor.ToLedColor();
                var secondary = this.SecondaryColor.ToLedColor();
                for (var chaserIndex = 0; chaserIndex < allChasers.Length; chaserIndex++)
                {
                    var ledIndex = allChasers[chaserIndex];
                    if (chaserIndex % 2 == 0)
                    {
                        script.AppendLine($"fill 1,{primary},{ledIndex},1");
                    }
                    else
                    {
                        script.AppendLine($"fill 1,{secondary},{ledIndex},1");
                    }
                }
            }

            var direction = this.Reverse.HasValue && this.Reverse.Value ? "0" : "1";
            script.AppendLine("render");
            script.AppendLine("do");
            script.AppendLine($"delay {this.ChaseSpeed}");
            script.AppendLine($"rotate 1,1,{direction}");
            script.AppendLine("render");
            script.AppendLine("loop");

            return script.ToString();
        }
    }
}
