using System.Diagnostics.CodeAnalysis;
using AddressableLed.Interop;

namespace AddressableLed
{
    /// <summary>
    /// Settings used to initialize a WS281x addressable LED controller.
    /// </summary>
    public class Settings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Settings"/> class.
        /// </summary>
        /// <param name="frequency">Set frequency in Hz.</param>
        /// <param name="dmaChannel">Set DMA channel to use.</param>
        public Settings(uint frequency = 800000, int dmaChannel = 10)
        {
            this.Frequency = frequency;
            this.DMAChannel = dmaChannel;
            this.Channels = new Channel[NativeMethods.RPI_PWM_CHANNELS];
        }

        /// <summary>
        /// Gets the channels which holds the LEDs.
        /// </summary>
        // TODO: Refactor this to be a list or collection if it won't impact interop.
        [SuppressMessage("CA1819", "CA1819", Justification = "Need to refactor this if possible.")]
        public Channel[] Channels { get; private set; }

        /// <summary>
        /// Gets the DMA channel.
        /// </summary>
        public int DMAChannel { get; private set; }

        /// <summary>
        /// Gets the used frequency in Hz.
        /// </summary>
        public uint Frequency { get; private set; }
    }
}
