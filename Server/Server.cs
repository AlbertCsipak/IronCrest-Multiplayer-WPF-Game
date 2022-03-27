using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Server
    {
        List<Socket> Clients;
        Socket ServerSocket;
        public Server(string ip = "26.99.118.45", int clients = 2, int turnLength = 100, int port = 10000)
        {
            Init(ip, clients, port);
            Session(turnLength, clients);
        }
        public void Init(string ip, int clients, int port)
        {

            ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            ServerSocket.Bind(new IPEndPoint(IPAddress.Parse(ip), port));
            ServerSocket.Listen(clients);

            List<Task> tasks = new List<Task>();
            Clients = new List<Socket>();

            int id = 1;

            Console.WriteLine("Waiting for " + clients + " clients...");

            while (Clients.Count < clients)
            {
                Socket client = ServerSocket.Accept();
                Clients.Add(client);
                client.Send(Encoding.ASCII.GetBytes(id.ToString()));
                Console.WriteLine("Client " + id + " has joined.");
                id++;
            }

            Console.WriteLine("Ready...");
        }
        public void Session(int turnLength, int clients)
        {

            Task core = new Task(() =>
            {
                while (Clients.Count == clients)
                {
                    foreach (var item in Clients)
                    {
                        try
                        {
                            item.Send(Encoding.ASCII.GetBytes("true"));
                            Console.WriteLine("I've sent: true to " + item.RemoteEndPoint);
                            ;
                            string msg = "";
                            int x = 0;
                            byte[] buffer = new byte[1024];

                            while (x < turnLength)
                            {
                                item.Receive(buffer);

                                msg = Encoding.ASCII.GetString(buffer);

                                Console.WriteLine("Server received: " + msg);

                                if (msg.Contains("skip"))
                                {
                                    x = turnLength + 100;
                                }
                                else
                                {
                                    foreach (var item2 in Clients)
                                    {
                                        if (item2 != item)
                                        {
                                            item2.Send(buffer);
                                        }
                                    }
                                    x += 1;
                                }
                                Console.WriteLine(x.ToString());
                            }
                            item.Send(Encoding.ASCII.GetBytes("false"));
                            Console.WriteLine("switch");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                }
                //save here
            }, TaskCreationOptions.LongRunning);

            core.Start();

            Console.ReadLine();
        }
    }
}
