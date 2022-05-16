using GUI_20212022_Z6O9JF.Models;
using GUI_20212022_Z6O9JF.UserControls;
using Microsoft.Toolkit.Mvvm.Messaging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace GUI_20212022_Z6O9JF.Logic
{
    public class LastClientEventArgs : EventArgs
    {
        public int lastClientId { get; set; }
        public bool CanSendBool { get; set; }
    }

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
        public object GameEndView { get; set; }
        public static EventHandler StartOfTurnEvent;
        public static EventHandler EndOfTurnEvent;
        LastClientEventArgs e = new LastClientEventArgs();
        private bool canSend;
        public bool CanSend
        {
            get { return canSend; }
            set
            {
                bool lastValue = canSend;
                canSend = value;
                if (canSend && !lastValue)
                {
                    StartOfTurnEvent?.Invoke(this, EventArgs.Empty);
                    YourTurn();
                }
                if (!CanSend && lastValue)
                {
                    e.lastClientId = this.ClientId;
                    e.CanSendBool = canSend;
                    EndOfTurnEvent?.Invoke(this, e);
                }
            }
        }

        public int ClientId { get; set; }
        public int Timer { get; set; }
        public bool inBattle { get; set; }
        int tmpTimer;
        SocketClient.SocketClient socketClient;
        public ClientLogic(IMessenger messenger, IGameLogic gameLogic)
        {
            this.messenger = messenger;
            this.gameLogic = gameLogic;
            socketClient = new SocketClient.SocketClient();
        }
        public void YourTurn()
        {
            if (gameLogic.Game.CurrentGoldMineOwner!=null)
            {
                gameLogic.AddGold();
            }
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
                            socketClient.DataSend(gameLogic.Game, packetSpeed: 250);
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
                                    foreach (var item in gameLogic.Game.Players)
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
                                    Game game = JsonConvert.DeserializeObject<Game>(message);
                                    ;
                                    if (game != null)
                                    {
                                        gameLogic.Game = game;
                                    }
                                }
                                catch (NullReferenceException) { }
                                catch (Exception) { }
                                ;
                            }
                        }
                    }
                }, TaskCreationOptions.LongRunning);

                Task Update = new Task(() =>
                {
                    while (socketClient.MySocket.Connected)
                    {
                        gameLogic.ReloadHexagonObjects();
                        ;
                        if (gameLogic.Game.CurrentBattle != null)
                        {
                            ;
                            if (!inBattle && gameLogic.Game.CurrentBattle.Defender.PlayerID == ClientId && gameLogic.Game.CurrentBattle.IsBattleStarted)
                            {
                                ;
                                inBattle = true;
                                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => BattleViewChange("battle")));
                            }
                        }
                        else
                        {
                            inBattle = false;
                        }
                        ;
                        messenger.Send("Message", "Base");
                        System.Threading.Thread.Sleep(500);
                    }
                }, TaskCreationOptions.LongRunning);

                Send.Start();
                Receive.Start();
                Update.Start();

                ChangeView("lobby");
            }
        }
        public void IsAllQuestsDone()
        {
            var player = gameLogic.Game.Players.Where(t => t.PlayerID == ClientId).FirstOrDefault();
            if (player.Quests.All(x => x.Done))
            {
                gameLogic.SetGameEndOrder();
                ChangeView("ending");
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
                if (gameLogic.Game.CurrentBattle != null)
                {
                    gameLogic.Game.CurrentBattle = null;
                }
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

        public void ChampSelect(Faction faction, string name)
        {
            if (CanSend)
            {
                if (gameLogic.Game.Players.Any(t => t.Faction == faction))
                {
                    gameLogic.Game.Players.Where(t => t.Faction == faction).Select(t => t.Name == name);
                    gameLogic.Game.Players.Where(t => t.Faction == faction).Select(t => t.PlayerID == ClientId);
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
                    //player.SetupGold(RandomNumber.RandomNumberGenerator(2, 5));
                    player.SetupGold(30);
                    //player.SetupArmyPower(RandomNumber.RandomNumberGenerator(0, 3));
                    //player.SetupPopulatiry(RandomNumber.RandomNumberGenerator(0, 3));
                    //player.SetupGold(20);
                    player.SetupArmyPower(20);
                    player.SetupPopulatiry(20);
                    player.SetupStone(20);
                    player.SetupWood(20);
                    player.SetupWheat(20);
                    gameLogic.Game.Players.Add(player);

                }
                Village village = null;
                Unit unit = null;
                if (gameLogic.Game.Players.Count == 1)
                {
                    village = new Village() { Position = new int[] { 2, 2 }, FactionType = faction, CanMove = false, Level = 1, OwnerId = gameLogic.ClientID, IsBase=true };
                    unit = new Unit() { CanMove = true, FactionType = faction, OwnerId = gameLogic.ClientID, Position = new int[] { 2, 2 } };

                }
                else if (gameLogic.Game.Players.Count == 2)
                {
                    village = new Village() { Position = new int[] { 8, 18 }, FactionType = faction, CanMove = false, Level = 1, OwnerId = gameLogic.ClientID, IsBase = true };
                    unit = new Unit() { CanMove = true, FactionType = faction, OwnerId = gameLogic.ClientID, Position = new int[] { 8, 18 } };
                }
                else if (gameLogic.Game.Players.Count == 3)
                {
                    village = new Village() { Position = new int[] { 2, 18 }, FactionType = faction, CanMove = false, Level = 1, OwnerId = gameLogic.ClientID, IsBase = true };
                    unit = new Unit() { CanMove = true, FactionType = faction, OwnerId = gameLogic.ClientID, Position = new int[] { 2, 18 } };
                }
                else if (gameLogic.Game.Players.Count == 4)
                {
                    village = new Village() { Position = new int[] { 8, 2 }, FactionType = faction, CanMove = false, Level = 1, OwnerId = gameLogic.ClientID, IsBase = true };
                    unit = new Unit() { CanMove = true, FactionType = faction, OwnerId = gameLogic.ClientID, Position = new int[] { 8, 2 } };
                }
                gameLogic.Game.Players.Where(x => x.PlayerID == ClientId).FirstOrDefault().Villages.Add(village);
                gameLogic.Game.Players.Where(x => x.PlayerID == ClientId).FirstOrDefault().Units.Add(unit);
                gameLogic.GameMap = gameLogic.GameMapSetup($"Resources/Maps/map{gameLogic.Map}.txt");
                ChangeView("game");
                System.Threading.Thread.Sleep(300);
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
            server.CreateNoWindow = false;
            Process.Start(server);
            ClientConnect(ip);
        }
        public void LoadGame(string save, int turnLength = 100, int clients = 1, string map = "1", string ip = "127.0.0.1")
        {
            gameLogic.Game = JsonConvert.DeserializeObject<Game>(save.Split('@')[0]);
            StartServer(turnLength: turnLength, clients: gameLogic.Game.Players.Count, map: save.Split('@')[1], ip: ip);
        }
    }
}
