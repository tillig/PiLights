using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using LightCommandLanguage;

namespace TestSocketListener
{
    internal class Program
    {
        public static void Main()
        {
            TcpListener server = null;
            try
            {
                // Set the TcpListener on port 13000.
                int port = 9999;
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");

                // TcpListener server = new TcpListener(port);
                server = new TcpListener(localAddr, port);

                // Start listening for client requests.
                server.Start();

                // Buffer for reading data
                // Based on default buffer size for listener: https://github.com/tom-2015/rpi-ws2812-server/blob/e99cf3e5511295721ecb4c8a39819239942d54c6/main.c#L20
                byte[] bytes = new byte[32768];

                // Enter the listening loop.
                while (true)
                {
                    Console.Write("Waiting for a connection... ");

                    // Perform a blocking call to accept requests.
                    // You could also user server.AcceptSocket() here.
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");

                    // Get a stream object for reading and writing
                    NetworkStream stream = client.GetStream();
                    int i = stream.Read(bytes, 0, bytes.Length);

                    // Translate data bytes to a ASCII string.
                    string data = Encoding.ASCII.GetString(bytes, 0, i);
                    Console.WriteLine("Received: {0}", data);

                    // Validate the script.
                    ValidationListener result = LightCommandAnalyzer.AnalyzeScript(data);
                    if (result.ErrorCount != 0)
                    {
                        Console.WriteLine("{0} errors detected", result.ErrorCount);
                        foreach (string error in result.Errors)
                        {
                            Console.WriteLine("error: {0}", error);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Script is valid.");
                    }

                    // Shutdown and end connection - ws2812x server doesn't send any responses
                    client.Close();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                // Stop listening for new clients.
                server.Stop();
            }

            Console.WriteLine("\nHit enter to continue...");
            Console.Read();
        }
    }
}
