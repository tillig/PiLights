using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PiLights.Configuration;
using PiLights.Models;

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
            var alert = new Alert();
            try
            {
                this._configuration.Save(model);
                alert.MessageHtml = "Successfully saved settings.";
                alert.Success = true;
            }
            catch (Exception ex)
            {
                alert.MessageHtml = "Failed to save settings: " + ex.Message;
                alert.Success = false;
            }

            this.HttpContext.Session.SetString("alert-message", JsonConvert.SerializeObject(alert));
            return this.View();
        }
    }
}