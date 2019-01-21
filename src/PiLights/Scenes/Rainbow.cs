using System;
using System.ComponentModel;
using System.Linq;
using HandlebarsDotNet;

namespace PiLights.Scenes
{
    [DisplayName(nameof(Rainbow))]
    public class Rainbow : Scene
    {
        private const string Template = @"rainbow 1
render{{# if chaseSpeed }}
do
  delay {{chaseSpeed}}
  rotate
  render
loop{{/if}}";

        private static readonly Func<object, string> CompiledTemplate = Handlebars.Compile(Template);

        [DisplayName("Chase Speed (0 for no chase)")]
        public int ChaseSpeed { get; set; }

        public override string GetSceneImplementation()
        {
            var data = new
            {
                chaseSpeed = this.ChaseSpeed,
            };

            var generated = CompiledTemplate(data);
            return generated;
        }
    }
}
