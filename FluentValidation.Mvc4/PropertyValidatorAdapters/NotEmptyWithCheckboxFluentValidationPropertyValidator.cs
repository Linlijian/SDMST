namespace FluentValidation.Mvc
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Internal;
    using Validators;
    using System.Reflection;

    internal class NotEmptyWithCheckboxFluentValidationPropertyValidator : FluentValidationPropertyValidator
    {
        private NotEmptyWithCheckboxValidator NotEmptyWithCheckboxValidator
        {
            get { return (NotEmptyWithCheckboxValidator)Validator; }
        }

        public NotEmptyWithCheckboxFluentValidationPropertyValidator(ModelMetadata metadata, ControllerContext controllerContext, PropertyRule rule, IPropertyValidator validator) : base(metadata, controllerContext, rule, validator)
        {
            ShouldValidate = false;
        }

        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
        {
            if (!ShouldGenerateClientSideRules()) yield break;

            var propertyToCompare = NotEmptyWithCheckboxValidator.MemberToCompare as PropertyInfo;
            if (propertyToCompare != null)
            {
                // If propertyToCompare is not null then we're comparing to another property.
                // If propertyToCompare is null then we're either comparing against a literal value, a field or a method call.
                // We only care about property comparisons in this case.

                var comparisonDisplayName =
                    ValidatorOptions.DisplayNameResolver(Rule.TypeToValidate, propertyToCompare, null)
                    ?? propertyToCompare.Name.SplitPascalCase();

                var formatter = new MessageFormatter()
                    .AppendPropertyName(Rule.GetDisplayName())
                    .AppendArgument("PropertyNameTo", comparisonDisplayName);

                string message = formatter.BuildMessage(NotEmptyWithCheckboxValidator.ErrorMessageSource.GetString());

                var rule = new ModelClientValidationRule
                {
                    ErrorMessage = message, // default error message
                    ValidationType = "notemptywithcheckbox" // name of the validatoin which will be used inside unobtrusive library
                };

                rule.ValidationParameters["comparewith"] = propertyToCompare.Name; // html element which includes prefix information
                yield return rule;
            }
        }
    }
}