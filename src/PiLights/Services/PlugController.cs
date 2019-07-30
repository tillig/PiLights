using System;
using System.Linq;
using System.Threading.Tasks;
using PiLights.Models;
using TPLink.SmartHome;

namespace PiLights.Services
{
    public class PlugController : IPlugController
    {
        public PlugController(PlugClient client)
        {
            this.Client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public PlugClient Client { get; private set; }

        public async Task ShutdownAsync()
        {
            await this.Client.SetOutputAsync(OutputState.Off);
        }

        public async Task StartupAsync()
        {
            await this.Client.SetOutputAsync(OutputState.On);
        }

        public async Task<PlugStatus> StatusAsync()
        {
            var output = await this.Client.GetOutputAsync();
            return new PlugStatus { On = output == OutputState.On };
        }
    }
}
