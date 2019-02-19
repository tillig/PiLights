using System;
using System.Linq;

namespace PiLights.Configuration
{
    public enum LedType
    {
        /// <summary>
        /// WS2811 RGB
        /// </summary>
        WS2811_STRIP_RGB,

        /// <summary>
        /// WS2811 RBG
        /// </summary>
        WS2811_STRIP_RBG,

        /// <summary>
        /// WS2811 GRB
        /// </summary>
        WS2811_STRIP_GRB,

        /// <summary>
        /// WS2811 GBR
        /// </summary>
        WS2811_STRIP_GBR,

        /// <summary>
        /// WS2811 BRG
        /// </summary>
        WS2811_STRIP_BRG,

        /// <summary>
        /// WS2811 BGR
        /// </summary>
        WS2811_STRIP_BGR,

        /// <summary>
        /// SK6812 RGBW
        /// </summary>
        SK6812_STRIP_RGBW,

        /// <summary>
        /// SK6812 RBGW
        /// </summary>
        SK6812_STRIP_RBGW,

        /// <summary>
        /// SK6812 GRBW
        /// </summary>
        SK6812_STRIP_GRBW,

        /// <summary>
        /// SK6812 GBRW
        /// </summary>
        SK6812_STRIP_GBRW,

        /// <summary>
        /// SK6812 BRGW
        /// </summary>
        SK6812_STRIP_BRGW,

        /// <summary>
        /// SK6812 BGRW
        /// </summary>
        SK6812_STRIP_BGRW,
    }
}
