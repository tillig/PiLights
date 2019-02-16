using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace PiLights.Services
{
    public class ColorModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var modelName = bindingContext.ModelName;
            var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);
            if (valueProviderResult == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);
            var value = valueProviderResult.FirstValue;
            if (string.IsNullOrEmpty(value))
            {
                return Task.CompletedTask;
            }

            var converter = new ColorConverter();
            Color converted;
            try
            {
                converted = (Color)converter.ConvertFromString(value);
            }
            catch (ArgumentException)
            {
                bindingContext.ModelState.TryAddModelError(
                                        modelName,
                                        "Please enter a color.");
                return Task.CompletedTask;
            }
            catch (NotSupportedException)
            {
                bindingContext.ModelState.TryAddModelError(
                                        modelName,
                                        "Please enter a color.");
                return Task.CompletedTask;
            }

            var ledColor = converted.R.ToString("x2", CultureInfo.InvariantCulture) +
                converted.G.ToString("x2", CultureInfo.InvariantCulture) +
                converted.B.ToString("x2", CultureInfo.InvariantCulture);

            bindingContext.Result = ModelBindingResult.Success(ledColor);
            return Task.CompletedTask;
        }
    }
}
