namespace FluentValidation.Validators
{
    using System;
    using System.Collections;
    using System.Reflection;
    using Attributes;
    using Internal;
    using Resources;
    using System.Linq;
    
    public class NotEmptyWithCheckboxValidator : PropertyValidator
    {
        readonly Func<object, object> func;

        public NotEmptyWithCheckboxValidator(Func<object, object> comparisonProperty, MemberInfo member)
            : base(nameof(Messages.notempty_error), typeof(Messages))
        {
            func = comparisonProperty;
            MemberToCompare = member;
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var comparisonValue = GetComparisonValue(context);
            bool success = true;

            var value = Convert.ToString(comparisonValue);

            if (Convert.ToString(context.PropertyValue) == "Y")
            {
                if (value == "" || value == null)
                {
                    return true;
                }
            }

            if (!success)
            {
                context.MessageFormatter.AppendArgument("PropertyNameTo", MemberToCompare.Name);
                return false;
            }

            return true;
        }

        private object GetComparisonValue(PropertyValidatorContext context)
        {
            if (func != null)
            {
                return func(context.Instance);
            }

            return null;
        }

        public MemberInfo MemberToCompare { get; private set; }

    }
}
