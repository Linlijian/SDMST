namespace FluentValidation.Validators
{
    using System.Linq;
    using Resources;
    using System.Reflection;
    using System.Collections.Generic;
    using System;

    public class CheckPasswordValidator : PropertyValidator
    {
        public List<MemberInfo> MemberParams { get; private set; }
        readonly List<Func<object, object>> funcs;
        public string KeyId { get; private set; }
        public CheckPasswordValidator() : base(nameof(Messages.email_error), typeof(Messages))
        {

        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var value = context.PropertyValue as string;

            if (value == null)
            {
                return true;
            }
            bool sTrue = true;

            if (System.Text.RegularExpressions.Regex.Match(value, @"/.[!,@,#,$,%,^,&,*,?,_,~,-,£,(,)]/", System.Text.RegularExpressions.RegexOptions.ECMAScript).Success)
            {
                sTrue = false;
            }

            return sTrue;
        }
    }
}
