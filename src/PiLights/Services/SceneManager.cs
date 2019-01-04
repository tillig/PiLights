using System;
using System.Collections.Generic;
using System.Linq;
using PiLights.Scenes;

namespace PiLights.Services
{
    public class SceneManager
    {
        public SceneManager(IEnumerable<Scene> scenes)
        {
            this.Scenes = scenes;
        }

        public IEnumerable<Scene> Scenes { get; private set; }
    }
}
