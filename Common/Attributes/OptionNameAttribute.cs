using System;

namespace Common.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class OptionNameAttribute : Attribute
    {
        readonly string _optionName;

        public OptionNameAttribute(string optionName)
        {
            this._optionName = optionName;
        }

        public string OptionName
        {
            get { return this._optionName; }
        }
    }
}
