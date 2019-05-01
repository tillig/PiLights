using System;
using System.ComponentModel;
using PiLights.Configuration;
using PiLights.Scenes;
using Xunit;

namespace PiLights.Test.Scenes
{
    public class SceneExtensionsTest
    {
        [Fact]
        public void GetDisplayName_Attribute()
        {
            var s = new NamedScene(new LedSettings());
            Assert.Equal("Named", s.GetDisplayName());
        }

        [Fact]
        public void GetDisplayName_NoAttribute()
        {
            var s = new NoNameScene(new LedSettings());
            Assert.Equal(nameof(NoNameScene), s.GetDisplayName());
        }

        [DisplayName("Named")]
        private class NamedScene : Scene
        {
            public NamedScene(LedSettings settings)
                : base(settings)
            {
            }

            public override string GetSceneImplementation()
            {
                throw new NotImplementedException();
            }
        }

        private class NoNameScene : Scene
        {
            public NoNameScene(LedSettings settings)
                : base(settings)
            {
            }

            public override string GetSceneImplementation()
            {
                throw new NotImplementedException();
            }
        }
    }
}
