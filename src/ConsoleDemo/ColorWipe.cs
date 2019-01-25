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

        private readonly WS281x _controller;

        public ColorWipe(WS281x controller, Color color)
        {
            this._controller = controller;
            this._color = color;
        }

        public void Execute(CancellationToken token)
        {
            for (var i = 0; i <= this._controller.Settings.Channels[0].LEDs.Count - 1 && !token.IsCancellationRequested; i++)
            {
                this._controller.SetLEDColor(0, i, this._color);
                this._controller.Render();
                Thread.Sleep(1000 / 15);
            }
        }
    }
}
