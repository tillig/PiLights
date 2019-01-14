using System;
using System.ComponentModel;
using PiLights.Scenes;
using Xunit;

namespace PiLights.Test.Scenes
{
    public class SceneExtensionsTest
    {
        [Fact]
        public void GetDisplayName_Attribute()
        {
            var s = new NamedScene();
            Assert.Equal("Named", s.GetDisplayName());
        }

        [Fact]
        public void GetDisplayName_NoAttribute()
        {
            var s = new NoNameScene();
            Assert.Equal(nameof(NoNameScene), s.GetDisplayName());
        }

        private class NoNameScene : Scene
        {
            public override void Execute()
            {
                throw new NotImplementedException();
            }
        }

        [DisplayName("Named")]
        private class NamedScene : Scene
        {
            public override void Execute()
            {
                throw new NotImplementedException();
            }
        }
    }
}
