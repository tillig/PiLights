using System.Diagnostics.CodeAnalysis;

namespace AddressableLed.Interop
{
    [SuppressMessage("SA1300", "SA1300", Justification = "Native methods have different naming conventions.")]
    internal enum ws2811_return_t
    {
        /// <summary>
        /// Success.
        /// </summary>
        WS2811_SUCCESS = 0,

        /// <summary>
        /// Generic failure.
        /// </summary>
        WS2811_ERROR_GENERIC = -1,

        /// <summary>
        /// Out of memory.
        /// </summary>
        WS2811_ERROR_OUT_OF_MEMORY = -2,

        /// <summary>
        /// Hardware revision is not supported.
        /// </summary>
        WS2811_ERROR_HW_NOT_SUPPORTED = -3,

        /// <summary>
        /// Memory lock failed.
        /// </summary>
        WS2811_ERROR_MEM_LOCK = -4,

        /// <summary>
        /// mmap() failed.
        /// </summary>
        WS2811_ERROR_MMAP = -5,

        /// <summary>
        /// Unable to map registers into userspace.
        /// </summary>
        WS2811_ERROR_MAP_REGISTERS = -6,

        /// <summary>
        /// Unable to initialize GPIO.
        /// </summary>
        WS2811_ERROR_GPIO_INIT = -7,

        /// <summary>
        /// Unable to initialize PWM.
        /// </summary>
        WS2811_ERROR_PWM_SETUP = -8,

        /// <summary>
        /// Failed to create mailbox device.
        /// </summary>
        WS2811_ERROR_MAILBOX_DEVICE = -9,

        /// <summary>
        /// DMA error.
        /// </summary>
        WS2811_ERROR_DMA = -10,

        /// <summary>
        /// Selected GPIO not possible.
        /// </summary>
        WS2811_ERROR_ILLEGAL_GPIO = -11,

        /// <summary>
        /// Unable to initialize PCM.
        /// </summary>
        WS2811_ERROR_PCM_SETUP = -12,

        /// <summary>
        /// Unable to initialize SPI.
        /// </summary>
        WS2811_ERROR_SPI_SETUP = -13,

        /// <summary>
        /// SPI transfer error.
        /// </summary>
        WS2811_ERROR_SPI_TRANSFER = -14,
    }
}