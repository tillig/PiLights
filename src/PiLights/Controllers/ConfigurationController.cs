using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PiLights.Models;

namespace PiLights.Controllers
{
    public class ConfigurationController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return this.View(ConfigurationManager.Configuration);
        }

        [HttpPost]
        public IActionResult Index(GlobalConfigurationSettings model)
        {
            ConfigurationManager.WriteConfiguration(model);
            return this.View();
        }
    }
}