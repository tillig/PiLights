using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using PiLights.Models;
using PiLights.Properties;
using PiLights.Scenes;

namespace PiLights.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(IEnumerable<Scene> scenes)
        {
            this.Scenes = scenes ?? throw new ArgumentNullException(nameof(scenes));
        }

        public IEnumerable<Scene> Scenes { get; private set; }

        [HttpGet]
        public IActionResult Index()
        {
            this.ViewBag.Scenes = this.Scenes.Select(x => new SelectListItem { Text = x.GetDisplayName(), Value = x.GetType().FullName });
            return this.View();
        }

        [HttpPost]
        public IActionResult SceneProperties(string sceneName)
        {
            var scene = this.Scenes.First(x => x.GetType().FullName == sceneName);
            return this.PartialView(scene);
        }

        [HttpPost]
        public async Task<IActionResult> StartScene(string sceneName)
        {
            var scene = this.Scenes.First(x => x.GetType().FullName == sceneName);
            if (!await this.TryUpdateModelAsync(scene, scene.GetType(), string.Empty))
            {
                this.ModelState.AddModelError(string.Empty, Resources.Home_BindError);
            }
            else if (!this.TryValidateModel(scene))
            {
                this.ModelState.AddModelError(string.Empty, Resources.Home_ValidateError);
            }

            if (!this.ModelState.IsValid)
            {
                return this.PartialView(scene);
            }

            this.SetAlert(scene.Execute());
            return this.RedirectToAction(nameof(this.Index));
        }

        private void SetAlert(bool success)
        {
            var alert = new Alert { MessageHtml = success ? Resources.Home_SceneSuccess : Resources.Home_SceneFail, Success = success };
            this.HttpContext.Session.SetString("alert-message", JsonConvert.SerializeObject(alert));
        }
    }
}
