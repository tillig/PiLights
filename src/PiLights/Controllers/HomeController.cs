using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using PiLights.Models;
using PiLights.Scenes;

namespace PiLights.Controllers
{
    public class HomeController : Controller
    {
        private const string ErrorMessage = "Failed to start the scene. Please try again later.";
        private const string SuccessMessage = "Successfully started the scene.";
        private readonly Lazy<IList<Scene>> scenes = new Lazy<IList<Scene>>(() => GetScenes());

        [HttpGet]
        public IActionResult Index()
        {
            this.ViewBag.Scenes = this.scenes.Value.Select(x => new SelectListItem { Text = x.GetDisplayName(), Value = x.GetType().FullName });
            return this.View();
        }

        [HttpPost]
        public IActionResult SceneProperties(string sceneName)
        {
            var scene = (Scene)Activator.CreateInstance(Type.GetType(sceneName));
            var sceneProperties = scene.GetSceneProperties();
            return this.PartialView(scene);
        }

        [HttpPost]
        public IActionResult StartScene(string sceneName)
        {
            var scene = (Scene)Activator.CreateInstance(Type.GetType(sceneName));
            var sceneProps = scene.GetSceneProperties();
            var properties = new Dictionary<string, string>();
            foreach (var prop in sceneProps)
            {
                this.Request.Form.TryGetValue(prop.Name, out var propValue);
                prop.SetValue(scene, Convert.ChangeType(propValue[0], prop.PropertyType, CultureInfo.InvariantCulture), null);
            }

            this.SetAlert(scene.Execute());
            return this.RedirectToAction("Index");
        }

        private static IList<Scene> GetScenes()
        {
            var all = Assembly
                .GetEntryAssembly()
                .GetTypes()
                .Where(type => typeof(Scene).GetTypeInfo().IsAssignableFrom(type));

            var scenes = new List<Scene>();
            foreach (var t in all)
            {
                if (!t.Equals(typeof(Scene)))
                {
                    scenes.Add((Scene)Activator.CreateInstance(t));
                }
            }

            return scenes;
        }

        private void SetAlert(bool success)
        {
            var alert = new Alert { MessageHtml = success ? SuccessMessage : ErrorMessage, Success = success };
            this.HttpContext.Session.SetString("alert-message", JsonConvert.SerializeObject(alert));
        }
    }
}
