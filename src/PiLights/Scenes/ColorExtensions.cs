using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;

namespace PiLights.Scenes
{
    public static class ColorExtensions
    {
        public static IEnumerable<Color> Interpolate(this Color from, Color to, int steps)
        {
            if (steps < 2)
            {
                steps = 2;
            }

            var results = new List<Color>();
            var stepFactor = 1.0 / (steps - 1);
            for (var i = 0; i < steps; i++)
            {
                results.Add(InterpolateStep(from, to, stepFactor * i));
            }

            return results;
        }

        public static string ToHexColor(this Color color)
        {
            return "#" + color.ToLedColor();
        }

        public static string ToLedColor(this Color color)
        {
            return color.R.ToString("x2", CultureInfo.InvariantCulture) +
                color.G.ToString("x2", CultureInfo.InvariantCulture) +
                color.B.ToString("x2", CultureInfo.InvariantCulture);
        }

        private static Color InterpolateStep(Color from, Color to, double factor)
        {
            var r = (int)Math.Round(from.R + (factor * (to.R - from.R)), 0, MidpointRounding.AwayFromZero);
            var g = (int)Math.Round(from.G + (factor * (to.G - from.G)), 0, MidpointRounding.AwayFromZero);
            var b = (int)Math.Round(from.B + (factor * (to.B - from.B)), 0, MidpointRounding.AwayFromZero);
            return Color.FromArgb(r, g, b);
        }
    }
}
