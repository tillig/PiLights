using System;
using System.Drawing;

namespace AddressableLed
{
    public class StubLedController : ILedController
    {
        public StubLedController(Settings settings)
        {
            this.Settings = settings;
        }

        public Settings Settings { get; private set; }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Render()
        {
        }

        public void SetLightColor(int index, Color color)
        {
        }

        protected virtual void Dispose(bool disposing)
        {
        }
    }
}
