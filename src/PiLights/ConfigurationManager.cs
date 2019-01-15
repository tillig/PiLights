using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json;
using PiLights.Models;

namespace PiLights
{
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

        private static Lazy<ConfigurationModel> lazyConfiguration = new Lazy<ConfigurationModel>(() => GetConfiguration());

        public static ConfigurationModel Configuration
        {
            get
            {
                return lazyConfiguration.Value;
            }
        }

        public static void SendDataToSocket(string script)
        {
            var byData = System.Text.Encoding.ASCII.GetBytes(script);

            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var ipAdd = System.Net.IPAddress.Parse(Configuration.ServerIP);
            var remoteEP = new IPEndPoint(ipAdd, Configuration.ServerPort);

            socket.Connect(remoteEP);
            socket.Send(byData);
            socket.Disconnect(true);
            socket.Close();
        }

        public static string GetLastKnownScene()
        {
            string lastKnownScene = null;
            if (System.IO.File.Exists(LastKnownSceneFile))
            {
                lastKnownScene = System.IO.File.ReadAllText(LastKnownSceneFile);
            }

            return lastKnownScene;
        }

        public static void WriteConfiguration(ConfigurationModel model)
        {
            var serializedModel = JsonConvert.SerializeObject(model);
            using (StreamWriter writer = System.IO.File.CreateText(ConfigurationFile))
            {
                writer.WriteLine(serializedModel);
            }

            lazyConfiguration = new Lazy<ConfigurationModel>(() => GetConfiguration());
        }

        public static void WriteLastKnownScene(string script)
        {
            using (StreamWriter writer = System.IO.File.CreateText(LastKnownSceneFile))
            {
                writer.WriteLine(script);
            }
        }

        private static ConfigurationModel GetConfiguration()
        {
            var model = new ConfigurationModel();
            if (System.IO.File.Exists(ConfigurationFile))
            {
                var serializedModel = System.IO.File.ReadAllText(ConfigurationFile);
                model = JsonConvert.DeserializeObject<ConfigurationModel>(serializedModel);
            }

            return model;
        }
    }
}
