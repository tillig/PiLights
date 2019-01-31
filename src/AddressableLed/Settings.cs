using System.Collections.Generic;
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
        /// <param name="channelSettings">Detailed settings for the LED channel.</param>
        /// <param name="frequency">Set frequency in Hz.</param>
        /// <param name="dmaChannel">Set DMA channel to use.</param>
        public Settings(Channel channelSettings, uint frequency = 800000, int dmaChannel = 10)
        {
            this.Frequency = frequency;
            this.DMAChannel = dmaChannel;
            this.ChannelSettings = channelSettings;
        }

        /// <summary>
        /// Gets the channel which holds the LEDs.
        /// </summary>
        public Channel ChannelSettings { get; private set; }

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
