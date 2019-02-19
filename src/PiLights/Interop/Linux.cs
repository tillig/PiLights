using System;
using System.Linq;
using System.Runtime.InteropServices;
using static PiLights.Interop.NativeMethods;

namespace PiLights.Interop
{
    public static class Linux
    {
        /// <summary>
        /// Shuts down the Linux system.
        /// </summary>
        public static void Shutdown()
        {
            var ret = reboot(LINUX_REBOOT_MAGIC1, LINUX_REBOOT_MAGIC2, LINUX_REBOOT_CMD_POWER_OFF, IntPtr.Zero);

            // `reboot(LINUX_REBOOT_CMD_POWER_OFF)` never returns if it's successful, so if it returns 0 then that's weird, we should treat it as an error condition instead of success:
            if (ret == 0)
            {
                throw new InvalidOperationException("reboot(LINUX_REBOOT_CMD_POWER_OFF) returned 0.");
            }

            // ..otherwise we expect it to return -1 in the event of failure, so any other value is exceptional:
            if (ret != -1)
            {
                throw new InvalidOperationException("Unexpected reboot() return value: " + ret);
            }

            // At this point, ret == -1, which means check `errno`!
            // `errno` is accessed via Marshal.GetLastWin32Error(), even on non-Win32 platforms and especially even on Linux
            int errno = Marshal.GetLastWin32Error();
            switch (errno)
            {
                case EPERM:
                    throw new UnauthorizedAccessException("You do not have permission to call reboot()");

                case EINVAL:
                    throw new ArgumentException("Bad magic numbers (stray cosmic-ray?)");

                case EFAULT:
                default:
                    throw new InvalidOperationException($"Could not call reboot(): errno {errno}");
            }
        }
    }
}
