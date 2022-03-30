using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SocketClient
{
    public class SocketClient
    {
        public int ClientId { get; set; }
        public string Map { get; set; }
        public Socket MySocket;
        public SocketClient() { }
        public void Connect(string ip = "26.99.118.45", int port = 10000)
        {
            if (MySocket is null)
            {
                MySocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                MySocket.Connect(IPAddress.Parse(ip), port);

                byte[] id = new byte[1];
                MySocket.Receive(id);
                ClientId = int.Parse(Encoding.ASCII.GetString(id));

                byte[] map = new byte[1];
                MySocket.Receive(map);
                Map = Encoding.ASCII.GetString(map);
            }
        }
        public void Disconnect()
        {
            if (MySocket != null && MySocket.Connected)
            {
                MySocket.Shutdown(SocketShutdown.Both);
                MySocket.Close();
                MySocket.Dispose();
            }
        }
        public void DataSend(object vs, int packetSpeed = 50)
        {
            string json = JsonConvert.SerializeObject(vs);
            MySocket.Send(Encoding.ASCII.GetBytes(json));
            System.Threading.Thread.Sleep(packetSpeed);//10packets/sec
        }
        public string DataReceive(int bufferSize = 2048)
        {
            byte[] buffer = new byte[bufferSize];
            try
            {
                MySocket.Receive(buffer);
            }
            catch (Exception)
            {
                Disconnect();
                return null;
            }

            string message = "";
            int idx = 0;
            while (buffer[idx] != 0)
            {
                byte[] helper = new byte[1];
                helper[0] = buffer[idx];
                message = message + Encoding.ASCII.GetString(helper);
                idx++;
                if (bufferSize-1 == idx)
                {
                    break;
                }
            }
            return message;
        }
        public void Skip()
        {
            MySocket.Send(Encoding.ASCII.GetBytes("skip"));
        }
    }
}

