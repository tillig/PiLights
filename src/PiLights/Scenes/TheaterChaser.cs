using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using PiLights.Services;

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
loop";

        [DisplayName("Chase Speed")]
        public int ChaseSpeed { get; set; }

        [DisplayName("Chase Quantity")]
        public int ChaseQuantity { get; set; }

        [DisplayName(nameof(Color))]
        [TypeConverter(typeof(HexColorConverter))]
        public string Color { get; set; }

        public override string GetSceneImplementation()
        {
            return string.Format(CultureInfo.InvariantCulture, this.template, this.Color, this.ChaseSpeed);
        }
    }
}
