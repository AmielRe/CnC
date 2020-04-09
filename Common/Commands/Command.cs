using System.Net.Sockets;

namespace Common.Commands
{
    /// <summary>
    /// This interface represent a command and therefore every command needs to implement it
    /// </summary>
    public interface Command
    {
        void Execute(Socket sendTo);
        void PrintDescription();
    }
}
