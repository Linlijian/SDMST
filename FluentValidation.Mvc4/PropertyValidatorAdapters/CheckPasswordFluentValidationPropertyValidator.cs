namespace FluentValidation.Mvc
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Internal;
    using Validators;
    using System.Reflection;

    internal class CheckPasswordFluentValidationPropertyValidator : FluentValidationPropertyValidator
    {
        private CheckPasswordValidator NotFileExeBatValidator
        {
            get { return (CheckPasswordValidator)Validator; }
        }

        public CheckPasswordFluentValidationPropertyValidator(ModelMetadata metadata, ControllerContext controllerContext, PropertyRule rule, IPropertyValidator validator) : base(metadata, controllerContext, rule, validator)
        {
            ShouldValidate = false;
        }

        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
        {
            if (!ShouldGenerateClientSideRules()) yield break;

            var paramName = string.Empty;
            if (NotFileExeBatValidator.MemberParams != null)
            {
                foreach (var item in NotFileExeBatValidator.MemberParams)
                {
                    paramName += item.Name + ",";
                }
            }
            paramName = paramName.TrimEnd(',');
            var formatter = new MessageFormatter()
                   .AppendPropertyName(Rule.GetDisplayName());

            string message = formatter.BuildMessage(NotFileExeBatValidator.ErrorMessageSource.GetString());

            var rule = new ModelClientValidationRule
            {
                ErrorMessage = message, // default error message
                ValidationType = "chackpassword" // name of the validatoin which will be used inside unobtrusive library
            };
            //rule.ValidationParameters["keyid"] = StoreValidator.KeyId;
            rule.ValidationParameters["parameternames"] = paramName; // html element which includes prefix information
            yield return rule;
        }
    }
}