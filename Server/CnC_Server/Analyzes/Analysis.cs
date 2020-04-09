using Server.CnC_Server;
using System.Collections.Generic;

namespace CnC_Server.Analyzes
{
    /// <summary>
    /// This interface represent analysis and thus every analysis needs to implement it
    /// </summary>
    public interface Analysis
    {
        void Run(List<Infected_Machine> bots, double paramToCompare);
        void PrintDescription();
    }
}
