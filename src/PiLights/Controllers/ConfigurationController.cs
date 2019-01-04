using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace PiLights.Controllers
{
    public class ConfigurationController : Controller
    {
        public IActionResult Index()
        {
            return this.View();
        }
    }
}