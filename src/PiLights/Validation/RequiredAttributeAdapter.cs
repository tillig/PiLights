using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Localization;

namespace PiLights.Validation
{
    public class RequiredAttributeAdapter : AttributeAdapterBase<RequiredAttribute>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RequiredAttributeAdapter"/> class.
        /// </summary>
        /// <param name="attribute">The <see cref="RequiredAttribute"/>.</param>
        /// <param name="stringLocalizer">The <see cref="IStringLocalizer"/>.</param>
        public RequiredAttributeAdapter(RequiredAttribute attribute, IStringLocalizer stringLocalizer)
            : base(attribute, stringLocalizer)
        {
        }

        /// <inheritdoc />
        public override void AddValidation(ClientModelValidationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            MergeAttribute(context.Attributes, "required", "required");
        }

        /// <inheritdoc />
        public override string GetErrorMessage(ModelValidationContextBase validationContext)
        {
            if (validationContext == null)
            {
                throw new ArgumentNullException(nameof(validationContext));
            }

            return this.GetErrorMessage(validationContext.ModelMetadata, validationContext.ModelMetadata.GetDisplayName());
        }
    }
}
