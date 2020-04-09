using System.Net.Sockets;

namespace Common.Commands.Collector_Commands
{
    /// <summary>
    /// This interface represent an collector command and therefore every collector command needs to implement it
    /// </summary>
    public interface Collector : Command
    {
        void Execute(Socket sendTo);
        void PrintDescription();
    }
}
