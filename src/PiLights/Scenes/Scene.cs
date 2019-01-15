using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace PiLights.Scenes
{
    public abstract class Scene
    {
        private const string Wrapper = @"setup 1,{0},{1}
init
thread_start
{2}
thread_stop
";

        public virtual void Execute()
        {
            // Commands via TCP need to...
            // - send the init directive
            // - start/end a thread around the render part
            // - use semicolons to separate commands
            // - end with a semicolon
            // - disconnect when the command is done being sent
            //
            // ws2812svr gets super picky if you miss any of those things
            // and won't render anything. The end semicolon in particular
            // will get you.
            var script = this.WrapScriptWithSetup(this.GenerateScript()).Trim().Replace("\r\n", "\n", StringComparison.Ordinal).Replace('\n', ';') + ";";
            var byData = System.Text.Encoding.ASCII.GetBytes(script);

            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var ipAdd = System.Net.IPAddress.Parse(ConfigurationManager.Configuration.ServerIP);
            var remoteEP = new IPEndPoint(ipAdd, ConfigurationManager.Configuration.ServerPort);

            socket.Connect(remoteEP);
            socket.Send(byData);
            socket.Disconnect(true);
            socket.Close();
        }

        public abstract string GenerateScript();

        private string WrapScriptWithSetup(string script)
        {
            return string.Format(CultureInfo.InvariantCulture, Wrapper, ConfigurationManager.Configuration.LedCount, (int)ConfigurationManager.Configuration.LedType, script);
        }
    }
}
