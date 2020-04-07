using Server.CnC_Server;
using System;
using System.Collections.Generic;

namespace CnC_Server.Options
{
    [OptionName("print-bot-status")]
    public class PrintBotStatus_Option : Option
    {
        private List<Infected_Machine> Bots;
        private int BotID;

        public PrintBotStatus_Option(List<Infected_Machine> bots)
        {
            this.Bots = bots;
        }

        public void Run()
        {
            if (Bots.Count >= BotID)
            {
                double ramData = Bots[BotID - 1].GetValueByName("ram");
                double processesData = Bots[BotID - 1].GetValueByName("processes");

                Console.WriteLine("\nBot ID: {0}", BotID);

                if(double.IsNaN(ramData))
                {
                    Console.WriteLine("No ram information on this bot.");
                }
                else
                {
                    Console.WriteLine("Ram: {0}%", ramData);
                }

                if (double.IsNaN(ramData))
                {
                    Console.WriteLine("No processes information on this bot.");
                }
                else
                {
                    Console.WriteLine("Number of processes: {0}", (int)ramData);
                }
            }
            else
            {
                Console.WriteLine("The requested bot is not in the system.");
            }
        }

        /// <summary>
        /// Describe what the "print bot-status" option does
        /// </summary>
        public void PrintDescription()
        {
            Console.WriteLine("This command will print all the information from the specific bot saved locally on the server. FORMAT: print bot-analysis <bot id>");
        }

        public bool ParseArguments(string arguments)
        {
            if (int.TryParse(arguments, out int botID) && botID > 0)
            {
                SetBotID(botID);
                return true;
            }
            return false;
        }

        private void SetBotID(int botID)
        {
            this.BotID = botID;
        }
    }
}
