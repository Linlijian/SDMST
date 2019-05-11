namespace FluentValidation.Mvc
{
    using System.Collections.Generic;
    using System.Reflection;
    using System.Web.Mvc;
    using Internal;
    using Validators;

    internal class GreaterThanOrEqualFluentValidationPropertyValidator : FluentValidationPropertyValidator
    {
        GreaterThanOrEqualValidator GreaterThanOrEqualValidator
        {
            get { return (GreaterThanOrEqualValidator)Validator; }
        }

        public GreaterThanOrEqualFluentValidationPropertyValidator(ModelMetadata metadata, ControllerContext controllerContext, PropertyRule rule, IPropertyValidator validator) : base(metadata, controllerContext, rule, validator)
        {
            ShouldValidate = false;
        }

        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
        {
            if (!ShouldGenerateClientSideRules()) yield break;
            var propertyToCompare = GreaterThanOrEqualValidator.MemberToCompare as PropertyInfo;
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


                string message = formatter.BuildMessage(GreaterThanOrEqualValidator.ErrorMessageSource.GetString());
                var rule = new ModelClientValidationRule
                {
                    ErrorMessage = message, // default error message
                    ValidationType = "greaterthanorequal" // name of the validatoin which will be used inside unobtrusive library
                };
                rule.ValidationParameters["type"] = "Ctrl";
                rule.ValidationParameters["valuetocompare"] = CompareAttribute.FormatPropertyForClientValidation(propertyToCompare.Name);
                yield return rule;
            }
            else if (GreaterThanOrEqualValidator.ValueToCompare != null)
            {
                var valueToCompare = GreaterThanOrEqualValidator.ValueToCompare;

                var formatter = new MessageFormatter()
                    .AppendPropertyName(Rule.GetDisplayName())
                    .AppendArgument("ComparisonValue", string.Format("{0:#,##0.##}", valueToCompare));


                string message = formatter.BuildMessage(GreaterThanOrEqualValidator.ErrorMessageSource.GetString());
                var rule = new ModelClientValidationRule
                {
                    ErrorMessage = message, // default error message
                    ValidationType = "greaterthanorequal" // name of the validatoin which will be used inside unobtrusive library
                };
                rule.ValidationParameters["type"] = "Value";
                rule.ValidationParameters["valuetocompare"] = valueToCompare;
                yield return rule;
            }
        }
    }
}