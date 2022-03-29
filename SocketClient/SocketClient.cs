using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SocketClient
{
    public class SocketClient<T>
    {
        public int ClientId { get; set; }
        public Socket MySocket;
        public SocketClient() { }
        public void Connect(string ip = "26.99.118.45")
        {
            MySocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            MySocket.Connect(IPAddress.Parse(ip), 10000);

            byte[] id = new byte[1];
            MySocket.Receive(id);
            ClientId = int.Parse(Encoding.ASCII.GetString(id));
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
        public void DataSend(IList<T> vs, int packetSpeed = 100)
        {
            string json = JsonConvert.SerializeObject(vs);
            MySocket.Send(Encoding.ASCII.GetBytes(json));
            System.Threading.Thread.Sleep(packetSpeed);//10packets/sec
        }
        public string DataReceive(int bufferSize=2048)
        {
            byte[] buffer = new byte[bufferSize];
            MySocket.Receive(buffer);

            string message = "";
            int idx = 0;
            while (buffer[idx] != 0)
            {
                byte[] helper = new byte[1];
                helper[0] = buffer[idx];
                message = message + Encoding.ASCII.GetString(helper);
                idx++;
            }
            return message;
        }
        public void Skip()
        {
            MySocket.Send(Encoding.ASCII.GetBytes("skip"));
        }

    }
}

