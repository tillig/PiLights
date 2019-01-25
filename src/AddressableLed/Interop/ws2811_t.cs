using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace AddressableLed.Interop
{
    [SuppressMessage("SA1300", "SA1300", Justification = "Native methods have different naming conventions.")]
    [SuppressMessage("SA1307", "SA1307", Justification = "Native methods have different naming conventions.")]
    [SuppressMessage("SA1310", "SA1310", Justification = "Native methods have different naming conventions.")]
    [StructLayout(LayoutKind.Sequential)]
    internal struct ws2811_t
    {
        public long render_wait_time;
        public IntPtr device;
        public IntPtr rpi_hw;
        public uint freq;
        public int dmanum;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = NativeMethods.RPI_PWM_CHANNELS)]
        public ws2811_channel_t[] channel;
    }
}
