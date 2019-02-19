using System;
using LightCommandLanguage;
using PiLights.Configuration;
using PiLights.Scenes;
using Xunit;

namespace PiLights.Test.Scenes
{
    public class SolidColorTest
    {
        [Theory]
        [InlineData("ffffff")]
        [InlineData("FFFFFF")]
        public void ValidScene(string color)
        {
            var scene = new SolidColor(new GlobalConfigurationSettings());
            scene.Color = color;
            var script = scene.GenerateScript();
            var result = LightCommandAnalyzer.AnalyzeScript(script);
            Assert.Equal(0, result.ErrorCount);
        }
    }
}
