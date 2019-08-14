using System;
using System.Runtime.InteropServices;

namespace AddressableLed.Interop
{
    internal static class NativeMethods
    {
        [DllImport("ws2811.so")]
        public static extern void ws2811_fini(IntPtr ws2811);

        [DllImport("ws2811.so")]
        public static extern IntPtr ws2811_get_return_t_str(int state);

        [DllImport("ws2811.so")]
        public static extern ws2811_return_t ws2811_init(IntPtr ws2811);

        [DllImport("ws2811.so")]
        public static extern ws2811_return_t ws2811_render(IntPtr ws2811);

        [DllImport("ws2811.so")]
        public static extern ws2811_return_t ws2811_wait(IntPtr ws2811);
    }
}
