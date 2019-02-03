using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AddressableLed
{
    public class Channel
    {
        public Channel(int ledCount, StripType stripType, int gpioPin = 18, byte brightness = 255, bool invert = false)
        {
            this.GpioPin = gpioPin;
            this.Invert = invert;
            this.Brightness = brightness;
            this.StripType = stripType;

            var ledList = new List<Light>();
            for (var i = 0; i <= ledCount - 1; i++)
            {
                ledList.Add(new Light());
            }

            this.Lights = new ReadOnlyCollection<Light>(ledList);
        }

        /// <summary>
        /// Gets or sets the brightness of the LEDs.
        /// </summary>
        /// <value>
        /// A brightness value from 0 (darkest) to 255 (brightest).
        /// </value>
        public byte Brightness { get; set; }

        /// <summary>
        /// Gets the GPIO pin to which the current strip/channel is connected.
        /// </summary>
        public int GpioPin { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the signal to the channel should be inverted.
        /// </summary>
        /// <value>
        /// <see langword="true" /> to invert the signal (e.g., when using an NPN transistor level shift); <see langword="false" />
        /// for default behavior.
        /// </value>
        public bool Invert { get; private set; }

        /// <summary>
        /// Gets all lights on this channel.
        /// </summary>
        public ReadOnlyCollection<Light> Lights { get; private set; }

        /// <summary>
        /// Gets the type of light on this channel.
        /// </summary>
        /// <value>
        /// A <see cref="AddressableLed.StripType"/> that indicates the
        /// ordering of the colors associated with the lights on this channel.
        /// </value>
        public StripType StripType { get; private set; }
    }
}
