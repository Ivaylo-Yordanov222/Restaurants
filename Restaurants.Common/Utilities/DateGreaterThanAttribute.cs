using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Restaurants.Common.Resources;
using System;
using System.ComponentModel.DataAnnotations;

namespace Restaurants.Common.Utilities
{
    public class DateGreaterThanAttribute : ValidationAttribute, IClientModelValidator
    {
        public string otherPropertyName;
        public DateGreaterThanAttribute() { }
        public DateGreaterThanAttribute(string otherPropertyName, string errorMessage)
            : base(errorMessage)
        {
            this.otherPropertyName = otherPropertyName;
        }

        private bool _checkedForLocalizer;
        private IStringLocalizer _stringLocalizer;
        public string LocalizedError { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var localizationService = (IStringLocalizer)validationContext.GetService(typeof(IStringLocalizer<ValidationResources>));
            this.LocalizedError = localizationService[ErrorMessageString];
            ValidationResult validationResult = ValidationResult.Success;
            try
            {
                var containerType = validationContext.ObjectInstance.GetType();
                var field = containerType.GetProperty(this.otherPropertyName);
                var extensionValue = field.GetValue(validationContext.ObjectInstance, null);
                if (extensionValue == null)
                {
                    return validationResult;
                }
                var datatype = extensionValue.GetType();

                if (field == null)
                    return new ValidationResult(String.Format("Unknown property: {0}.", otherPropertyName));

                if ((field.PropertyType == typeof(DateTime) || (field.PropertyType.IsGenericType && field.PropertyType == typeof(Nullable<DateTime>))))
                {
                    DateTime toValidate = (DateTime)value;
                    DateTime referenceProperty = (DateTime)field.GetValue(validationContext.ObjectInstance, null);
                    
                    if (toValidate.CompareTo(referenceProperty) < 1)
                    {
                        validationResult = new ValidationResult(this.LocalizedError);
                    }
                }
                else
                {
                    validationResult = new ValidationResult("An error occurred while validating the property. OtherProperty is not of type DateTime");
                }
            }
            catch (Exception ex)
            {
                // Do stuff, i.e. log the exception
                // Let it go through the upper levels, something bad happened
                throw ex;
            }

            return validationResult;
        }
        public void AddValidation(ClientModelValidationContext context)
        {
            CheckForLocalizer(context);
            var errorMessage = GetErrorMessage(context.ModelMetadata.GetDisplayName());
            context.Attributes.Add("data-val", "true");
            context.Attributes.Add("data-val-isgreater", errorMessage);
            context.Attributes.Add("data-val-isgreater-otherproperty", "StartTime");
        }
        private string GetErrorMessage(string displayName)
        {
            if (_stringLocalizer != null &&
                !string.IsNullOrEmpty(ErrorMessage) &&
                string.IsNullOrEmpty(ErrorMessageResourceName) &&
                ErrorMessageResourceType == null)
            {
                return _stringLocalizer[ErrorMessage, displayName];
            }

            return FormatErrorMessage(displayName);
        }

        private void CheckForLocalizer(ClientModelValidationContext context)
        {
            if (!_checkedForLocalizer)
            {
                _checkedForLocalizer = true;

                var services = context.ActionContext.HttpContext.RequestServices;
                var options = services.GetRequiredService<IOptions<MvcDataAnnotationsLocalizationOptions>>();
                var factory = services.GetService<IStringLocalizerFactory>();

                var provider = options.Value.DataAnnotationLocalizerProvider;
                if (factory != null && provider != null)
                {
                    _stringLocalizer = provider(
                        context.ModelMetadata.ContainerType ?? context.ModelMetadata.ModelType,
                        factory);
                }
            }
        }
    }
}
