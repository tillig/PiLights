using System;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace PiLights.Services
{
    /// <summary>
    /// Default object editor template forked from https://github.com/aspnet/AspNetCore/blob/c565386a3ed135560bc2e9017aa54a950b4e35dd/src/Mvc/Mvc.ViewFeatures/src/DefaultEditorTemplates.cs
    /// to allow for Bootstrap 4 compatible editors.
    /// </summary>
    public static class ObjectEditorTemplate
    {
        public static IHtmlContent ObjectTemplate(IHtmlHelper htmlHelper)
        {
            var viewData = htmlHelper.ViewData;
            var templateInfo = viewData.TemplateInfo;
            var modelExplorer = viewData.ModelExplorer;

            if (templateInfo.TemplateDepth > 1)
            {
                if (modelExplorer.Model == null)
                {
                    return new HtmlString(modelExplorer.Metadata.NullDisplayText);
                }

                var text = modelExplorer.GetSimpleDisplayText();
                if (modelExplorer.Metadata.HtmlEncode)
                {
                    return new StringHtmlContent(text);
                }

                return new HtmlString(text);
            }

            var serviceProvider = htmlHelper.ViewContext.HttpContext.RequestServices;
            var viewEngine = serviceProvider.GetRequiredService<ICompositeViewEngine>();
            var viewBufferScope = serviceProvider.GetRequiredService<IViewBufferScope>();

            var content = new HtmlContentBuilder(modelExplorer.Metadata.Properties.Count);
            foreach (var propertyExplorer in modelExplorer.Properties)
            {
                var propertyMetadata = propertyExplorer.Metadata;
                if (!ShouldShow(propertyExplorer, templateInfo))
                {
                    continue;
                }

                var templateBuilder = new TemplateBuilder(
                    viewEngine,
                    viewBufferScope,
                    htmlHelper.ViewContext,
                    htmlHelper.ViewData,
                    propertyExplorer,
                    htmlFieldName: propertyMetadata.PropertyName,
                    templateName: null,
                    readOnly: false,
                    additionalViewData: null);

                var templateBuilderResult = templateBuilder.Build();
                if (!propertyMetadata.HideSurroundingHtml)
                {
                    var groupTag = new TagBuilder("div");
                    var isBooleanProperty = propertyMetadata.ModelType == typeof(bool) || propertyMetadata.ModelType == typeof(bool?);
                    if (!isBooleanProperty)
                    {
                        groupTag.AddCssClass("form-group");
                        var label = htmlHelper.Label(propertyMetadata.PropertyName, labelText: null, htmlAttributes: null);
                        using (var writer = new HasContentTextWriter())
                        {
                            label.WriteTo(writer, PassThroughHtmlEncoder.Default);
                            if (writer.HasContent)
                            {
                                groupTag.InnerHtml.AppendHtml(label);
                            }
                        }
                    }

                    groupTag.InnerHtml.AppendHtml(templateBuilderResult);

                    if (isBooleanProperty)
                    {
                        groupTag.AddCssClass("form-check");
                        var label = htmlHelper.Label(propertyMetadata.PropertyName, labelText: null, htmlAttributes: new { @class = "form-check-label" });
                        using (var writer = new HasContentTextWriter())
                        {
                            label.WriteTo(writer, PassThroughHtmlEncoder.Default);
                            if (writer.HasContent)
                            {
                                groupTag.InnerHtml.AppendHtml(label);
                            }
                        }
                    }

                    content.AppendLine(groupTag);
                }
                else
                {
                    content.AppendHtml(templateBuilderResult);
                }
            }

            return content;
        }

        private static bool ShouldShow(ModelExplorer modelExplorer, TemplateInfo templateInfo)
        {
            return
                modelExplorer.Metadata.ShowForEdit &&
                !modelExplorer.Metadata.IsComplexType &&
                !templateInfo.Visited(modelExplorer);
        }

        private class HasContentTextWriter : TextWriter
        {
            public override Encoding Encoding => Null.Encoding;

            public bool HasContent { get; private set; }

            public override void Write(char value)
            {
                this.HasContent = true;
            }

            public override void Write(string value)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.HasContent = true;
                }
            }

            public override void Write(char[] buffer, int index, int count)
            {
                if (count > 0)
                {
                    this.HasContent = true;
                }
            }
        }

        // An HTML encoder which passes through all input data. Does no encoding.
        // Copied from Microsoft.AspNetCore.Razor.TagHelpers.NullHtmlEncoder.
        private class PassThroughHtmlEncoder : HtmlEncoder
        {
            private PassThroughHtmlEncoder()
            {
            }

            public static new PassThroughHtmlEncoder Default { get; } = new PassThroughHtmlEncoder();

            public override int MaxOutputCharactersPerInputCharacter => 1;

            public override string Encode(string value)
            {
                return value;
            }

            public override void Encode(TextWriter output, char[] value, int startIndex, int characterCount)
            {
                if (output == null)
                {
                    throw new ArgumentNullException(nameof(output));
                }

                if (characterCount == 0)
                {
                    return;
                }

                output.Write(value, startIndex, characterCount);
            }

            public override void Encode(TextWriter output, string value, int startIndex, int characterCount)
            {
                if (output == null)
                {
                    throw new ArgumentNullException(nameof(output));
                }

                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                if (characterCount == 0)
                {
                    return;
                }

                output.Write(value.Substring(startIndex, characterCount));
            }

            public override unsafe int FindFirstCharacterToEncode(char* text, int textLength)
            {
                return -1;
            }

            public override unsafe bool TryEncodeUnicodeScalar(
                int unicodeScalar,
                char* buffer,
                int bufferLength,
                out int numberOfCharactersWritten)
            {
                numberOfCharactersWritten = 0;

                return false;
            }

            public override bool WillEncode(int unicodeScalar)
            {
                return false;
            }
        }
    }
}
