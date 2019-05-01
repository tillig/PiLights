using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using HandlebarsDotNet;
using PiLights.Configuration;

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

        public Rainbow(LedSettings settings)
            : base(settings)
        {
            this.ChaseSpeed = 20;
            this.Reverse = false;
        }

        [DisplayName("Chase Speed (0 for no chase)")]
        [Range(0, 1000)]
        public int ChaseSpeed { get; set; }

        [DisplayName("Reverse Direction")]
        public bool? Reverse { get; set; }

        public override string GetSceneImplementation()
        {
            var data = new
            {
                chaseSpeed = this.ChaseSpeed,
                direction = this.Reverse.HasValue && this.Reverse.Value ? 0 : 1,
            };

            var generated = CompiledTemplate(data);
            return generated;
        }
    }
}
