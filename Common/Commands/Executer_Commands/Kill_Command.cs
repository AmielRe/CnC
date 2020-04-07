using Common.Attributes;
using System;
using System.Net.Sockets;
using System.Runtime.Serialization;

namespace Common.Commands.Executer_Commands
{
    [Serializable]
    [CommandName("kill")]
    public class Kill_Command : Command
    {
        public void execute(Socket sendTo)
        {
            try
            {
                Environment.Exit(1);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error occured while executing kill command!\n{0}", ex.ToString());
            }
        }

        public void printDescription()
        {
            Console.WriteLine("This plugin will make the infected machine kill itself");
        }

        #region Serializable

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
