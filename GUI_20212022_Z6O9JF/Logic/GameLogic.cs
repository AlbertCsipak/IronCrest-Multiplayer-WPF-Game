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
using System.Windows.Threading;

namespace GUI_20212022_Z6O9JF.Logic
{
    public class GameLogic : IGameLogic
    {
        public int ClientId { get; set; }
        public bool CanSend { get; set; }
        ObservableCollection<string> Data;
        List<Task> Tasks;
        Socket MySocket;
        IMessenger Messenger;
        public GameLogic(IMessenger messenger)
        {
            Data = new ObservableCollection<string>();
            this.Messenger = messenger;
        }
        public ObservableCollection<string> Setup() { return this.Data; }
        public void Red() { Data.Add("red"); }
        public void Blue() { Data.Add("blue"); }
        public void Skip()
        {
            if (CanSend)
            {
                MySocket.Send(Encoding.ASCII.GetBytes("skip"));
            }
        }
        public void Connect(string ip = "26.99.118.45")
        {
            MySocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            MySocket.Connect(IPAddress.Parse(ip), 10000);
            Tasks = new List<Task>();

            byte[] id = new byte[1];
            MySocket.Receive(id);
            ClientId = int.Parse(Encoding.ASCII.GetString(id));

            Task send = new Task(() =>
            {
                while (MySocket.Connected)
                {
                    if (CanSend)
                    {
                        string json = JsonConvert.SerializeObject(Data);
                        MySocket.Send(Encoding.ASCII.GetBytes(json));
                    }
                    System.Threading.Thread.Sleep(100);//10packets/sec
                }
            }, TaskCreationOptions.LongRunning);

            Task receive = new Task(() =>
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
                        ObservableCollection<string> helper = JsonConvert.DeserializeObject<ObservableCollection<string>>(message);
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
            }, TaskCreationOptions.LongRunning);

            Tasks.Add(send);
            Tasks.Add(receive);

            receive.Start();
            send.Start();
        }

    }
}
