using System;
using System.Drawing;
using System.Linq;
using PiLights.Scenes;
using Xunit;

namespace PiLights.Test.Scenes
{
    public class ColorExtensionsTest
    {
        [Fact]
        public void Interpolate_GeneratesGradient()
        {
            var start = Color.FromArgb(94, 79, 162);
            var end = Color.FromArgb(247, 148, 89);
            var gradient = start.Interpolate(end, 5).ToArray();
            Assert.Equal(Color.FromArgb(94, 79, 162), gradient[0]);
            Assert.Equal(Color.FromArgb(132, 96, 144), gradient[1]);
            Assert.Equal(Color.FromArgb(171, 114, 126), gradient[2]);
            Assert.Equal(Color.FromArgb(209, 131, 107), gradient[3]);
            Assert.Equal(Color.FromArgb(247, 148, 89), gradient[4]);
        }
    }
}
