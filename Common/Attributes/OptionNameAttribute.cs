using System;

namespace Common.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class OptionNameAttribute : Attribute
    {
        readonly string _optionName;

        // Set the option name
        public OptionNameAttribute(string optionName)
        {
            this._optionName = optionName;
        }

        // Return the option name
        public string OptionName
        {
            get { return this._optionName; }
        }
    }
}
