using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CnC_Server.Options
{
    /// <summary>
    /// General option interface - all options implement the option interface
    /// </summary>
    public interface Option
    {
        void Run();
        bool ParseArguments(string arguments);
        void PrintDescription();
    }
}
