using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClientSocket
{
    public class Client
    {
        int ClientId { get; set; }
        bool canSend { get; set; }
        Socket client { get; set; }

        int Number = 0;
        public Client(string ip = "127.0.0.1")
        {
            Connection(ip);
        }
        public void Connection(string ip)
        {
            try
            {
                client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                client.Connect(IPAddress.Parse(ip), 10000);

                byte[] id = new byte[1];
                client.Receive(id);
                ClientId = int.Parse(Encoding.ASCII.GetString(id));

                Task send = new Task(() =>
                {
                    while (true)
                    {
                        if (canSend)
                        {
                            client.Send(Encoding.ASCII.GetBytes(Number.ToString()));
                            Console.WriteLine("I've sent: " + Number);
                            Number++;
                        }
                        System.Threading.Thread.Sleep(100);
                    }
                }, TaskCreationOptions.LongRunning);

                Task receive = new Task(() =>
                {
                    while (true)
                    {
                        byte[] buffReceived = new byte[8];
                        int nRec = client.Receive(buffReceived);
                        string msg = Encoding.ASCII.GetString(buffReceived, 0, nRec);
                        if (msg.Equals("false") || msg.Equals("true"))
                        {
                            canSend = bool.Parse(msg);
                            Console.WriteLine("I've received: " + msg);
                        }
                        else
                        {

                            string message = "";
                            foreach (var item in buffReceived)
                            {
                                if (item != 0)
                                {
                                    byte[] helper = new byte[1];
                                    helper[0] = item;
                                    message = message + Encoding.ASCII.GetString(helper);
                                }
                            }

                            Number = int.Parse(message);
                            Console.WriteLine("I've received: " + Number.ToString());
                        }
                    }
                }, TaskCreationOptions.LongRunning);

                receive.Start();
                send.Start();
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                if (client != null)
                {
                    if (client.Connected)
                    {
                        client.Shutdown(SocketShutdown.Both);
                        client.Close();
                        client.Dispose();
                    }
                }
            }
        }
    }
}
