using Common.Attributes;
using Server.CnC_Server;
using System;
using System.Collections.Generic;

namespace CnC_Server.Options
{
    [OptionName("print-bots")]
    public class PrintBots_Option : Option
    {
        private List<Infected_Machine> Bots;

        public PrintBots_Option(List<Infected_Machine> bots)
        {
            this.Bots = bots;
        }

        /// <summary>
        /// Runs the print bots option
        /// </summary>
        public void Run()
        {
            if(Bots.Count < 1)
            {
                Console.WriteLine("No active bots in the system!");
                return;
            }

            for(int i = 0;i<Bots.Count;i++)
            {
                Console.WriteLine("Bot ID: {0}  Bot IP: {1}\n", i + 1, Bots[i].ip);
            }
        }

        /// <summary>
        /// Describe what the "print bots" option does
        /// </summary>
        public void PrintDescription()
        {
            Console.WriteLine("This option will print a list of IPs of all bots and the Identifier of each bot. FORMAT: print-bots");
        }

        /// <summary>
        /// No arguments to the "print bots" option and therefore return true always
        /// </summary>
        public bool ParseArguments(string[] arguments)
        {
            return true;
        }
    }
}
