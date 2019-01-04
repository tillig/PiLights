using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace PiLights.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return this.View();
        }
    }
}
