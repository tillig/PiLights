using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace PiLights.Services
{
    /// <summary>
    /// Default object display template forked from https://github.com/aspnet/AspNetCore/blob/c565386a3ed135560bc2e9017aa54a950b4e35dd/src/Mvc/Mvc.ViewFeatures/src/DefaultDisplayTemplates.cs
    /// to allow for Bootstrap 4 compatible display similar to the form display.
    /// </summary>
    public static class ObjectDisplayTemplate
    {
        public static IHtmlContent ObjectTemplate(IHtmlHelper htmlHelper)
        {
            var viewData = htmlHelper.ViewData;
            var templateInfo = viewData.TemplateInfo;
            var modelExplorer = viewData.ModelExplorer;

            if (modelExplorer.Model == null)
            {
                return new HtmlString(modelExplorer.Metadata.NullDisplayText);
            }

            if (templateInfo.TemplateDepth > 1)
            {
                var text = modelExplorer.GetSimpleDisplayText();
                if (modelExplorer.Metadata.HtmlEncode)
                {
                    text = htmlHelper.Encode(text);
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
                    readOnly: true,
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
                modelExplorer.Metadata.ShowForDisplay &&
                !modelExplorer.Metadata.IsComplexType &&
                !templateInfo.Visited(modelExplorer);
        }
    }
}
