using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace AddressableLed.Interop
{
    [SuppressMessage("SA1300", "SA1300", Justification = "Native methods have different naming conventions.")]
    [SuppressMessage("SA1307", "SA1307", Justification = "Native methods have different naming conventions.")]
    [SuppressMessage("SA1310", "SA1310", Justification = "Native methods have different naming conventions.")]
    [SuppressMessage("SA1401", "SA1401", Justification = "Using a class with public fields instead of a struct to allow memory pinning, but keeping the data simple so fields instead of props.")]
    [StructLayout(LayoutKind.Sequential)]
    internal class ws2811_t
    {
        public long render_wait_time;
        public IntPtr device;
        public IntPtr rpi_hw;
        public uint freq;
        public int dmanum;
        public ws2811_channel_t channel_1;
        public ws2811_channel_t channel_2;
    }
}
