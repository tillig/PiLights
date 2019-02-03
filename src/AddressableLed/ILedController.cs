using System;
using System.Drawing;

namespace AddressableLed
{
    public interface ILedController : IDisposable
    {
        Settings Settings { get; }

        void Render();

        void SetLightColor(int index, Color color);
    }
}
