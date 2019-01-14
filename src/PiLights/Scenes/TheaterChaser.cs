using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace PiLights.Scenes
{
    [DisplayName("Theater Chaser")]
    public class TheaterChaser : Scene
    {
        private readonly string template = @"fill 1,{0},0,1
render
do
  delay {1}
  rotate
  render
loop {2}";

        [DisplayName("Chase Speed")]
        public int ChaseSpeed { get; set; }

        [DisplayName("Color")]
        public string Color { get; set; }

        public override void Execute()
        {
            this.Command = string.Format(CultureInfo.InvariantCulture, this.template, this.Color, this.ChaseSpeed, ConfigurationManager.Configuration.LedCount);
            base.Execute();
        }
    }
}
