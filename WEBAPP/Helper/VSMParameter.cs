using System;

namespace WEBAPP.Helper
{

    public class VSMParameter
    {
        public VSMParameter()
        {
        }
        public VSMParameter(VSMParameterType type, string name, object value)
        {
            Type = type;
            Name = name;
            Value = value;
        }
        public VSMParameter(VSMParameterType type, string name)
        {
            Type = type;
            Name = name;
        }
        public VSMParameter(object name)
        {
            Name = Convert.ToString(name);
            Value = name;
        }
        public VSMParameter(string name, object value)
        {
            Name = name;
            Value = value;
        }
        public VSMParameterType _Type = VSMParameterType.None;
        public VSMParameterType Type
        {
            get
            {
                return _Type;
            }
            set
            {
                _Type = value;
            }
        }
        public string Name { get; set; }
        public object Value { get; set; }
    }


    public enum VSMParameterType
    {
        None,
        ByControlId,
        ByModelData,
        ByGridDate
    }
}
