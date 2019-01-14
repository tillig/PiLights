using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace PiLights.Scenes
{
    public static class SceneExtensions
    {
        public static string GetDisplayName(this Scene scene)
        {
            var attrib = scene.GetType().GetCustomAttribute<DisplayNameAttribute>();
            if (attrib == null)
            {
                return scene.GetType().Name;
            }

            return attrib.DisplayName;
        }

        public static PropertyInfo[] GetSceneProperties(this Scene scene)
        {
            return scene.GetType().GetProperties();
        }
    }
}
