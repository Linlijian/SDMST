namespace FluentValidation.Mvc
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Internal;
    using Validators;
    using System.Reflection;

    internal class CheckBoxCheckValidationPropertyValidator : FluentValidationPropertyValidator
    {
        private CheckBoxCheckValidator CheckBoxCheckValidator
        {
            get { return (CheckBoxCheckValidator)Validator; }
        }

        public CheckBoxCheckValidationPropertyValidator(ModelMetadata metadata, ControllerContext controllerContext, PropertyRule rule, IPropertyValidator validator) : base(metadata, controllerContext, rule, validator)
        {
            ShouldValidate = false;
        }

        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
        {
            if (!ShouldGenerateClientSideRules()) yield break;

            var propertyToCompare = CheckBoxCheckValidator.MemberToCompare as PropertyInfo;
            if (propertyToCompare != null)
            {
                var comparisonDisplayName =
                    ValidatorOptions.DisplayNameResolver(Rule.TypeToValidate, propertyToCompare, null)
                    ?? propertyToCompare.Name.SplitPascalCase();

                var formatter = new MessageFormatter()
                    .AppendPropertyName(Rule.GetDisplayName())
                    .AppendArgument("PropertyNameTo", comparisonDisplayName);

                string message = formatter.BuildMessage(CheckBoxCheckValidator.ErrorMessageSource.GetString());
                
                var rule = new ModelClientValidationRule
                {
                    ErrorMessage = message,
                    ValidationType = "checkboxcheck" 
                };

                rule.ValidationParameters["comparewith"] = propertyToCompare.Name;
                yield return rule;
            }
        }
    }
}