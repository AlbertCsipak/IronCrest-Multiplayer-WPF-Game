using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class SocketServer
    {
        List<Socket> Clients;
        Socket ServerSocket;
        public SocketServer()
        {
        }
        public void Init(string ip, int clients, int port, string map)
        {

            ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            ServerSocket.Bind(new IPEndPoint(IPAddress.Parse(ip), port));
            ServerSocket.Listen(clients);

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
        public void Session(int turnLength,int clients, int bufferSize)
        {
            foreach (var item in Clients)
            {
                Task core = new Task(() =>
                {
                    while (Clients.Count == clients)
                    {
                        try
                        {
                            byte[] buffer = new byte[bufferSize];
                            item.Receive(buffer);
                            Console.WriteLine($"Message from: {item}");
                            foreach (var item2 in Clients)
                            {
                                if (item2!=item)
                                {
                                    item2.Send(buffer);
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                            Clients.Remove(item);
                        }
                    }
                }, TaskCreationOptions.LongRunning);
                core.Start();
            }
            
            while (Clients.Count == clients)
            {
                //:)
                System.Threading.Thread.Sleep(100);
            }
            Console.WriteLine("Press enter to close the server window.");
            Console.ReadLine();

            ServerSocket.Close();
            ServerSocket.Dispose();
        }
    }
}
