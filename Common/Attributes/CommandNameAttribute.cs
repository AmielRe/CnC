using System;

namespace Common.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CommandNameAttribute : Attribute
    {
        readonly string _commandName;

        public CommandNameAttribute(string commandName)
        {
            this._commandName = commandName;
        }

        public string CommandName
        {
            get { return this._commandName; }
        }
    }
}
