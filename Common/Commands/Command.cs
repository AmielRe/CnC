﻿using System.Net.Sockets;

namespace Common.Commands
{
    public interface Command
    {
        void Execute(Socket sendTo);
        void PrintDescription();
    }
}
