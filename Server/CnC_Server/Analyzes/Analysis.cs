using Server.CnC_Server;
using System.Collections.Generic;

namespace CnC_Server.Analyzes
{
    public interface Analysis
    {
        void run(List<Infected_Machine> bots, double paramToCompare);
        void printDescription();
    }
}
