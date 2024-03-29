﻿using Common.Attributes;
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

        /// <summary>
        /// Runs the option with the set arguments.
        /// </summary>
        public void Run()
        {
            if (Bots.Count >= BotID)
            {
                // Get the current bot data
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

                if (double.IsNaN(processesData))
                {
                    Console.WriteLine("No processes information on this bot.");
                }
                else
                {
                    Console.WriteLine("Number of processes: {0}", (int)processesData);
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
            Console.WriteLine("This option will print all the information from the specific bot saved locally on the server. FORMAT: print bot-status <bot id>\n");
        }

        /// <summary>
        /// Receive the arguments of the option, check if they are valid and if they are, set their values to be prepared for execution
        /// </summary>
        /// <param name="arguments">The received arguments for the option</param>
        /// <returns>True for success ; False for failure</returns>
        public bool ParseArguments(string[] arguments)
        {
            // If in correct format, set the argument
            if (arguments.Length == 1
                && int.TryParse(arguments[0], out int botID) && botID > 0)
            {
                SetBotID(botID);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Set the botID argument
        /// </summary>
        /// <param name="botID">The botID to set</param>
        private void SetBotID(int botID)
        {
            this.BotID = botID;
        }
    }
}
