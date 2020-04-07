using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common.Commands
{
    public interface Command : ISerializable
    {
        void execute(Socket sendTo);
        void printDescription();
    }
}
