using Common.Attributes;
using System;
using System.Net.Sockets;
using System.Runtime.Serialization;

namespace Common.Commands.Executer_Commands
{
    [Serializable]
    [CommandName("kill")]
    public class Kill_Command : Executer
    {
        // <summary>
        /// Execute the kill command
        /// </summary>
        public void Execute(Socket sendTo)
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

        /// <summary>
        /// Print the command description (what it does)
        /// </summary>
        public void PrintDescription()
        {
            Console.WriteLine("This command will make the infected machine kill itself");
        }
    }
}
