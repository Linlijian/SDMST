namespace FluentValidation.Validators
{
    using System.Linq;
    using Resources;
    using System.Reflection;
    using System.Collections.Generic;
    using System;

    public class NotFileExeBatValidator : PropertyValidator
    {
        public List<MemberInfo> MemberParams { get; private set; }
        readonly List<Func<object, object>> funcs;
        public string KeyId { get; private set; }
        public NotFileExeBatValidator() : base(nameof(Messages.FileTypeError), typeof(Messages))
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

            if (value == "exe" || value == "bat")
            {
                sTrue = true;
            }
            else
            {
                sTrue = false;
            }

            return sTrue;
        }
    }
}
