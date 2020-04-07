using Common.Attributes;
using Common.Commands;
using System;
using System.Collections.Generic;

namespace CnC_Server.Options
{
    [OptionName("print-commands")]
    public class PrintCommands_Option : Option
    {
        private Dictionary<string, Command> Commands;

        public PrintCommands_Option(Dictionary<string, Command> commands)
        {
            this.Commands = commands;
        }

        /// <summary>
        /// Run the print commands option
        /// </summary>
        public void Run()
        {
            if(Commands.Count < 1)
            {
                Console.WriteLine("No commands available!");
                return;
            }

            foreach (KeyValuePair<string, Command> Entry in Commands)
            {
                Console.Write(String.Format("{0} - ", Entry.Key));
                Entry.Value.printDescription();
            }
        }

        /// <summary>
        /// Describe what the "print commands" option does
        /// </summary>
        public void PrintDescription()
        {
            Console.WriteLine("This option will print a list of all the possible commands the server can execute - name of command and what it does. FORMAT: print-commands");
        }

        /// <summary>
        /// No arguments to the "print commands" option and therefore return true always
        /// </summary>
        public bool ParseArguments(string[] arguments)
        {
            return true;
        }
    }
}
