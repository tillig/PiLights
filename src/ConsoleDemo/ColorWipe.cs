using System;
using System.Drawing;
using System.Linq;
using System.Threading;
using AddressableLed;

namespace ConsoleDemo
{
    public class ColorWipe : IAnimation
    {
        private readonly Color _color;

        private readonly ILedController _controller;

        public ColorWipe(ILedController controller, Color color)
        {
            this._controller = controller;
            this._color = color;
        }

        public void Execute(CancellationToken token)
        {
            for (var i = 0; i <= this._controller.Settings.ChannelSettings.LEDs.Count - 1 && !token.IsCancellationRequested; i++)
            {
                this._controller.SetLEDColor(i, this._color);
                this._controller.Render();
                Thread.Sleep(10);
            }
        }

        public override string ToString()
        {
            return $"Color wipe: {this._color.ToString()}";
        }
    }
}
