using System;
using System.IO;

namespace PiLights
{
    public static class LastKnownScene
    {
        /// <summary>
        /// The name of the file holding last known scene data.
        /// </summary>
        private const string LastKnownSceneFile = "pilights-scene.txt";

        public static string GetLastKnownScene()
        {
            string lastKnownScene = null;
            if (File.Exists(LastKnownSceneFile))
            {
                lastKnownScene = File.ReadAllText(LastKnownSceneFile);
            }

            return lastKnownScene;
        }

        public static void WriteLastKnownScene(string script)
        {
            using (var writer = File.CreateText(LastKnownSceneFile))
            {
                writer.WriteLine(script);
            }
        }
    }
}
