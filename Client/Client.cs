using Microsoft.Toolkit.Mvvm.Messaging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static System.Net.Mime.MediaTypeNames;

namespace Client
{
    public class Client
    {
        public int ClientId { get; set; }
        public bool CanSend { get; set; }
        IList<string> Data;
        List<Task> Tasks;
        Socket MySocket;
        public void Init()
        {
            Tasks = new List<Task>();
            MySocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
        public void Connect(string ip = "26.99.118.45")
        {
            MySocket.Connect(IPAddress.Parse(ip), 10000);

            byte[] id = new byte[1];
            MySocket.Receive(id);
            ClientId = int.Parse(Encoding.ASCII.GetString(id));

            Task send = new Task(() => { DataSend(); }, TaskCreationOptions.LongRunning);

            Task receive = new Task(() => { DataReceive(); }, TaskCreationOptions.LongRunning);

            Tasks.Add(send);
            Tasks.Add(receive);

            receive.Start();
            send.Start();
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
        public void DataSend(int packetSpeed = 100)
        {
            while (MySocket.Connected)
            {
                if (CanSend)
                {
                    string json = JsonConvert.SerializeObject(Data);
                    MySocket.Send(Encoding.ASCII.GetBytes(json));
                }
                System.Threading.Thread.Sleep(packetSpeed);//10packets/sec
            }
        }
        public void DataReceive()
        {
            while (MySocket.Connected)
            {
                byte[] buffer = new byte[2048];
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

                if (message.Equals("false") || message.Equals("true"))
                {
                    CanSend = bool.Parse(message);
                }
                else
                {
                    IList<string> helper = JsonConvert.DeserializeObject<IList<string>>(message);
                    if (helper != null)
                    {
                        Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => Data.Clear()));
                        foreach (var item in helper)
                        {
                            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => Data.Add(item)));
                            //cross thread exception miatt meg kell hivni az ui main threadjet
                        }
                    }
                }
            }
        }
        public void Skip()
        {
            //if (CanSend)
            //{
            //    MySocket.Send(Encoding.ASCII.GetBytes("skip"));
            //}
            //Data.Add("aasd");
        }
    }
}
