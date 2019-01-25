using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using AddressableLed.Interop;

namespace AddressableLed
{
    /// <summary>
    /// Wrapper class to control WS281x LEDs.
    /// </summary>
    public class WS281x : IDisposable
    {
        private bool _isDisposingAllowed;

        private ws2811_t _ws2811;

        private GCHandle _ws2811Handle;

        private bool disposedValue = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="WS281x"/> class.
        /// </summary>
        /// <param name="settings">Settings used for initialization.</param>
        public WS281x(Settings settings)
        {
            this._ws2811 = default(ws2811_t);

            // Pin the object in memory. Otherwise GC will probably move the object to another memory location.
            // This would cause errors because the native library has a pointer on the memory location of the object.
            this._ws2811Handle = GCHandle.Alloc(this._ws2811, GCHandleType.Pinned);

            this._ws2811.dmanum = settings.DMAChannel;
            this._ws2811.freq = settings.Frequency;
            this._ws2811.channel = new ws2811_channel_t[NativeMethods.RPI_PWM_CHANNELS];

            for (var i = 0; i <= this._ws2811.channel.Length - 1; i++)
            {
                if (settings.Channels[i] != null)
                {
                    this.InitChannel(i, settings.Channels[i]);
                }
            }

            this.Settings = settings;

            var initResult = NativeMethods.ws2811_init(ref this._ws2811);
            if (initResult != ws2811_return_t.WS2811_SUCCESS)
            {
                var returnMessage = GetMessageForStatusCode(initResult);
                throw new Exception(string.Format(CultureInfo.InvariantCulture, "Error while initializing.{0}Error code: {1}{0}Message: {2}", Environment.NewLine, initResult.ToString(), returnMessage));
            }

            // Disposing is only allowed if the init was successful.
            // Otherwise the native cleanup function throws an error.
            this._isDisposingAllowed = true;
        }

        ~WS281x()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            this.Dispose(false);
        }

        /// <summary>
        /// Gets the settings which are used to initialize the component.
        /// </summary>
        public Settings Settings { get; private set; }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Renders the content of the channels.
        /// </summary>
        public void Render()
        {
            for (var i = 0; i <= this.Settings.Channels.Length - 1; i++)
            {
                if (this.Settings.Channels[i] != null)
                {
                    var ledColor = this.Settings.Channels[i].LEDs.Select(x => x.RGBValue).ToArray();
                    Marshal.Copy(ledColor, 0, this._ws2811.channel[i].leds, ledColor.Count());
                }
            }

            var result = NativeMethods.ws2811_render(ref this._ws2811);
            if (result != ws2811_return_t.WS2811_SUCCESS)
            {
                var returnMessage = GetMessageForStatusCode(result);
                throw new Exception(string.Format(CultureInfo.InvariantCulture, "Error while rendering.{0}Error code: {1}{0}Message: {2}", Environment.NewLine, result.ToString(), returnMessage));
            }
        }

        /// <summary>
        /// Sets the color of a given LED.
        /// </summary>
        /// <param name="channelIndex">Channel which controls the LED.</param>
        /// <param name="ledID">ID/Index of the LED.</param>
        /// <param name="color">New color.</param>
        public void SetLEDColor(int channelIndex, int ledID, Color color)
        {
            this.Settings.Channels[channelIndex].LEDs[ledID].Color = color;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.
                if (this._isDisposingAllowed)
                {
                    NativeMethods.ws2811_fini(ref this._ws2811);
                    this._ws2811Handle.Free();

                    this._isDisposingAllowed = false;
                }

                this.disposedValue = true;
            }
        }

        /// <summary>
        /// Returns the error message for the given status code.
        /// </summary>
        /// <param name="statusCode">Status code to resolve.</param>
        private static string GetMessageForStatusCode(ws2811_return_t statusCode)
        {
            var strPointer = NativeMethods.ws2811_get_return_t_str((int)statusCode);
            return Marshal.PtrToStringAuto(strPointer);
        }

        /// <summary>
        /// Initialize the channel properties.
        /// </summary>
        /// <param name="channelIndex">Index of the channel to initialize.</param>
        /// <param name="channelSettings">Settings for the channel.</param>
        private void InitChannel(int channelIndex, Channel channelSettings)
        {
            this._ws2811.channel[channelIndex].count = channelSettings.LEDs.Count;
            this._ws2811.channel[channelIndex].gpionum = channelSettings.GPIOPin;
            this._ws2811.channel[channelIndex].brightness = channelSettings.Brightness;
            this._ws2811.channel[channelIndex].invert = Convert.ToInt32(channelSettings.Invert);

            if (channelSettings.StripType != StripType.Unknown)
            {
                // Strip type is set by the native assembly if not explicitly set.
                // This type defines the ordering of the colors e. g. RGB or GRB, ...
                this._ws2811.channel[channelIndex].strip_type = (int)channelSettings.StripType;
            }
        }
    }
}
