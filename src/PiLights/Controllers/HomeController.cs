﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using PiLights.Models;
using PiLights.Scenes;
using PiLights.Services;

namespace PiLights.Controllers
{
    public class HomeController : Controller
    {
        private const string ErrorMessage = "Failed to start the scene. Please try again later.";

        private const string SuccessMessage = "Successfully started the scene.";

        public HomeController(SceneManager sceneManager)
        {
            this.SceneManager = sceneManager;
        }

        public SceneManager SceneManager { get; private set; }

        [HttpGet]
        public IActionResult Index()
        {
            this.ViewBag.Scenes = this.SceneManager.Scenes.Select(x => new SelectListItem { Text = x.GetDisplayName(), Value = x.GetType().FullName });
            return this.View();
        }

        [HttpPost]
        public IActionResult SceneProperties(string sceneName)
        {
            var scene = this.SceneManager.Scenes.First(x => x.GetType().FullName == sceneName);
            var sceneProperties = scene.GetSceneProperties();
            return this.PartialView(scene);
        }

        [HttpPost]
        public async Task<IActionResult> StartScene(string sceneName)
        {
            // TODO: Custom model binder for color - trim off the #
            // TODO: Bootstrap 4 editor templates for all controls: https://getbootstrap.com/docs/4.3/components/forms/
            // TODO: Separate scene properties from scene execution - use property object as model.
            var scene = this.SceneManager.Scenes.First(x => x.GetType().FullName == sceneName);
            if (!await this.TryUpdateModelAsync(scene, scene.GetType(), string.Empty))
            {
                this.ModelState.AddModelError(string.Empty, "Unable to bind properties to model.");
            }
            else if (!this.TryValidateModel(scene))
            {
                this.ModelState.AddModelError(string.Empty, "Unable to validate model.");
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
            var alert = new Alert { MessageHtml = success ? SuccessMessage : ErrorMessage, Success = success };
            this.HttpContext.Session.SetString("alert-message", JsonConvert.SerializeObject(alert));
        }
    }
}
