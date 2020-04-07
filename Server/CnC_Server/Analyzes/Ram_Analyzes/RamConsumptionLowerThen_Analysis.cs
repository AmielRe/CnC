using Common.Attributes;
using Server.CnC_Server;
using System;
using System.Collections.Generic;

namespace CnC_Server.Analyzes.Ram_Analyzes
{
    [AnalysisName("RamLowerThen")]
    public class RamConsumptionLowerThen_Analysis : Analysis
    {
        public void printDescription()
        {
            Console.WriteLine("This analysis will analyze current RAM data by the received parameter");
        }

        public void run(List<Infected_Machine> bots, double paramToCompare)
        {
            bool botPrinted = false;

            for(int i = 0; i < bots.Count; i++)
            {
                if(!double.IsNaN(bots[i].GetValueByName("ram"))
                    && bots[i].GetValueByName("ram") < paramToCompare)
                {
                    Console.WriteLine("Bot ID: {0}", i + 1);
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
