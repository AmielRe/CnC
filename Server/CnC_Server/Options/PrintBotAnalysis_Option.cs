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

        /// <summary>
        /// Receive the arguments of the option, check if they are valid and if they are, set their values to be prepared for execution
        /// </summary>
        /// <param name="arguments">The received arguments for the option</param>
        /// <returns>True for success ; False for failure</returns>
        public bool ParseArguments(string[] arguments)
        {
            // If in correct format, set the arguments
            if (arguments.Length == 2 && Analyzes.ContainsKey(arguments[0]))
            {
                SetAnalysisName(arguments[0]);
                SetParam(double.Parse(arguments[1]));

                return true;
            }

            return false;
        }

        /// <summary>
        /// Print the option description (what it does)
        /// </summary>
        public void PrintDescription()
        {
            Console.WriteLine("This option will run an analysis (on the data you have locally) and print bots who match this analysis (IP and ID). FORMAT: print-bot-analysis <name of analysis> <param>\n");
        }

        /// <summary>
        /// Runs the option with the set arguments.
        /// </summary>
        public void Run()
        {
            Analyzes[AnalysisName].Run(Bots, Param);
        }

        /// <summary>
        /// Set the analysisName argument
        /// </summary>
        /// <param name="commandName">The command name to set</param>
        private void SetAnalysisName(string analysisName)
        {
            this.AnalysisName = analysisName;
        }

        /// <summary>
        /// Set the param to compare to argument
        /// </summary>
        /// <param name="param">The param to compare to</param>
        private void SetParam(double param)
        {
            this.Param = param;
        }
    }
}
