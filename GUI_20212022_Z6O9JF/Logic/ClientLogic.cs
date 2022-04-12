using GUI_20212022_Z6O9JF.Logic;
using GUI_20212022_Z6O9JF.Models;
using GUI_20212022_Z6O9JF.UserControls;
using Microsoft.Toolkit.Mvvm.Messaging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace GUI_20212022_Z6O9JF.Logic
{
    public static class RandomNumber
    {
        private static readonly RNGCryptoServiceProvider _generator = new RNGCryptoServiceProvider();

        public static int RandomNumberGenerator(int minimumValue, int maximumValue)
        {
            byte[] randomNumber = new byte[1];

            _generator.GetBytes(randomNumber);

            double asciiValueOfRandomCharacter = Convert.ToDouble(randomNumber[0]);

            // We are using Math.Max, and substracting 0.00000000001, 
            // to ensure "multiplier" will always be between 0.0 and .99999999999
            // Otherwise, it's possible for it to be "1", which causes problems in our rounding.
            double multiplier = Math.Max(0, (asciiValueOfRandomCharacter / 255d) - 0.00000000001d);

            // We need to add one to the range, to allow for the rounding done with Math.Floor
            int range = maximumValue - minimumValue + 1;

            double randomValueInRange = Math.Floor(multiplier * range);

            return (int)(minimumValue + randomValueInRange);
        }
    }

    public class ClientLogic : IClientLogic
    {
        //public static Random r = new Random();
        public List<Quest> quests;
        IGameLogic gameLogic;
        IMessenger messenger;
        public object View { get; set; }
        public bool CanSend { get; set; }
        public int ClientId { get; set; }
        public int Timer { get; set; }
        int tmpTimer;
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
                            tmpTimer++;
                            if (tmpTimer == 4)
                            {
                                Timer--;
                                tmpTimer = 0;
                            }
                            socketClient.DataSend(gameLogic.Players, packetSpeed: 250);
                        }
                    }
                }, TaskCreationOptions.LongRunning);

                Task Receive = new Task(() =>
                {
                    int counter = 0;
                    while (socketClient.MySocket.Connected)
                    {
                        tmpTimer++;
                        if (tmpTimer == 4)
                        {
                            Timer--;
                            tmpTimer = 0;
                        }
                        string message = socketClient.DataReceive();
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
                                        messenger.Send("FactionsAdded", "Base");
                                    }
                                }

                            }
                            else if (message.Equals("timer"))
                            {
                                tmpTimer = 0;
                                Timer = 60;
                            }
                            else
                            {
                                try
                                {
                                    ObservableCollection<Player> players = JsonConvert.DeserializeObject<ObservableCollection<Player>>(message);
                                    if (players != null)
                                    {
                                        Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => gameLogic.Players.Clear()));
                                        foreach (var item in players)
                                        {
                                            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => gameLogic.Players.Add(item)));
                                        }
                                    }
                                }
                                catch (NullReferenceException) { }
                                catch (Exception) { }
                            }
                        }
                    }
                }, TaskCreationOptions.LongRunning);

                Task Update = new Task(() =>
                {
                    while (socketClient.MySocket.Connected)
                    {
                        gameLogic.ReloadHexagonObjects();
                        messenger.Send("Message", "Base");
                        System.Threading.Thread.Sleep(250);
                    }
                }, TaskCreationOptions.LongRunning);

                Send.Start();
                Receive.Start();
                Update.Start();
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
                    Quests = RandomQuestSelector(3),
                    Units = new List<Unit>(),
                    Villages = new List<Village>(),
                    Gold = RandomNumber.RandomNumberGenerator(0, 5),
                    Popularity = RandomNumber.RandomNumberGenerator(0, 3),
                    ArmyPower = RandomNumber.RandomNumberGenerator(0, 3)
                });
                }
                gameLogic.GameMap = gameLogic.GameMapSetup($"Resources/Maps/map{gameLogic.Map}.txt");
                ChangeView("game");
                System.Threading.Thread.Sleep(500);
                SkipTurn();
            }
        }

        public void ReadQuests()
        {
            var file = File.ReadAllLines("Resources/Maps/Quests.txt");
            quests = new List<Quest>();
            foreach (var item in file)
            {
                string[] line = item.Split(';');
                quests.Add(new Quest(line[0], line[1] == "false" ? false : true));
            }
        }
        public List<Quest> RandomQuestSelector(int n)
        {
            ReadQuests();
            List<Quest> curr_quests = new List<Quest>();
            List<int> indexes=new List<int>();
            int db = 0;
            while (db!=n)
            {
            int ind = RandomNumber.RandomNumberGenerator(0, quests.Count - 1);
                if (!indexes.Contains(ind))
                {
                    indexes.Add(ind);
                    db++;
                    curr_quests.Add(quests.ElementAt(ind));
                }
            }
            return curr_quests;
        }
        public void SkipTurn()
        {
            if (CanSend)
            {
                socketClient.Skip();
            }
        }
        public void StartServer(int turnLength = 100, int clients = 1, string map = "1", string ip = "127.0.0.1", int port = 10000, int bufferSize = 8192)
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
