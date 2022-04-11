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
        IGameLogic gameLogic;
        IMessenger messenger;
        public object View { get; set; }
        public bool CanSend { get; set; }
        public int ClientId { get; set; }
        public int Timer { get; set; }
        SocketClient.SocketClient socketClient;
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
                gameLogic.ClientID = ClientId;
                gameLogic.Map = socketClient.Map;

                Task Send = new Task(() =>
                {
                    while (socketClient.MySocket.Connected)
                    {
                        if (CanSend)
                        {
                            socketClient.DataSend(gameLogic.Players, packetSpeed: 250);
                        }
                    }
                }, TaskCreationOptions.LongRunning);

                Task Receive = new Task(() =>
                {
                    int counter = 0;
                    while (socketClient.MySocket.Connected)
                    {
                        string message = socketClient.DataReceive();
                        ;
                        if (message != null)
                        {
                            if (message.Equals("false") || message.Equals("true"))
                            {
                                CanSend = bool.Parse(message);
                                if (CanSend)
                                {
                                    gameLogic.ResetMoves();
                                }

                                if (counter < 1 && message.Equals("true"))
                                {
                                    counter++;
                                    foreach (var item in gameLogic.Players)
                                    {
                                        gameLogic.AvailableFactions.Remove(item.Faction);
                                        gameLogic.AvailableFactions.Sort();
                                    }
                                }
                                messenger.Send("FactionsAdded", "Base");
                            }
                            else if (message.Equals("timer"))
                            {
                                Timer = 60;
                            }
                            else
                            {
                                try
                                {
                                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => gameLogic.Players.Clear()));
                                    foreach (var item in JsonConvert.DeserializeObject<ObservableCollection<Player>>(message))
                                    {
                                        Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => gameLogic.Players.Add(item)));
                                    }
                                }
                                catch (Exception)
                                {

                                }
                            }
                        }
                    }
                }, TaskCreationOptions.LongRunning);

                Task Update = new Task(() =>
                {
                    while (socketClient.MySocket.Connected)
                    {
                        gameLogic.ReloadHexagonObjects();
                        System.Threading.Thread.Sleep(500);
                    }
                }, TaskCreationOptions.LongRunning);

                Task Clock = new Task(() =>
                {
                    while (socketClient.MySocket.Connected)
                    {
                        Timer--;
                        messenger.Send("Message", "Base");
                        System.Threading.Thread.Sleep(1000);
                    }
                }, TaskCreationOptions.LongRunning);

                Send.Start();
                Receive.Start();
                Update.Start();
                Clock.Start();
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
            messenger.Send("Message", "Base");
        }
        public void ChampSelect(Faction faction, string name)
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
                        Quests = new List<Quest>(),
                        Units = new List<Unit>(),
                        Villages = new List<Village>()
                    });
                }
                gameLogic.GameMap = gameLogic.GameMapSetup($"Resources/Maps/map{gameLogic.Map}.txt");
                ChangeView("game");
                System.Threading.Thread.Sleep(500);
                SkipTurn();
            }
        }
        public void SkipTurn()
        {
            if (CanSend)
            {
                socketClient.Skip();
            }
        }
        public void StartServer(int turnLength = 100, int clients = 1, string map = "1", string ip = "127.0.0.1", int port = 10000, int bufferSize = 4096)
        {
            ProcessStartInfo server = new ProcessStartInfo();
            server.FileName = "SocketServer.exe";
            server.Arguments = $" {ip} {clients} {port} {map} {turnLength} {bufferSize}";
            Process.Start(server);
            ClientConnect(ip);
        }
        public void LoadGame(string save, int turnLength = 100, int clients = 1, string map = "1", string ip = "127.0.0.1")
        {
            gameLogic.Players = JsonConvert.DeserializeObject<ObservableCollection<Player>>(save.Split('@')[0]);
            StartServer(turnLength: turnLength, clients: gameLogic.Players.Count, map: save.Split('@')[1], ip: ip);
        }
    }
}
