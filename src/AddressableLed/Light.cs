using System.Drawing;

namespace AddressableLed
{
    /// <summary>
    /// Represents a single light in the system.
    /// </summary>
    public class Light
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Light"/> class.
        /// </summary>
        public Light()
        {
            this.Color = Color.Empty;
        }

        /// <summary>
        /// Gets or sets the color for the light.
        /// </summary>
        /// <value>
        /// A <see cref="System.Drawing.Color"/> that the light
        /// should display.
        /// </value>
        public Color Color { get; set; }

        /// <summary>
        /// Gets the RGB value of the light's color.
        /// </summary>
        /// <value>
        /// An <see cref="int"/> with the <see cref="Color"/>
        /// as a 32-bit ARGB value.
        /// </value>
        public int RgbValue => this.Color.ToArgb();
    }
}
