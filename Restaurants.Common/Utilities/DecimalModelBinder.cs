using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Localization;
using Restaurants.Common.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Common.Utilities
{
    public class DecimalModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (valueProviderResult == null)
            {
                return Task.CompletedTask;
            }

            var value = valueProviderResult.FirstValue;

            if (string.IsNullOrEmpty(value))
            {
                return Task.CompletedTask;
            }

            // Remove unnecessary commas and spaces
            value = value.Replace(",", string.Empty).Trim();

            decimal myValue = 0;
            if (!decimal.TryParse(value,NumberStyles.Any,new CultureInfo("en"), out myValue))
            {
                
                bindingContext.ModelState.TryAddModelError(
                                        bindingContext.ModelName,
                                        "Could not parse MyValue.");
                return Task.CompletedTask;
            }
            if (myValue < 0.00m || myValue > 9999.99m)
            {
                bindingContext.ModelState.TryAddModelError(
                                    bindingContext.ModelName,
                                    "Price must be between 0.00 and 9999.99!");
            }
            bindingContext.Result = ModelBindingResult.Success(myValue);
            return Task.CompletedTask;
        }
    }
}
