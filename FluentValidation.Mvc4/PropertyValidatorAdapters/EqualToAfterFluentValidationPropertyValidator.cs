namespace FluentValidation.Mvc
{
    using System.Collections.Generic;
    using System.Reflection;
    using System.Web.Mvc;
    using Internal;
    using Validators;

    internal class EqualToAfterFluentValidationPropertyValidator : FluentValidationPropertyValidator
    {
        EqualAfterValidator EqualAfterValidator
        {
            get { return (EqualAfterValidator)Validator; }
        }

        public EqualToAfterFluentValidationPropertyValidator(ModelMetadata metadata, ControllerContext controllerContext, PropertyRule rule, IPropertyValidator validator) : base(metadata, controllerContext, rule, validator)
        {
            ShouldValidate = false;
        }

        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
        {
            if (!ShouldGenerateClientSideRules()) yield break;

            var propertyToCompare = EqualAfterValidator.MemberToCompare as PropertyInfo;
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
                    .AppendArgument("ComparisonValue", comparisonDisplayName);


                string message = formatter.BuildMessage(EqualAfterValidator.ErrorMessageSource.GetString());
                var rule = new ModelClientValidationRule
                {
                    ErrorMessage = message, // default error message
                    ValidationType = "equaltoafter" // name of the validatoin which will be used inside unobtrusive library
                };
                rule.ValidationParameters["other"] = CompareAttribute.FormatPropertyForClientValidation(propertyToCompare.Name);
                yield return rule;
            }
        }
    }
}