using System;
using System.Linq;
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
        public ActionResult Shutdown()
        {
            this.PlugController.Shutdown();
            return this.View("ShutdownComplete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Startup()
        {
            this.PlugController.Startup();
            return this.View("ShutdownComplete");
        }

        [HttpGet]
        public ActionResult<PlugStatus> Status()
        {
            return this.PlugController.Status();
        }
    }
}
