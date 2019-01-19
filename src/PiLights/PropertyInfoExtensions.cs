using System;
using System.ComponentModel;
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
    }
}
