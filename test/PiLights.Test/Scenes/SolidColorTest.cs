using System;
using System.Drawing;
using LightCommandLanguage;
using PiLights.Configuration;
using PiLights.Scenes;
using Xunit;

namespace PiLights.Test.Scenes
{
    public class SolidColorTest
    {
        [Theory]
        [InlineData("#ff0000")]
        [InlineData("#00ff00")]
        public void ValidScene(string color)
        {
            var scene = new SolidColor(new LedSettings());
            scene.Color = (Color)new ColorConverter().ConvertFromString(color);
            var script = scene.GenerateScript();
            var result = LightCommandAnalyzer.AnalyzeScript(script);
            Assert.Equal(0, result.ErrorCount);
        }
    }
}
