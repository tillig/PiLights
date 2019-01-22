using System;
using System.Globalization;

namespace PiLights.Scenes
{
    public abstract class Scene
    {
        // TODO: Ensure a max script length or figure out why large scripts hang.
        // Unclear if it's this issue where multiple init calls cause a lock up
        // https://github.com/tom-2015/rpi-ws2812-server/issues/20
        // but I do see that if the script is too long it hangs the whole RPi.
        // Don't know if it has to do with how complex the operations are
        // when the init hits. Running a theater chaser with 48
        // lights generates a 1062 char script. Running this a second
        // time in a row causes a lock up and the RPi stops responding on
        // the network. I thought it was a script length problem but that
        // doesn't seem to be consistent. I also tried streaming the script in
        // blocks of 1024 in case the ws2812svr had a buffer problem, no luck.
        private const string Wrapper = @"setup 1,{0},{1},0,{2}
init
thread_start
{3}
thread_stop
";

        public virtual bool Execute()
        {
            var script = this.GenerateScript();
            var result = ConfigurationManager.SendDataToSocket(script);
            if (result)
            {
                ConfigurationManager.WriteLastKnownScene(script);
            }

            return result;
        }

        public abstract string GetSceneImplementation();

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
            return string.Format(
                CultureInfo.InvariantCulture,
                Wrapper,
                ConfigurationManager.Configuration.LedCount,
                (int)ConfigurationManager.Configuration.LedType,
                ConfigurationManager.Configuration.GlobalBrightness,
                this.GetSceneImplementation()).Trim().Replace("\r\n", "\n", StringComparison.Ordinal).Replace('\n', ';') + ";";
        }
    }
}
