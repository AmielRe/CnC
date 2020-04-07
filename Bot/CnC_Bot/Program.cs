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
            Bot CnC_Bot = new Bot();
            CnC_Bot.Start("127.0.0.1", 8425);
        }
    }
}
