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
        public object TradeView { get; set; }
        public object MysteryView { get; set; }
        public object MysteryHeroView { get; set; }
        public object GoldMineView { get; set; }
        public object BattleView { get; set; }
        public object ESCView { get; set; }
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
                            if (message.Contains("win"))
                            {
                                //aki ilyet kap az vesztett.
                            }

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
        public void ChooseOffer()
        {
            gameLogic.MakeTrade();
            TradeViewChange("");
        }
        public void MysteryButtonOK()
        {
            MysteryViewChange("");
            MysteryHeroViewChange("");
        }
        public void BattleViewChange(string view)
        {
            if (view.Equals("battle"))
            {
                BattleView = new BattleUC();
            }
            else
            {
                BattleView = null;
            }
            messenger.Send("Message", "Base");
        }
        public void TradeViewChange(string view)
        {

            if (view.Equals("trade"))
            {
                TradeView = new TradeUC();
            }
            else
            {
                TradeView = null;
            }
            messenger.Send("Message", "Base");
        }
        public void ESCChange(string view)
        {

            if (view.Equals("ESC"))
            {
                ESCView = new GameSubMenuUC();
            }
            else
            {
                ESCView = null;
            }
            messenger.Send("Message", "Base");
        }
        public void MysteryViewChange(string view)
        {

            if (view.Equals("mystery"))
            {
                MysteryView = new MysteryUC();
            }
            else
            {
                MysteryView = null;
            }
            messenger.Send("Message", "Base");
        }
        public void MysteryHeroViewChange(string view)
        {

            if (view.Equals("mysteryHero"))
            {
                MysteryHeroView = new MysteryHeroUC();
            }
            else
            {
                MysteryHeroView = null;
            }
            messenger.Send("Message", "Base");
        }
        public void GoldMineViewChange(string view)
        {
            if (view.Equals("goldmine"))
            {
                GoldMineView = new GoldMineUC();
            }
            else
            {
                GoldMineView = null;
            }
            messenger.Send("Message", "Base");
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
            else if (view.Equals("goldmine"))
            {
                View = new GoldMineUC();
            }
            else if (view.Equals("ending"))
            {
                View = new GameEndUC();
            }
            else if (view.Equals("lobby") && socketClient.ClientId != 0)
            {
                View = new LobbyUC();
            }
            messenger.Send("Message", "Base");
        }
        public void EnterGoldMine()
        {
            GoldMineViewChange("goldmine");
        }
        public void IsAllQuestsDone()
        {
            var player = gameLogic.Players.Where(t => t.PlayerID == ClientId).FirstOrDefault();
            if (player.Quests.All(x => x.Done))
            {
                gameLogic.SetGameEndOrder();
                ChangeView("ending");
            }
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
                    int defaultMove = 0;
                    if (faction == Faction.Mongolian)
                    {
                        defaultMove = 3;
                    }
                    else
                    {
                        defaultMove = 2;
                    }
                    Player player = new Player()
                    {
                        PlayerID = ClientId,
                        Name = name,
                        Faction = faction,
                        DefaultNumOfMoves = defaultMove,
                        RemainingMoves = defaultMove,
                        Quests = gameLogic.RandomQuestSelector(3),
                        Units = new List<Unit>(),
                        Villages = new List<Village>(),
                        Trade = null,
                        HasEnteredGoldMine = false
                    };
                    player.SetupGold(20);
                    //player.SetupGold(RandomNumber.RandomNumberGenerator(2, 5));
                    //player.SetupArmyPower(RandomNumber.RandomNumberGenerator(0, 3));
                    //player.SetupPopulatiry(RandomNumber.RandomNumberGenerator(0, 3));
                    player.SetupArmyPower(20);
                    player.SetupPopulatiry(20);
                    player.SetupStone(30);
                    player.SetupWood(30);
                    player.SetupWheat(10);
                    gameLogic.Players.Add(player);

                }
                Village village = null;
                Unit unit = null;
                if (gameLogic.Players.Count == 1)
                {
                    village = new Village() { Position = new int[] { 2, 2 }, FactionType = faction, CanMove = false, Level = 1, OwnerId = gameLogic.ClientID };
                    unit = new Unit() { CanMove = true, FactionType = faction, OwnerId = gameLogic.ClientID, Position = new int[] { 2, 2 } };

                }
                else if (gameLogic.Players.Count == 2)
                {
                    village = new Village() { Position = new int[] { 8, 18 }, FactionType = faction, CanMove = false, Level = 1, OwnerId = gameLogic.ClientID };
                    unit = new Unit() { CanMove = true, FactionType = faction, OwnerId = gameLogic.ClientID, Position = new int[] { 8, 18 } };
                }
                else if (gameLogic.Players.Count == 3)
                {
                    village = new Village() { Position = new int[] { 2, 18 }, FactionType = faction, CanMove = false, Level = 1, OwnerId = gameLogic.ClientID };
                    unit = new Unit() { CanMove = true, FactionType = faction, OwnerId = gameLogic.ClientID, Position = new int[] { 2, 18 } };
                }
                else if (gameLogic.Players.Count == 4)
                {
                    village = new Village() { Position = new int[] { 8, 2 }, FactionType = faction, CanMove = false, Level = 1, OwnerId = gameLogic.ClientID };
                    unit = new Unit() { CanMove = true, FactionType = faction, OwnerId = gameLogic.ClientID, Position = new int[] { 8, 2 } };
                }
                gameLogic.Players.Where(x => x.PlayerID == ClientId).FirstOrDefault().Villages.Add(village);
                gameLogic.Players.Where(x => x.PlayerID == ClientId).FirstOrDefault().Units.Add(unit);
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
