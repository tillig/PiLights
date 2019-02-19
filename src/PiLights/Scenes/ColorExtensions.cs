using System;
using System.Drawing;
using System.Globalization;
using System.Linq;

namespace PiLights.Scenes
{
    public static class ColorExtensions
    {
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
    }
}
