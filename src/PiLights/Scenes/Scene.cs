using System;
using System.Globalization;
using System.Linq;
using PiLights.Configuration;

namespace PiLights.Scenes
{
    // TODO: Create scene "KITT" - back and forth chaser like KITT or Cylons.
    // TODO: Create scene "Bumps" - like theater chaser but with more gradient between individual lights. Maybe an option on Theater Chaser instead?
    public abstract class Scene
    {
        private const string Wrapper = @"setup 1,{0},{1},0,{2}
init
thread_start
{3}
thread_stop
";

        protected Scene(GlobalConfigurationSettings settings)
        {
            this.Settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public GlobalConfigurationSettings Settings { get; private set; }

        public virtual bool Execute()
        {
            var script = this.GenerateScript();
            var result = this.Settings.SendDataToSocket(script);
            if (result)
            {
                LastKnownScene.WriteLastKnownScene(script);
            }

            return result;
        }

        public string GenerateScript()
        {
            // Commands via TCP need to...
            // - send the init directive
            // - start/end a thread around the render part
            // - use semicolons to separate commands
            // - end with a semicolon
            // - disconnect when the command is done being sent
            //
            // ws2812svr gets super picky if you miss any of those things
            // and won't render anything. The end semicolon in particular
            // will get you.
            var combined = string.Format(
                CultureInfo.InvariantCulture,
                Wrapper,
                this.Settings.LedCount,
                (int)this.Settings.LedType,
                this.Settings.GlobalBrightness,
                this.GetSceneImplementation());

            var trimmed = string.Join(';', combined
                .Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrEmpty(x))) + ";\n";

            return trimmed;
        }

        public abstract string GetSceneImplementation();
    }
}
