using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
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
        public IActionResult StartScene(string sceneName)
        {
            // TODO: Separate scene properties from scene execution - use property object as model.
            // TODO: Update to use model binding instead of manual parsing.
            var scene = this.SceneManager.Scenes.First(x => x.GetType().FullName == sceneName);
            var sceneProps = scene.GetSceneProperties();
            var properties = new Dictionary<string, string>();
            foreach (var prop in sceneProps)
            {
                this.Request.Form.TryGetValue(prop.Name, out var propValue);
                object value = null;
                var typeConverter = prop.GetCustomAttribute<TypeConverterAttribute>();
                if (typeConverter == null)
                {
                    value = Convert.ChangeType(propValue[0], prop.PropertyType, CultureInfo.InvariantCulture);
                }
                else
                {
                    var converterInstance = (TypeConverter)Activator.CreateInstance(Type.GetType(typeConverter.ConverterTypeName));
                    value = converterInstance.ConvertTo(propValue[0], prop.PropertyType);
                }

                prop.SetValue(scene, value, null);
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
