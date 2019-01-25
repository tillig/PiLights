using System.Drawing;

namespace AddressableLed
{
    /// <summary>
    /// Represents a LED which can be controlled by the WS281x controller.
    /// </summary>
    public class LED
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LED"/> class.
        /// </summary>
        /// <param name="id">ID / index of the LED.</param>
        public LED(int id)
        {
            this.ID = id;
            this.Color = Color.Empty;
        }

        /// <summary>
        /// Gets or sets the color for the LED.
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// Gets the ID / index of the LED.
        /// </summary>
        public int ID { get; private set; }

        /// <summary>
        /// Gets the RGB value of the color.
        /// </summary>
        public int RGBValue { get => this.Color.ToArgb(); }
    }
}
