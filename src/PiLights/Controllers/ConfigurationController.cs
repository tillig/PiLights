using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using PiLights.Configuration;

namespace PiLights.Controllers
{
    public class ConfigurationController : Controller
    {
        private readonly GlobalConfiguration _configuration;

        public ConfigurationController(GlobalConfiguration configuration)
        {
            this._configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        [HttpGet]
        public IActionResult Index()
        {
            return this.View(this._configuration.GetSettings());
        }

        [HttpPost]
        public IActionResult Index(GlobalConfigurationSettings model)
        {
            // TODO: Display a success message when the configuration is saved.
            this._configuration.Save(model);
            return this.View();
        }
    }
}