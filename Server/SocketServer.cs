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
                client.Send(Encoding.ASCII.GetBytes(map));
                Console.WriteLine("Client " + id + " has joined.");
                id++;
            }
            Console.WriteLine("Ready...");
        }
        public void Session(int turnLength, int clients, int bufferSize)
        {

            Task core = new Task(() =>
            {
                while (Clients.Count == clients)
                {
                    foreach (var item in Clients)
                    {
                        try
                        {
                            foreach (var item2 in Clients)
                            {
                                item2.Send(Encoding.ASCII.GetBytes("timer"));
                            }
                            System.Threading.Thread.Sleep(250);

                            item.Send(Encoding.ASCII.GetBytes("true"));
                            Console.WriteLine("I've sent true to " + item.RemoteEndPoint);

                            string msg = "";
                            int x = 0;

                            while (x < turnLength)
                            {
                                byte[] buffer = new byte[20000];
                                item.Receive(buffer);

                                msg = Encoding.ASCII.GetString(buffer);

                                Console.WriteLine("Server received a packet from :" + item.RemoteEndPoint);

                                string message = "";
                                int idx = 0;
                                while (buffer[idx] != 0)
                                {
                                    byte[] helper = new byte[1];
                                    helper[0] = buffer[idx];
                                    message = message + Encoding.ASCII.GetString(helper);
                                    idx++;
                                    if (20000 - 1 == idx)
                                    {
                                        break;
                                    }
                                }
                                Console.WriteLine(message);

                                if (msg.Contains("skip"))
                                {
                                    break;
                                }

                                foreach (var item2 in Clients)
                                {
                                    if (item2 != item)
                                    {
                                        ;
                                        item2.Send(buffer);
                                    }
                                }

                                x += 1;
                                Console.WriteLine(x.ToString());
                            }
                            item.Send(Encoding.ASCII.GetBytes("false"));
                            Console.WriteLine("switch");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                            Console.WriteLine("Press enter to close the server window.");
                            Clients.Remove(item);
                        }
                    }
                }
            }, TaskCreationOptions.LongRunning);
            core.Start();

            while (Clients.Count == clients)
            {
                //:)
                System.Threading.Thread.Sleep(500);
            }

            ServerSocket.Close();
            ServerSocket.Dispose();
        }
    }
}
