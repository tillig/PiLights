using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace PiLights.Services
{
    public class HexColorConverter : TypeConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(string) || base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType != typeof(string))
            {
                return base.ConvertTo(context, culture, value, destinationType);
            }

            var s = value as string;
            if (s == null)
            {
                return null;
            }

            s = s.Trim();
            if (!Regex.IsMatch(s, "#[0-9A-Fa-f]{6}"))
            {
                throw new FormatException("String to convert from must be in the hex color format #012345.");
            }

            return s.Substring(1);
        }
    }
}
