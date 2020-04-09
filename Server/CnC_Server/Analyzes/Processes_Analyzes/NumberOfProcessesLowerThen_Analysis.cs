using Common.Attributes;
using Server.CnC_Server;
using System;
using System.Collections.Generic;

namespace CnC_Server.Analyzes.Processes_Analyzes
{
    [AnalysisName("NumberOfProcessesLowerThen")]
    public class NumberOfProcessesLowerThen_Analysis : Analysis
    {
        /// <summary>
        /// Print the analysis description (what it does)
        /// </summary>
        public void PrintDescription()
        {
            Console.WriteLine("This analysis will analyze current processes data by the received parameter");
        }

        /// <summary>
        /// Run the analysis on the received bots and param.
        /// </summary>
        /// <param name="bots">The bots list to check the analysis on</param>
        /// <param name="paramToCompare">The param to do the analysis on</param>
        public void Run(List<Infected_Machine> bots, double paramToCompare)
        {
            bool botPrinted = false;

            // Go through every bot in the list and do the analysis
            for(int i = 0;i<bots.Count;i++)
            {
                if (!double.IsNaN(bots[i].GetValueByName("processes"))
                    && bots[i].GetValueByName("processes") < paramToCompare)
                {
                    Console.WriteLine("Bot ID: {0}", i+1);
                    Console.WriteLine("Bot IP: {0}", bots[i].ip);
                    botPrinted = true;
                }
            }

            if (!botPrinted)
            {
                Console.WriteLine("No bot data match the given parameter");
                return;
            }
        }
    }
}
