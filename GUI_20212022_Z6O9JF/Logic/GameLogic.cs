using GUI_20212022_Z6O9JF.Models;
using GUI_20212022_Z6O9JF.UserControls;
using Microsoft.Toolkit.Mvvm.Messaging;
using Newtonsoft.Json;
using Server;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace GUI_20212022_Z6O9JF.Logic
{
    public class GameLogic : IGameLogic
    {
        public ObservableCollection<Player> Players { get; set; }
        public object View { get; set; }
        public string Map { get; set; }
        public enum FieldType { grass, water, village, desert, snow }
        public FieldType[,] GameMap { get; set; }
        SocketClient.SocketClient socketClient;
        bool CanSend;
        int ClientId;
        IMessenger messenger;
        public GameLogic(IMessenger messenger)
        {
            this.messenger = messenger;
            socketClient = new SocketClient.SocketClient();
            this.Players = new ObservableCollection<Player>();
        }
        public FieldType[,] GameMapSetup(string path)
        {
            string[] lines = File.ReadAllLines(path);
            ;
            FieldType[,] map = new FieldType[int.Parse(lines[0]), int.Parse(lines[1])];

            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    switch (lines[i + 2][j])
                    {
                        case 'g':
                            map[i, j] = FieldType.grass;
                            break;
                        case 'w':
                            map[i, j] = FieldType.water;
                            break;
                        case 'v':
                            map[i, j] = FieldType.village;
                            break;
                        case 'd':
                            map[i, j] = FieldType.desert;
                            break;
                        case 's':
                            map[i, j] = FieldType.snow;
                            break;
                        default:
                            break;
                    }
                }
            }

            return map;
        }
        public void ClientConnect()
        {
            socketClient.Connect();
            ClientId = socketClient.ClientId;
            Map = socketClient.Map;
            Task Send = new Task(() =>
            {
                while (socketClient.MySocket.Connected)
                {
                    if (CanSend)
                    {
                        socketClient.DataSend(Players);
                    }
                }
            }, TaskCreationOptions.LongRunning);

            Task Receive = new Task(() =>
            {
                while (socketClient.MySocket.Connected)
                {
                    string message = socketClient.DataReceive();
                    if (message != null)
                    {
                        if (message.Equals("false") || message.Equals("true"))
                        {
                            CanSend = bool.Parse(message);
                        }
                        else
                        {
                            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => Players.Clear()));
                            foreach (var item in JsonConvert.DeserializeObject<ObservableCollection<Player>>(message))
                            {
                                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => Players.Add(item)));
                                //cross thread exception miatt meg kell hivni az ui main threadjet
                            }
                        }
                    }
                }
            }, TaskCreationOptions.LongRunning);

            Send.Start();
            Receive.Start();
        }
        public void ChangeView(string view)
        {
            if (view.Equals("game"))
            {
                View = new GameUC();
            }
            else if (view.Equals("menu"))
            {
                View = new MenuUC();
            }
            messenger.Send("ViewChanged", "Base");
        }
        public void StartServer()
        {
            Task s = new Task(() => { SocketServer socketServer = new SocketServer(); }, TaskCreationOptions.LongRunning);
            s.Start();
        }
        public void ChampSelect(string name, Faction faction)
        {
            Players.Add(new Player()
            {
                PlayerID = ClientId,
                Name = name,
                Faction = faction,
                Moves = 2,
                ArmyPower = 0,
                BattlesWon = 0,
                Popularity = 0,
                GoldMine = false,
                Heroes = new List<Hero>(),
                Quests = new List<Quest>(),
                Units = new List<Unit>(),
                Villages = new List<Village>(),
                Food = 10,
                Gold = 10,
                Stone = 10,
                Wood = 10
            });
            socketClient.Skip();
        }
    }
}
