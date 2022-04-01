using GUI_20212022_Z6O9JF.Models;
using GUI_20212022_Z6O9JF.UserControls;
using Microsoft.Toolkit.Mvvm.Messaging;
using Newtonsoft.Json;
using Server;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace GUI_20212022_Z6O9JF.Logic
{
    public class GameLogic : IGameLogic
    {
        public object View { get; set; }
        public string Map { get; set; }
        public bool CanSend { get; set; }
        public int ClientId { get; set; }
        public FieldType[,] GameMap { get; set; }
        public ObservableCollection<Player> Players { get; set; }
        public List<Faction> AvailableFactions { get; set; }
        public enum FieldType { field, water, village, hill, forest, wheat }
        SocketClient.SocketClient socketClient;
        IMessenger messenger;
        public GameLogic(IMessenger messenger)
        {
            this.messenger = messenger;
            socketClient = new SocketClient.SocketClient();
            this.Players = new ObservableCollection<Player>();

            AvailableFactions = new List<Faction>();
            AvailableFactions.Add(Faction.Viking);
            AvailableFactions.Add(Faction.Crusader);
            AvailableFactions.Add(Faction.Arabian);
            AvailableFactions.Add(Faction.Mongolian);
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
                        case 'm':
                            map[i, j] = FieldType.field;
                            break;
                        case 'v':
                            map[i, j] = FieldType.water;
                            break;
                        case 'e':
                            map[i, j] = FieldType.forest;
                            break;
                        case 'h':
                            map[i, j] = FieldType.hill;
                            break;
                        case 'b':
                            map[i, j] = FieldType.wheat;
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

            if (socketClient.MySocket!=null)
            {
                ClientId = socketClient.ClientId;
                Map = socketClient.Map;

                Task Send = new Task(() =>
                {
                    while (socketClient.MySocket.Connected)
                    {
                        if (CanSend)
                        {
                            socketClient.DataSend(Players, packetSpeed: 500);
                        }
                    }
                }, TaskCreationOptions.LongRunning);

                Task Receive = new Task(() =>
                {
                    int counter = 0;
                    while (socketClient.MySocket.Connected)
                    {
                        string message = socketClient.DataReceive();
                        if (message != null)
                        {
                            if (message.Equals("false") || message.Equals("true"))
                            {
                                CanSend = bool.Parse(message);
                                if (counter < 4 && message.Equals("true"))
                                {
                                    counter++;
                                    foreach (var item in Players)
                                    {
                                        AvailableFactions.Remove(item.Faction);
                                        AvailableFactions.Sort();
                                        messenger.Send("FactionRemoved", "Base");
                                    }
                                }
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
                ChangeView("join");
            }
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
            else if (view.Equals("server"))
            {
                View = new ServerStartUC();
            }
            else if (view.Equals("join") && socketClient.ClientId!=0)
            {
                View = new JoinGameUC();
            }
            messenger.Send("ViewChanged", "Base");
        }
        public void ChampSelect(Faction faction, string name = "Anon")
        {
            if (CanSend)
            {
                if (Players.Any(t => t.Faction == faction))
                {
                    Players.Where(t => t.Faction == faction).Select(t => t.Name == name);
                }
                else
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
                        Food = 0,
                        Gold = 0,
                        Stone = 0,
                        Wood = 0
                    });
                }
                ChangeView("game");
                System.Threading.Thread.Sleep(600);
                socketClient.Skip();
            }
        }
        public void StartServer(int turnLength = 100, int clients = 1, string map = "1", string ip = "127.0.0.1", int port = 10000, int bufferSize = 2048)
        {
            ProcessStartInfo server = new ProcessStartInfo();
            server.FileName = "SocketServer.exe";
            server.Arguments = $" {ip} {clients} {port} {map} {turnLength} {bufferSize}";
            Process.Start(server);
        }
        public void LoadGame(ObservableCollection<Player> vs, int turnLength = 100, int clients = 1, string map = "1", string ip = "127.0.0.1")
        {
            Players = vs;
            StartServer(turnLength: turnLength, clients: clients, map: map, ip: ip);
        }
    }
}
