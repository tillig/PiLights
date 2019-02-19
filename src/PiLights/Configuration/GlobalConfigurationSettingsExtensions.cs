using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace PiLights.Configuration
{
    public static class GlobalConfigurationSettingsExtensions
    {
        public static bool SendDataToSocket(this GlobalConfigurationSettings config, string script)
        {
            var msg = System.Text.Encoding.ASCII.GetBytes(script);
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var ipAddress = IPAddress.Parse(config.ServerIP);
            var endpoint = new IPEndPoint(ipAddress, config.ServerPort);
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
    }
}
