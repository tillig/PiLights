using System;
using LightCommandParser;
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
            var scene = new SolidColor();
            scene.Color = color;
            var script = scene.GenerateScript();
            var result = LightCommandAnalyzer.AnalyzeScript(script);
            Assert.Equal(0, result.ErrorCount);
        }
    }
}
