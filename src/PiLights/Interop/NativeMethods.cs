using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace PiLights.Interop
{
    // https://stackoverflow.com/questions/50141260/how-to-shutdown-computer-from-a-net-core-application-running-on-linux
    internal static class NativeMethods
    {
        public const int EFAULT = 14;

        public const int EINVAL = 22;

        public const int EPERM = 1;

        public const int LINUX_REBOOT_CMD_CAD_OFF = 0x00000000;

        public const int LINUX_REBOOT_CMD_CAD_ON = unchecked((int)0x89ABCDEF);

        public const int LINUX_REBOOT_CMD_HALT = unchecked((int)0xCDEF0123);

        public const int LINUX_REBOOT_CMD_KEXEC = 0x45584543;

        public const int LINUX_REBOOT_CMD_POWER_OFF = 0x4321FEDC;

        public const int LINUX_REBOOT_CMD_RESTART = 0x01234567;

        public const int LINUX_REBOOT_CMD_RESTART2 = unchecked((int)0xA1B2C3D4);

        public const int LINUX_REBOOT_CMD_SW_SUSPEND = unchecked((int)0xD000FCE2);

        public const int LINUX_REBOOT_MAGIC1 = unchecked((int)0xFEE1DEAD);

        public const int LINUX_REBOOT_MAGIC2 = 672274793;

        public const int LINUX_REBOOT_MAGIC2A = 85072278;

        public const int LINUX_REBOOT_MAGIC2B = 369367448;

        public const int LINUX_REBOOT_MAGIC2C = 537993216;

        // May need to change this to "libc.so.6" or "libc.so.7"
        [DllImport("libc.so", SetLastError = true)]
        public static extern int reboot(int magic, int magic2, int cmd, IntPtr arg);
    }
}
