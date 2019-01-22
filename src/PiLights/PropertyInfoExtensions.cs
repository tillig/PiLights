using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace PiLights
{
    public static class PropertyInfoExtensions
    {
        public static string GetDisplayName(this PropertyInfo property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            var display = property.GetCustomAttribute<DisplayNameAttribute>();
            if (display != null)
            {
                return display.DisplayName;
            }

            return property.Name;
        }

        public static int GetMaxLength(this PropertyInfo property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            var maxLength = property.GetCustomAttribute<MaxLengthAttribute>();
            if (maxLength != null)
            {
                return maxLength.Length;
            }

            return int.MaxValue;
        }

        public static int GetMaxValue(this PropertyInfo property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            var attr = property.GetCustomAttribute<MaxAttribute>();
            if (attr != null)
            {
                return attr.Maximum;
            }

            return int.MaxValue;
        }
    }
}
