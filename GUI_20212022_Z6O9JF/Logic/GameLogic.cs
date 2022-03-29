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
using SocketClient;

namespace GUI_20212022_Z6O9JF.Logic
{
    public class GameLogic : IGameLogic
    {
        int ClientId { get; set; }
        bool CanSend { get; set; }
        ObservableCollection<string> Data;
        SocketClient<string> socketClient;
        IMessenger Messenger;
        public GameLogic(IMessenger messenger)
        {
            socketClient = new SocketClient<string>();
            this.Messenger = messenger;
            this.Data = new ObservableCollection<string>();
        }
        public ObservableCollection<string> Setup()
        {
            return Data;
        }
        public void ClientSetup()
        {
            socketClient.Connect();
            this.ClientId = socketClient.ClientId;

            Task Send = new Task(() =>
            {
                while (socketClient.MySocket.Connected)
                {
                    if (CanSend)
                    {
                        socketClient.DataSend(Data);
                        ;
                    }
                }
            }, TaskCreationOptions.LongRunning);

            Task Receive = new Task(() =>
            {
                while (socketClient.MySocket.Connected)
                {
                    string message = socketClient.DataReceive();
                    if (message.Equals("false") || message.Equals("true"))
                    {
                        CanSend = bool.Parse(message);
                        ;
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
                                ;
                            }
                        }
                    }
                }
            }, TaskCreationOptions.LongRunning);

            Send.Start();
            Receive.Start();
        }
        public void Skip() { socketClient.Skip(); }
        public void Red() { Data.Add("red"); }
    }
}
