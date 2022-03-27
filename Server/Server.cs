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
        List<Socket> Clients { get; set; }
        public Server(string ip = "26.99.118.45", int clients = 2, int turnLength = 100)
        {
            Init(ip, clients);
            Session(turnLength);
        }
        public void Init(string ip, int clients)
        {

            Socket listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var endPoint = new IPEndPoint(IPAddress.Parse(ip), 10000);
            listenerSocket.Bind(endPoint);
            listenerSocket.Listen(clients);

            List<Task> tasks = new List<Task>();
            Clients = new List<Socket>();

            int id = 1;

            Console.WriteLine("Waiting for " + clients + " clients...");

            while (Clients.Count < clients)
            {
                Socket client = listenerSocket.Accept();
                Clients.Add(client);
                client.Send(Encoding.ASCII.GetBytes(id.ToString()));
                Console.WriteLine("Client " + id + " has joined.");
                id++;
            }

            Console.WriteLine("Ready...");
        }
        public void Session(int turnLength)
        {

            Task core = new Task(() =>
            {
                while (true)
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
            }, TaskCreationOptions.LongRunning);

            core.Start();

            Console.ReadLine();
        }
    }
}
