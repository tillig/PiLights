using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PiLights.Models
{
    /// <summary>
    /// Details about the contents of an individual alert that should be displayed
    /// to the user in a view.
    /// </summary>
    public class Alert
    {
        /// <summary>
        /// Gets or sets the HTML message content.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> with the full content for the alert.
        /// The content may contain HTML for additional formatting within the alert.
        /// </value>
        public string MessageHtml { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the alert is a success message.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the alert is a success message; otherwise,
        /// <see langword="false"/>.
        /// </value>
        public bool Success { get; set; }
    }
}
