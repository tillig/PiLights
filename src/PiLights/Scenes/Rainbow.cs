using System;
using System.ComponentModel;
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
  rotate 1, 1, {{direction}}
  render
loop{{/if}}";

        private static readonly Func<object, string> CompiledTemplate = Handlebars.Compile(Template);

        [DisplayName("Chase Speed (0 for no chase)")]
        [Max(1000)]
        public int ChaseSpeed { get; set; }

        [DisplayName("Reverse Direction")]
        public bool Reverse { get; set; }

        public override string GetSceneImplementation()
        {
            var data = new
            {
                chaseSpeed = this.ChaseSpeed,
                direction = this.Reverse ? 0 : 1,
            };

            var generated = CompiledTemplate(data);
            return generated;
        }
    }
}
