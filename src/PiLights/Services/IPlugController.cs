using System;
using System.Linq;
using PiLights.Models;

namespace PiLights.Services
{
    public interface IPlugController
    {
        void Shutdown();

        void Startup();

        PlugStatus Status();
    }
}
