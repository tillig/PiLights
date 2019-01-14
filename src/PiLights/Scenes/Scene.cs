using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace PiLights.Scenes
{
    public abstract class Scene
    {
        internal string Command;

        public virtual void Execute()
        {
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var ipAdd = System.Net.IPAddress.Parse(ConfigurationManager.Configuration.ServerIP);
            var remoteEP = new IPEndPoint(ipAdd, ConfigurationManager.Configuration.ServerPort);

            socket.Connect(remoteEP);

            byte[] byData = System.Text.Encoding.ASCII.GetBytes(this.Command);
            socket.Send(byData);
            socket.Disconnect(false);
            socket.Close();
        }
    }
}
