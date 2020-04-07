using CnC_Server.Analyzes;
using Common.Attributes;
using Server.CnC_Server;
using System;
using System.Collections.Generic;

namespace CnC_Server.Options
{
    [OptionName("print-bot-analysis")]
    public class PrintBotAnalysis_Option : Option
    {
        private List<Infected_Machine> Bots;
        private Dictionary<string, Analysis> Analyzes;
        private string AnalysisName;
        private double Param;

        public PrintBotAnalysis_Option(List<Infected_Machine> bots, Dictionary<string, Analysis> analyzes)
        {
            this.Bots = bots;
            this.Analyzes = analyzes;
        }

        public bool ParseArguments(string arguments)
        {
            if (arguments.Length > 0 && arguments.Contains(" "))
            {
                String[] splitArguments = arguments.Split(' ');
                if (splitArguments.Length == 2 && Analyzes.ContainsKey(splitArguments[0]))
                {
                    SetAnalysisName(splitArguments[0]);
                    SetParam(double.Parse(splitArguments[1]));
                }
            }

            return false;
        }

        public void PrintDescription()
        {
            Console.WriteLine("This option will run an analysis (on the data you have locally) and print bots who match this analysis (IP and ID). FORMAT: print-bot-analysis <name of analysis> <param>");
        }

        public void Run()
        {
            Analyzes[AnalysisName].run(Bots, Param);
        }

        private void SetAnalysisName(string analysisName)
        {
            this.AnalysisName = analysisName;
        }

        private void SetParam(double param)
        {
            this.Param = param;
        }
    }
}
