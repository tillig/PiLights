using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PiLights.Models;
using PiLights.Services;

namespace PiLights.Controllers
{
    public class PowerController : Controller
    {
        public PowerController(IPlugController plugController)
        {
            this.PlugController = plugController ?? throw new ArgumentNullException(nameof(plugController));
        }

        public IPlugController PlugController { get; private set; }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Shutdown()
        {
            await this.PlugController.ShutdownAsync();
            return this.Ok();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Startup()
        {
            await this.PlugController.StartupAsync();
            return this.Ok();
        }

        [HttpGet]
        public async Task<ActionResult<PlugStatus>> Status()
        {
            return await this.PlugController.StatusAsync();
        }
    }
}
