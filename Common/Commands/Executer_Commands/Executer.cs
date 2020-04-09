using System.Net.Sockets;

namespace Common.Commands.Executer_Commands
{
    /// <summary>
    /// This interface represent an executer command and therefore every executer command needs to implement it
    /// </summary>
    public interface Executer : Command
    {
        void Execute(Socket sendTo);
        void PrintDescription();
    }
}
