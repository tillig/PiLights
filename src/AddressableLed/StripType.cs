using System;
using System.Collections.Generic;
using System.Text;

namespace AddressableLed
{
    /// <summary>
    /// The type of the LED strip defines the ordering of the colors (e. g. RGB, GRB, ...).
    /// Maybe the RGBValue property of the LED class needs to be changed if there are other strip types.
    /// </summary>
    // TODO: Add the rest of the strip types.
    public enum StripType
    {
        /// <summary>
        /// Unknown / unset.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Predefined GRB type.
        /// </summary>
        WS2812_STRIP = 0x00081000,
    }
}
