namespace FluentValidation.Mvc
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Internal;
    using Validators;
    using System.Reflection;

    internal class StorePropertyValidator : FluentValidationPropertyValidator
    {
        private StoreValidator StoreValidator
        {
            get { return (StoreValidator)Validator; }
        }

        public StorePropertyValidator(ModelMetadata metadata, ControllerContext controllerContext, PropertyRule rule, IPropertyValidator validator) : base(metadata, controllerContext, rule, validator)
        {
            ShouldValidate = false;
        }

        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
        {
            if (!ShouldGenerateClientSideRules()) yield break;

            var paramName = string.Empty;
            if (StoreValidator.MemberParams != null)
            {
                foreach (var item in StoreValidator.MemberParams)
                {
                    paramName += item.Name + ",";
                }
            }
            paramName = paramName.TrimEnd(',');
            var formatter = new MessageFormatter()
                   .AppendPropertyName(Rule.GetDisplayName());

            string message = formatter.BuildMessage(StoreValidator.ErrorMessageSource.GetString());

            var rule = new ModelClientValidationRule
            {
                ErrorMessage = message, // default error message
                ValidationType = "store" // name of the validatoin which will be used inside unobtrusive library
            };
            rule.ValidationParameters["keyid"] = StoreValidator.KeyId;
            rule.ValidationParameters["apppath"] = StoreValidator.AppPath;
            rule.ValidationParameters["parameternames"] = paramName; // html element which includes prefix information
            yield return rule;
        }
    }
}