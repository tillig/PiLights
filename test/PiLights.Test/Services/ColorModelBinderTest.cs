using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Primitives;
using Moq;
using PiLights.Services;
using Xunit;

namespace PiLights.Test.Services
{
    public class ColorModelBinderTest
    {
        [Fact]
        public async Task BindModelAsync_NoValue()
        {
            var binder = new ColorModelBinder();
            var bindingContext = GetBindingContext(new Dictionary<string, StringValues>());
            await binder.BindModelAsync(bindingContext);
            Assert.False(bindingContext.Result.IsModelSet);
            Assert.Null(bindingContext.Result.Model);
        }

        [Fact]
        public async Task BindModelAsync_EmptyValue()
        {
            var binder = new ColorModelBinder();
            var formPostValues = new Dictionary<string, StringValues>()
            {
                { "Color", string.Empty },
            };
            var bindingContext = GetBindingContext(formPostValues);
            await binder.BindModelAsync(bindingContext);
            Assert.False(bindingContext.Result.IsModelSet);
            Assert.Null(bindingContext.Result.Model);
        }

        [Theory]
        [InlineData("x")]
        [InlineData("#no")]
        [InlineData("123jkl")]
        public async Task BindModelAsync_ColorNotParsed(string value)
        {
            var binder = new ColorModelBinder();
            var formPostValues = new Dictionary<string, StringValues>()
            {
                { "Color", value },
            };
            var bindingContext = GetBindingContext(formPostValues);
            await binder.BindModelAsync(bindingContext);
            Assert.False(bindingContext.Result.IsModelSet);
            Assert.Null(bindingContext.Result.Model);
            Assert.Equal(1, bindingContext.ModelState.ErrorCount);
        }

        [Theory]
        [InlineData("000000", "000000")]
        [InlineData("#000000", "000000")]
        public async Task BindModelAsync_ColorParsed(string input, string output)
        {
            var binder = new ColorModelBinder();
            var formPostValues = new Dictionary<string, StringValues>()
            {
                { "Color", input },
            };
            var bindingContext = GetBindingContext(formPostValues);
            await binder.BindModelAsync(bindingContext);
            Assert.True(bindingContext.Result.IsModelSet);
            Assert.Equal(output, bindingContext.Result.Model);
            Assert.Equal(0, bindingContext.ModelState.ErrorCount);
        }

        [Fact]
        public async Task BindModelAsync_NullContext()
        {
            var binder = new ColorModelBinder();
            await Assert.ThrowsAsync<ArgumentNullException>(() => binder.BindModelAsync(null));
        }

        private static DefaultModelBindingContext GetBindingContext(Dictionary<string, StringValues> formPostValues)
        {
            var formCollection = new FormCollection(formPostValues);
            var metadataProvider = new EmptyModelMetadataProvider();
            var metadata = metadataProvider.GetMetadataForType(typeof(Color));

            var httpContext = new Mock<HttpContext>();
            httpContext
                .Setup(h => h.Request.ReadFormAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<IFormCollection>(formCollection));
            httpContext.Setup(h => h.Request.HasFormContentType).Returns(true);

            var bindingContext = new DefaultModelBindingContext
            {
                ActionContext = new ActionContext()
                {
                    HttpContext = httpContext.Object,
                },
                ModelMetadata = metadata,
                ModelName = nameof(Color),
                ModelState = new ModelStateDictionary(),
                ValidationState = new ValidationStateDictionary(),
                ValueProvider = new FormValueProvider(BindingSource.Form, formCollection, CultureInfo.InvariantCulture),
            };

            return bindingContext;
        }
    }
}
