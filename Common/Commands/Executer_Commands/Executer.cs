using System.Net.Sockets;

namespace Common.Commands.Executer_Commands
{
    public interface Executer : Command
    {
        void Execute(Socket sendTo);
        void PrintDescription();
    }
}
