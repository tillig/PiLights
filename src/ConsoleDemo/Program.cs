using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using AddressableLed;

namespace ConsoleDemo
{
    /// <summary>
    /// Simple console app to test manual integration with the WS281x library.
    /// </summary>
    public class Program
    {
        private static void Main()
        {
            // AddressableLed is a fork/combo of...
            // https://github.com/rpi-ws281x/rpi-ws281x-csharp
            // https://github.com/chris579/rpi_ws281x.Net
            var settings = new Settings(new Channel(540, StripType.WS2811_STRIP_GRB, brightness: 64));

            // using (ILedController controller = new StubLedController(settings))
            using (ILedController controller = new Ws281xController(settings))
            {
                var animations = new IAnimation[]
                {
                    new ColorWipe(controller, Color.Red),
                    new ColorWipe(controller, Color.Green),
                    new ColorWipe(controller, Color.Blue),
                    new RainbowColorAnimation(controller),
                };

                // TODO: Segmentation fault happens if you try to do this fancy demo.
                // Working through it here https://github.com/rpi-ws281x/rpi-ws281x-powershell/issues/2
                // The Console.WriteLine calls in WS281x looked like they were saving me
                // but that appears to only work for the simple once-through run of the
                // color wipe. When I try to do multiple things, fail.
                /*
                Console.WriteLine("Beginning test - press any key to exit.");
                var tokenSource = new CancellationTokenSource();
                var task = Task.Run(
                    () =>
                    {
                        while (!tokenSource.Token.IsCancellationRequested)
                        {
                            foreach (var animation in animations)
                            {
                                Console.WriteLine(animation.ToString());
                                animation.Execute(tokenSource.Token);
                                Thread.Sleep(1000);
                            }
                        }
                    },
                    tokenSource.Token);
                Console.ReadKey();
                Console.WriteLine("Finishing up.");
                tokenSource.Cancel();
                task.Wait();
                */

                // Simple for now - just wipe red, then clear the lights.
                Console.WriteLine(animations[0].ToString());
                animations[0].Execute(CancellationToken.None);

                var off = new ColorWipe(controller, Color.Black);
                off.Execute(CancellationToken.None);
                Console.WriteLine("Done.");
            }
        }
    }
}
