using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Localization;

namespace PiLights.Validation
{
    public class StringLengthAttributeAdapter : AttributeAdapterBase<StringLengthAttribute>
    {
        private readonly string _max;

        private readonly string _min;

        public StringLengthAttributeAdapter(StringLengthAttribute attribute, IStringLocalizer stringLocalizer)
            : base(attribute, stringLocalizer)
        {
            this._max = this.Attribute.MaximumLength.ToString(CultureInfo.InvariantCulture);
            this._min = this.Attribute.MinimumLength.ToString(CultureInfo.InvariantCulture);
        }

        /// <inheritdoc />
        public override void AddValidation(ClientModelValidationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (this.Attribute.MaximumLength != int.MaxValue)
            {
                MergeAttribute(context.Attributes, "maxlength", this._max);
            }

            if (this.Attribute.MinimumLength != 0)
            {
                MergeAttribute(context.Attributes, "minlength", this._min);
            }
        }

        /// <inheritdoc />
        public override string GetErrorMessage(ModelValidationContextBase validationContext)
        {
            if (validationContext == null)
            {
                throw new ArgumentNullException(nameof(validationContext));
            }

            return this.GetErrorMessage(
                validationContext.ModelMetadata,
                validationContext.ModelMetadata.GetDisplayName(),
                this.Attribute.MaximumLength,
                this.Attribute.MinimumLength);
        }
    }
}
