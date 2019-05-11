namespace FluentValidation.Validators
{
    using System;
    using System.Collections;
    using System.Reflection;
    using Attributes;
    using Internal;
    using Resources;
    using System.Linq;

    //Add by Waiantarai 2016-12-16
    public class IDCardValidator : PropertyValidator
    {
        readonly Func<object, object> func;

        public IDCardValidator(Func<object, object> comparisonProperty, MemberInfo member)
            : base(nameof(Messages.IDCardError), typeof(Messages))
        {
            func = comparisonProperty;
            MemberToCompare = member;
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var comparisonValue = GetComparisonValue(context);
            bool success = true;
            
            var value = Convert.ToString(comparisonValue).Replace("-", "").Replace(" ", "");

            if (Convert.ToString(context.PropertyValue) == "1")
            {

                if (value.Length != 13)
                {
                    return true;
                }

                string sText = "";
                string sValue = "";

                if (sText != "")
                {
                    if (sText.IndexOf("-") != -1)
                    {
                        if (sValue.Length == 13)
                        {
                            if (sValue.ToCharArray().All(c => char.IsNumber(c)) == false)
                                return true;

                            int sumValue = 0;
                            for (int i = 0; i < sValue.Length - 1; i++)
                                sumValue += int.Parse(sValue[i].ToString()) * (13 - i);
                            int v = 11 - (sumValue % 11);
                            if (v >= 10)
                            {
                                int r = v % 10;
                                if (sValue[12].ToString() == r.ToString())
                                    success = false;

                            }
                            else
                            {
                                if (sValue[12].ToString() == v.ToString())
                                    success = false;
                            }
                        }
                    }
                    else
                    {
                        if (sText.Length == 13)
                        {
                            if (sText.ToCharArray().All(c => char.IsNumber(c)) == false)
                                return true;

                            int sumValue = 0;
                            for (int i = 0; i < sText.Length - 1; i++)
                                sumValue += int.Parse(sText[i].ToString()) * (13 - i);
                            int v = 11 - (sumValue % 11);
                            if (v >= 10)
                            {
                                int r = v % 10;
                                if (sText[12].ToString() == r.ToString())
                                    success = false;

                            }
                            else
                            {
                                if (sText[12].ToString() == v.ToString())
                                    success = false;
                            }
                        }
                    }
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
