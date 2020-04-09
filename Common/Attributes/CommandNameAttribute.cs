using System;

namespace Common.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CommandNameAttribute : Attribute
    {
        readonly string _commandName;

        // Set the command name
        public CommandNameAttribute(string commandName)
        {
            this._commandName = commandName;
        }

        // Return the command name
        public string CommandName
        {
            get { return this._commandName; }
        }
    }
}
