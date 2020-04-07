using Common;
using Common.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CnC_Server.Options
{
    [OptionName("help")]
    public class Help_Option : Option
    {
        private readonly Dictionary<string, Option> AvailableOptions;

        public Help_Option(Dictionary<string, Option> availableOptions)
        {
            this.AvailableOptions = availableOptions;
        }

        /// <summary>
        /// No arguments to the "help" option and therefore return true always
        /// </summary>
        /// <returns>True always</returns>
        public bool ParseArguments(string arguments)
        {
            return true;
        }

        /// <summary>
        /// Describe what the "help" command does
        /// </summary>
        public void PrintDescription()
        {
            Console.WriteLine("This option will print the help menu. FORMAT: help");
        }

        /// <summary>
        /// Print the help menu for the user
        /// </summary>
        public void Run()
        {
            Console.WriteLine("\nHello and welcome to the C&C server!\n\nAvailable options:\n");
            
            foreach(KeyValuePair<string, Option> Entry in AvailableOptions)
            {
                Console.Write(String.Format("{0} - ", Entry.Key));
                Entry.Value.PrintDescription();
            }
        }
    }
}
