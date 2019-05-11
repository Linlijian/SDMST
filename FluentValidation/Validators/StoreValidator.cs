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
    using System.Collections.Generic;
    using DataAccess;

    //Add by Waiantarai 2016-12-16
    public class StoreValidator : PropertyValidator
    {
        readonly List<Func<object, object>> funcs;
        public List<MemberInfo> MemberParams { get; private set; }
        public string KeyId { get; private set; }
        public string AppPath { get; private set; }
        public StoreValidator(string keyId, string apppath, List<Func<object, object>> comparisonProperty, List<MemberInfo> members)
            : base(nameof(Messages.Store_error), typeof(Messages))
        {
            AppPath = apppath;
            KeyId = keyId;
            funcs = comparisonProperty;
            MemberParams = members;
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var value = GetValue(context);
            var propertyValue = context.PropertyValue;
            if (!string.IsNullOrEmpty(value))
            {
                propertyValue += "," + value;
            }
            bool success = NotExistData(propertyValue);

            return success;
        }

        private string GetValue(PropertyValidatorContext context)
        {
            var val = string.Empty;
            if (funcs != null)
            {
                foreach (var item in funcs)
                {
                    val += item(context.Instance);
                }
            }

            return val;
        }

        public bool NotExistData(object parameters)
        {
            var existData = true;
            var _DBManger = new SqlDBManger();
            var param = new List<SqlDBParameter>();
            param.Add(new SqlDBParameter
            {                
                ParameterName = "pKEY_ID",
                Value = KeyId,
                DBType = SqlDBType.None,
                Direction = System.Data.ParameterDirection.Input
            });
            param.Add(new SqlDBParameter
            {
                ParameterName = "pParameterValues",
                Value = parameters,
                DBType = SqlDBType.None,
                Direction = System.Data.ParameterDirection.Input
            });

            var data = _DBManger.ExecuteDataSet("SP_AUTOCOMPLETE_GetDataValidate", param);
            if (data.OutputDataSet != null && data.OutputDataSet.Tables.Count > 0 && data.OutputDataSet.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToInt32(data.OutputDataSet.Tables[0].Rows[0][0]) > 0)
                {
                    existData = false;
                }
            }
            return existData;
        }
    }
}