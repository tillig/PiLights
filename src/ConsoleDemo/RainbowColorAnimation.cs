using System;
using System.Drawing;
using System.Linq;
using System.Threading;
using AddressableLed;

namespace ConsoleDemo
{
    public class RainbowColorAnimation : IAnimation
    {
        private static readonly Color[] _animationColors = new Color[]
            {
                Color.FromArgb(0x201000),
                Color.FromArgb(0x202000),
                Color.Green,
                Color.FromArgb(0x002020),
                Color.Blue,
                Color.FromArgb(0x100010),
                Color.FromArgb(0x200010),
            };

        private readonly ILedController _controller;

        public RainbowColorAnimation(ILedController controller)
        {
            this._controller = controller;
        }

        public void Execute(CancellationToken token)
        {
            var colorOffset = 0;
            for (var i = 0; i <= this._controller.Settings.Channel.Lights.Count - 1 && !token.IsCancellationRequested; i++)
            {
                var colorIndex = (i + colorOffset) % _animationColors.Length;
                this._controller.SetLightColor(i, _animationColors[colorIndex]);
                this._controller.Render();

                if (colorOffset == int.MaxValue)
                {
                    colorOffset = 0;
                }

                colorOffset++;
                Thread.Sleep(10);
            }
        }

        public override string ToString()
        {
            return "Rainbow";
        }
    }
}
