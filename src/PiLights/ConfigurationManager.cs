using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Newtonsoft.Json;
using PiLights.Models;

namespace PiLights
{
    // TODO: Make global configuration use IConfiguration.
    // TODO: Persist JSON settings by environment.
    public static class ConfigurationManager
    {
        /// <summary>
        /// The name of the file holding configuration data.
        /// </summary>
        private const string ConfigurationFile = "pilights-config.txt";

        /// <summary>
        /// The name of the file holding last known scene data.
        /// </summary>
        private const string LastKnownSceneFile = "pilights-scene.txt";

        private static Lazy<GlobalConfigurationSettings> lazyConfiguration = new Lazy<GlobalConfigurationSettings>(() => GetConfiguration());

        public static GlobalConfigurationSettings Configuration
        {
            get
            {
                return lazyConfiguration.Value;
            }
        }

        public static bool SendDataToSocket(string script)
        {
            var msg = System.Text.Encoding.ASCII.GetBytes(script);
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var ipAddress = System.Net.IPAddress.Parse(Configuration.ServerIP);
            var endpoint = new IPEndPoint(ipAddress, Configuration.ServerPort);
            var tries = 0;
            var success = false;

            while (tries < 1 && !success)
            {
                try
                {
                    socket.Connect(endpoint);

                    // This sleep is required to make things consistently work.
                    // https://github.com/tom-2015/rpi-ws2812-server/issues/25
                    Thread.Sleep(200);
                    socket.Send(msg, 0, msg.Length, SocketFlags.None);
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                    success = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.InnerException);
                    Thread.Sleep(5000);
                    tries++;
                }
            }

            return success;
        }

        public static string GetLastKnownScene()
        {
            string lastKnownScene = null;
            if (File.Exists(LastKnownSceneFile))
            {
                lastKnownScene = File.ReadAllText(LastKnownSceneFile);
            }

            return lastKnownScene;
        }

        public static void WriteConfiguration(GlobalConfigurationSettings model)
        {
            var serializedModel = JsonConvert.SerializeObject(model);
            using (StreamWriter writer = File.CreateText(ConfigurationFile))
            {
                writer.WriteLine(serializedModel);
            }

            lazyConfiguration = new Lazy<GlobalConfigurationSettings>(() => GetConfiguration());
        }

        public static void WriteLastKnownScene(string script)
        {
            using (StreamWriter writer = File.CreateText(LastKnownSceneFile))
            {
                writer.WriteLine(script);
            }
        }

        private static GlobalConfigurationSettings GetConfiguration()
        {
            var model = new GlobalConfigurationSettings();
            if (File.Exists(ConfigurationFile))
            {
                var serializedModel = File.ReadAllText(ConfigurationFile);
                model = JsonConvert.DeserializeObject<GlobalConfigurationSettings>(serializedModel);
            }

            return model;
        }
    }
}
