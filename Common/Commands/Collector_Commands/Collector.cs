using System.Net.Sockets;

namespace Common.Commands.Collector_Commands
{
    public interface Collector : Command
    {
        void Execute(Socket sendTo);
        void PrintDescription();
    }
}
