using System;
using Microsoft.AspNetCore.Mvc;
using PiLights.Configuration;

namespace PiLights.Controllers
{
    public class ConfigurationController : Controller
    {
        private readonly LedSettings _configuration;

        public ConfigurationController(LedSettings configuration)
        {
            this._configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        [HttpGet]
        public IActionResult Index()
        {
            return this.View(this._configuration);
        }
    }
}