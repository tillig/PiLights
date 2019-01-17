using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Newtonsoft.Json;
using PiLights.Models;

namespace PiLights
{
    /// <summary>
    /// AspNetCore tag helper for alerts.
    /// </summary>
    [HtmlTargetElement("alerts")]
    public class AlertsTagHelper : TagHelper
    {
        /// <summary>
        /// The class names used for the show and hide animations used by the alert.
        /// </summary>
        public const string AnimationNames = "fade show";

        /// <summary>
        /// The liga name used for the "close" material design icon used for the alert close button.
        /// </summary>
        public const string CloseIconName = "close";

        /// <summary>
        /// The session key for storing alert messages.
        /// </summary>
        public const string AlertMessage = "alert-message";

        /// <summary>
        /// Gets or sets the view context.
        /// </summary>
        /// <value>
        /// A <see cref="ViewContext"/> containing the view context.
        /// </value>
        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        /// <summary>
        /// Processes the tag helper to output html.
        /// </summary>
        /// <param name="context">The <see cref="TagHelperContext"/>.</param>
        /// <param name="output">The <see cref="TagHelperOutput"/>.</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "span";
            var message = this.ViewContext.HttpContext.Session.GetString(AlertMessage);
            if (message != null)
            {
                var alert = JsonConvert.DeserializeObject<Alert>(message);
                output.Content.AppendHtml(GetTagBuilder(alert));
                this.ViewContext.HttpContext.Session.Remove(AlertMessage);
            }
        }

        /// <summary>
        /// Returns a <see cref="TagBuilder"/> containing an <see cref="Alert"/>.
        /// </summary>
        /// <param name="alert">
        /// The <see cref="Alert"/> to convert to a <see cref="TagBuilder"/>.
        /// </param>
        /// <returns>
        /// A <see cref="TagBuilder"/> with CSS classes
        /// applied to the alert values so it can be styled in UI.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="alert" /> is <see langword="null" />.
        /// </exception>
        /// <remarks>
        /// <para>
        /// The HTML generated for an <see cref="Alert"/>
        /// is compatible with the Twitter Bootstrap "Alert" component. You can
        /// read details on the CSS and HTML for Twitter Bootstrap alerts
        /// here: <c>https://getbootstrap.com/docs/4.0/components/alerts/</c>.
        /// </para>
        /// <para>
        /// In addition, a new element has been introduced to the HTML to allow
        /// insertion of a small icon or image in the alert, like a "red warning icon" or
        /// "green checkmark" for error or success alerts, respectively.
        /// </para>
        /// <para>
        /// Take note that the order for the elements inside of the alert is important. The
        /// alert component uses a <c>flex</c> display, so the order must be such that the
        /// alert type icon markup comes first, followed by the message related markup, and concluded
        /// with the close icon markup.</para>
        /// </remarks>
        private static TagBuilder GetTagBuilder(Alert alert)
        {
            if (alert == null)
            {
                throw new ArgumentNullException(nameof(alert));
            }

            var closeClass = " alert-dismissable";
            var alertName = alert.Success ? "alert-success" : "alert-danger";
            var div = new TagBuilder("div");
            div.AddCssClass($"alert {alertName}{closeClass} {AnimationNames}");
            div.Attributes.Add("role", "alert");

            var icon = new TagBuilder("i");
            icon.AddCssClass("material-icons");
            icon.InnerHtml.AppendHtml(alert.Success ? "check_circle" : "error");
            div.InnerHtml.AppendHtml(icon);

            var message = new TagBuilder("span");
            message.AddCssClass("alert-message");
            message.InnerHtml.AppendHtml(alert.MessageHtml);
            div.InnerHtml.AppendHtml(message);

            var close = new TagBuilder("button");
            close.AddCssClass("close");
            close.Attributes.Add("data-dismiss", "alert");
            close.Attributes.Add("data-test-element", "alert-close-button");

            var closeIcon = new TagBuilder("i");
            closeIcon.AddCssClass("material-icons");
            closeIcon.InnerHtml.AppendHtml(CloseIconName);
            close.InnerHtml.AppendHtml(closeIcon);
            div.InnerHtml.AppendHtml(close);

            return div;
        }
    }
}
