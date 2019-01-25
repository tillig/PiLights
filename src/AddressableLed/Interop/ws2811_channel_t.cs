using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace AddressableLed.Interop
{
    [StructLayout(LayoutKind.Sequential)]
    [SuppressMessage("SA1300", "SA1300", Justification = "Native methods have different naming conventions.")]
    [SuppressMessage("SA1307", "SA1307", Justification = "Native methods have different naming conventions.")]
    [SuppressMessage("SA1310", "SA1310", Justification = "Native methods have different naming conventions.")]
    internal struct ws2811_channel_t
    {
        public int gpionum;
        public int invert;
        public int count;
        public int strip_type;
        public IntPtr leds;
        public byte brightness;
        public byte wshift;
        public byte rshift;
        public byte gshift;
        public byte bshift;
        public IntPtr gamma;
    }
}
