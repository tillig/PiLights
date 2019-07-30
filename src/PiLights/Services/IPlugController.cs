using System;
using System.Linq;
using System.Threading.Tasks;
using PiLights.Models;

namespace PiLights.Services
{
    public interface IPlugController
    {
        Task ShutdownAsync();

        Task StartupAsync();

        Task<PlugStatus> StatusAsync();
    }
}
