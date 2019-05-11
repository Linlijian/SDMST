#region License
// Copyright (c) Jeremy Skinner (http://www.jeremyskinner.co.uk)
// 
// Licensed under the Apache License, Version 2.0 (the "License"); 
// you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at 
// 
// http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software 
// distributed under the License is distributed on an "AS IS" BASIS, 
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
// See the License for the specific language governing permissions and 
// limitations under the License.
// 
// The latest version of this file can be found at https://github.com/jeremyskinner/FluentValidation
#endregion

namespace FluentValidation.Validators
{
    using System;
    using System.Collections;
    using System.Reflection;
    using Attributes;
    using Internal;
    using Resources;
    //Add by Waiantarai 2016-12-16
    public class CheckBoxCheckValidator : PropertyValidator
    {
        readonly Func<object, object> func;

        public CheckBoxCheckValidator(Func<object, object> comparisonProperty, MemberInfo member)
            : base(nameof(Messages.empty_error), typeof(Messages))
        {
            func = comparisonProperty;
            MemberToCompare = member;
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var comparisonValue = GetComparisonValue(context);
            bool success = true;

            if (Convert.ToString(comparisonValue) == "N" && (Convert.ToString(context.PropertyValue) != "N"))
            {
                success = false;
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