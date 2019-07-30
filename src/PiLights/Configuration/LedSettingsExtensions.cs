using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace PiLights.Configuration
{
    public static class LedSettingsExtensions
    {
        [SuppressMessage("CA1031", "CA1031", Justification = "The exception catch block is part of a retry and report policy.")]
        public static bool SendDataToSocket(this LedSettings config, string script)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            var msg = Encoding.ASCII.GetBytes(script);
            var ipAddress = IPAddress.Parse(config.IPAddress);
            var endpoint = new IPEndPoint(ipAddress, config.Port);
            var tries = 0;
            var success = false;
            using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
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
            }

            return success;
        }
    }
}
