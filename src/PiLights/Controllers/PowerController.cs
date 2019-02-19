using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using PiLights.Interop;

namespace PiLights.Controllers
{
    public class PowerController : Controller
    {
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Shutdown()
        {
            Linux.Shutdown();
            return this.View("ShutdownComplete");
        }
    }
}
