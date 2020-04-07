using CnC_Server.Analyzes;
using Common.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CnC_Server.Options
{
    [OptionName("print-analyzes")]
    public class PrintAnalyzes_Option : Option
    {
        private Dictionary<string, Analysis> Analyzes;

        public PrintAnalyzes_Option(Dictionary<string, Analysis> analyzes)
        {
            this.Analyzes = analyzes;
        }

        public bool ParseArguments(string arguments)
        {
            return true;
        }

        public void PrintDescription()
        {
            Console.WriteLine("This option will print a list of all the possible analysis the server can execute - name of analysis and what it does. FORMAT: print-analyzes");
        }

        public void Run()
        {
            if (Analyzes.Count < 1)
            {
                Console.WriteLine("No analyzes available!");
                return;
            }

            foreach (KeyValuePair<string, Analysis> Entry in Analyzes)
            {
                Console.Write(String.Format("{0} - ", Entry.Key));
                Entry.Value.printDescription();
            }
        }
    }
}
