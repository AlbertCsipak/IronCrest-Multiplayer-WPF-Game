using GUI_20212022_Z6O9JF.Models;
using GUI_20212022_Z6O9JF.UserControls;
using Microsoft.Toolkit.Mvvm.Messaging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace GUI_20212022_Z6O9JF.Logic
{
    public class ClientLogic : IClientLogic
    {
        public IGameLogic gameLogic { get; set; }
        public object View { get; set; }
        public bool CanSend { get; set; }
        public int ClientId { get; set; }

        SocketClient.SocketClient socketClient;
        IMessenger messenger;
        public ClientLogic(IMessenger messenger, IGameLogic gameLogic)
        {
            this.messenger = messenger;
            this.gameLogic = gameLogic;
            socketClient = new SocketClient.SocketClient();
        }
        public void ClientConnect(string ip)
        {
            socketClient.Connect(ip: ip);

            if (socketClient.MySocket != null)
            {
                ClientId = socketClient.ClientId;
                gameLogic.Map = socketClient.Map;

                Task Send = new Task(() =>
                {
                    while (socketClient.MySocket.Connected)
                    {
                        if (CanSend)
                        {
                            socketClient.DataSend(gameLogic.Players, packetSpeed: 500);
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
                                if (counter < 1 && message.Equals("true"))
                                {
                                    counter++;
                                    foreach (var item in gameLogic.Players)
                                    {
                                        gameLogic.AvailableFactions.Remove(item.Faction);
                                        gameLogic.AvailableFactions.Sort();
                                        messenger.Send("FactionRemoved", "Base");
                                    }
                                }
                            }
                            else
                            {
                                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => gameLogic.Players.Clear()));
                                foreach (var item in JsonConvert.DeserializeObject<ObservableCollection<Player>>(message))
                                {
                                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => gameLogic.Players.Add(item)));
                                }
                            }
                        }
                    }
                }, TaskCreationOptions.LongRunning);

                Send.Start();
                Receive.Start();
                ChangeView("lobby");
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
                View = new ServerUC();
            }
            else if (view.Equals("lobby") && socketClient.ClientId != 0)
            {
                View = new LobbyUC();
            }
            messenger.Send("ViewChanged", "Base");
        }
        public void ChampSelect(Faction faction, string name = "Anon")
        {
            if (CanSend)
            {
                if (gameLogic.Players.Any(t => t.Faction == faction))
                {
                    gameLogic.Players.Where(t => t.Faction == faction).Select(t => t.Name == name);
                    gameLogic.Players.Where(t => t.Faction == faction).Select(t => t.PlayerID == ClientId);
                }
                else
                {
                    gameLogic.Players.Add(new Player()
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
            ChangeView("menu");
        }
        public void LoadGame(string save, int turnLength = 100, int clients = 1, string map = "1", string ip = "127.0.0.1")
        {
            gameLogic.Players = JsonConvert.DeserializeObject<ObservableCollection<Player>>(save.Split('@')[0]);
            StartServer(turnLength: turnLength, clients: gameLogic.Players.Count, map: save.Split('@')[1], ip: ip);
        }
    }
}
