using GUI_20212022_Z6O9JF.Models;
using Microsoft.Toolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace GUI_20212022_Z6O9JF.Logic
{
    public class GameLogic : IGameLogic
    {
        IMessenger messenger;
        public int ClientID { get; set; }
        public string Map { get; set; }
        public HexagonTile SelectedHexagonTile { get; set; }
        public ObservableCollection<Player> Players { get; set; }
        public List<Faction> AvailableFactions { get; set; }
        public HexagonTile[,] GameMap { get; set; }
        public List<Quest> Quests;
        public Queue<Trade> Trades;
        public Trade CurrentTrade { get; set; }
        public Queue<MysteryEvent> MysteryEvents { get; set; }
        public MysteryEvent CurrentMystery { get; set; }

        public GameLogic(IMessenger messenger)
        {
            this.messenger = messenger;
            this.Players = new ObservableCollection<Player>();
            Trades = new Queue<Trade>();
            MysteryEvents = new Queue<MysteryEvent>();
            CurrentMystery = null;
            CurrentTrade = null;

            AvailableFactions = new List<Faction>();
        }
        public void SelectableFactions()
        {
            if (Players.Count == 0)
            {
                AvailableFactions.Add(Faction.Viking);
                AvailableFactions.Add(Faction.Crusader);
                AvailableFactions.Add(Faction.Arabian);
                AvailableFactions.Add(Faction.Mongolian);
            }
            else
            {
                foreach (var item in Players)
                {
                    AvailableFactions.Add(item.Faction);
                }
            }
            messenger.Send("FactionsAdded", "Base");
        }

        public HexagonTile[,] GameMapSetup(string path)
        {
            string[] lines = File.ReadAllLines(path);
            HexagonTile[,] map = new HexagonTile[int.Parse(lines[0]), int.Parse(lines[1])];
            Trades = LoadTrades();
            MysteryEvents = LoadMysteryEvents();
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    map[i, j] = new HexagonTile();
                    map[i, j].Position[0] = i;
                    map[i, j].Position[1] = j;
                    switch (lines[i + 2][j])
                    {
                        case 'g':
                            map[i, j].FieldType = FieldType.grass;
                            break;
                        case 'l':
                            map[i, j].FieldType = FieldType.lake;
                            break;
                        case 'f':
                            map[i, j].FieldType = FieldType.forest;
                            break;
                        case 'm':
                            map[i, j].FieldType = FieldType.mountain;
                            break;
                        case 'w':
                            map[i, j].FieldType = FieldType.wheat;
                            break;
                        case 'G':
                            map[i, j].FieldType = FieldType.goldMine;
                            break;
                        case 'o':
                            map[i, j].FieldType = FieldType.ocean;
                            break;
                        case 'c':
                            map[i, j].FieldType = FieldType.compassField;
                            map[i, j].Compass = Trades.Dequeue();
                            break;
                        default:
                            break;
                    }
                }
            }

            return map;
        }
        //Fisher–Yates shuffle
        public static void Shuffle<T>(List<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = RandomNumber.RandomNumberGenerator(0, n);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
        public Queue<Trade> LoadTrades()
        {
            List<Trade> trades = new List<Trade>()
            {
                new Trade(new Offer[3]{ new Offer("You get 3 gold.", new Dictionary<string, int>() { { "Gold", 0 } }, new Dictionary<string, int>() { { "Gold", 3 } }), new Offer("You get 2 logs and 1 wheat for 2 gold.", new Dictionary<string, int>() { { "Gold", 2 } }, new Dictionary<string, int>() { { "Wood", 2 }, { "Wheat", 1 } }), new Offer("You get 3 stones and 3 logs for 4 gold and 1 popularity.", new Dictionary<string, int>() { { "Gold", 4 }, {"Popularity",1 } }, new Dictionary<string, int>() { { "Stone", 3 }, { "Wood", 3 } })}),
                new Trade(new Offer[3]{ new Offer("You get 4 logs.", new Dictionary<string, int>() { { "Gold", 0 } }, new Dictionary<string, int>() { { "Wood", 4 } }), new Offer("You get 3 stones for 1 gold and 1 army power.", new Dictionary<string, int>() { { "Gold", 1 },{"ArmyPower",1 } }, new Dictionary<string, int>() { { "Stone", 3 } }), new Offer("You get 2 wheats, 2 logs and a popularity for 4 gold.", new Dictionary<string, int>() { { "Gold", 4 } }, new Dictionary<string, int>() { { "Wheat", 2 }, { "Wood", 2 }, {"Popularity",1 } })}),
                new Trade(new Offer[3]{ new Offer("You get 2 gold and a popularity.", new Dictionary<string, int>() { { "Gold", 0 } }, new Dictionary<string, int>() { { "Gold", 2 },{"Popularity",1 } }), new Offer("You get 2 logs and 2 army power for 3 gold.", new Dictionary<string, int>() { { "Gold", 3 }}, new Dictionary<string, int>() { { "Wood", 2 }, { "ArmyPower", 2 } }), new Offer("You get 3 logs, 2 wheat and 2 popularity for 3 gold and 2 stones.", new Dictionary<string, int>() { { "Gold", 3 },{"Stone",2 } }, new Dictionary<string, int>() { { "Wood", 3 }, { "Wheat", 2 },{"Popularity",2 } })}),
                new Trade(new Offer[3]{ new Offer("You get 4 stones.", new Dictionary<string, int>() { { "Gold", 0 } }, new Dictionary<string, int>() { { "Stone", 4 } }), new Offer("You get 2 wheats and 1 popularity for 2 gold.", new Dictionary<string, int>() { { "Gold", 2 } }, new Dictionary<string, int>() { { "Wheat", 2 }, { "Popularity", 1 } }), new Offer("You get 2 stones, 2 logs and 2 wheats for 3 gold and 2 army power.", new Dictionary<string, int>() { { "Gold", 3 },{"ArmyPower",2 } }, new Dictionary<string, int>() { { "Stone", 2 }, { "Wood", 2 }, { "Wheat", 2 } })}),
                new Trade(new Offer[3]{ new Offer("You get 2 gold and a stone.", new Dictionary<string, int>() { { "Gold", 0 } }, new Dictionary<string, int>() { { "Gold", 2 },{"Stone",1 } }), new Offer("You get 2 logs and 2 army power for 3 gold.", new Dictionary<string, int>() { { "Gold", 3 } }, new Dictionary<string, int>() { { "Wood", 2 }, { "ArmyPower", 2 } }), new Offer("You get 4 wheats and 2 stones for 3 gold and a log.", new Dictionary<string, int>() { { "Gold", 3 },{"Wood",1 } }, new Dictionary<string, int>() { { "Wheat", 4 }, { "Stone", 2 } })}),
                new Trade(new Offer[3]{ new Offer("You get 3 gold and a popularity.", new Dictionary<string, int>() { { "Gold", 0 } }, new Dictionary<string, int>() { { "Gold", 3 },{"Popularity",1 } }), new Offer("You get 3 stones and 1 popularity for 2 gold and a wheat.", new Dictionary<string, int>() { { "Gold", 2 },{"Wheat",1 } }, new Dictionary<string, int>() { { "Stone", 3 }, { "Popularity", 1 } }), new Offer("You get 3 army power, 2 logs and 1 popularity for 4 gold.", new Dictionary<string, int>() { { "Gold", 4 } }, new Dictionary<string, int>() { { "ArmyPower", 3 }, { "Wood", 2 },{"Popularity",1 } })}),
                new Trade(new Offer[3]{ new Offer("You get 2 gold and 2 logs.", new Dictionary<string, int>() { { "Gold", 0 } }, new Dictionary<string, int>() { { "Gold", 2 },{"Wood",2 } }), new Offer("You get 2 wheats, 1 log and 1 army power for 2 gold and a popularity.", new Dictionary<string, int>() { { "Gold", 2 },{"Popularity",1} }, new Dictionary<string, int>() { { "Wheat", 2 }, { "Wood", 1 },{"ArmyPower",1 } }), new Offer("You get 5 stones and a wheat for 3 gold and 2 army power.", new Dictionary<string, int>() { { "Gold", 3 },{"ArmyPower",2 } }, new Dictionary<string, int>() { { "Stone", 5 }, { "Wheat", 1 } })}),
                new Trade(new Offer[3]{ new Offer("You get 4 gold.", new Dictionary<string, int>() { { "Gold", 0 } }, new Dictionary<string, int>() { { "Gold", 4 } }), new Offer("You get 2 wheats and 2 army power for 3 gold.", new Dictionary<string, int>() { { "Gold", 3 } }, new Dictionary<string, int>() { { "Wheat", 2 }, { "ArmyPower", 2 } }), new Offer("You get 3 logs, 2 stones and 2 popularity for 5 gold.", new Dictionary<string, int>() { { "Gold", 5 } }, new Dictionary<string, int>() { { "Wood", 3 }, { "Stone", 2 },{"Popularity",2 } })}),
                new Trade(new Offer[3]{ new Offer("You get 1 gold, 1 army power and 1 popularity.", new Dictionary<string, int>() { { "Gold", 0 } }, new Dictionary<string, int>() { { "Gold", 1 }, { "ArmyPower", 1 }, { "Popularity", 1 } }), new Offer("You get 3 logs and 2 army power for 2 gold and 1 popularity.", new Dictionary<string, int>() { { "Gold", 2 },{"Popularity",1 } }, new Dictionary<string, int>() { { "Wood", 3 }, { "ArmyPower", 2 } }), new Offer("You get 3 popularity and 3 wheats for 3 gold and 1 log.", new Dictionary<string, int>() { { "Gold", 3 },{"Wood",1 } }, new Dictionary<string, int>() { { "Popularity", 3 }, { "Wheat", 3 } })}),
                new Trade(new Offer[3]{ new Offer("You get 2 gold, 1 stone, and a wheat.", new Dictionary<string, int>() { { "Gold", 0 } }, new Dictionary<string, int>() { { "Gold", 2 },{"Stone",1 },{"Wheat",1 } }), new Offer("You get 3 logs and 1 army power for 2 gold and a popularity.", new Dictionary<string, int>() { { "Gold", 2 },{"Popularity",1 } }, new Dictionary<string, int>() { { "Wood", 2 }, { "Wheat", 1 } }), new Offer("You get 4 logs and 2 wheats for 3 gold and 1 army power.", new Dictionary<string, int>() { { "Gold", 3 },{"ArmyPower",1 } }, new Dictionary<string, int>() { { "Wood", 4 }, { "Wheat", 2 } })}),
                new Trade(new Offer[3]{ new Offer("You get 3 gold and a wheat.", new Dictionary<string, int>() { { "Gold", 0 } }, new Dictionary<string, int>() { { "Gold", 3 },{"Wheat",1 } }), new Offer("You get 3 logs and 1 stone for 1 gold and 2 popularity.", new Dictionary<string, int>() { { "Gold", 1 },{"Popularity",2 } }, new Dictionary<string, int>() { { "Wood", 3 }, { "Stone", 1 } }), new Offer("You get 3 stones, 2 logs, and 2 army power for 3 gold and 2 wheat.", new Dictionary<string, int>() { { "Gold", 3 },{"Wheat",2 } }, new Dictionary<string, int>() { { "Stone", 3 }, { "ArmyPower", 2 } })}),
                new Trade(new Offer[3]{ new Offer("You get 1 gold and 2 army power.", new Dictionary<string, int>() { { "Gold", 0 } }, new Dictionary<string, int>() { { "Gold", 1 },{"ArmyPower",2 } }), new Offer("You get 3 logs and 1 popularity for 2 gold.", new Dictionary<string, int>() { { "Gold", 2 } }, new Dictionary<string, int>() { { "Wood", 3 }, { "Popularity", 1 } }), new Offer("You get 2 stones, 2 logs, 1 popularity and 1 army power for 4 gold.", new Dictionary<string, int>() { { "Gold", 4 } }, new Dictionary<string, int>() { { "Stone", 2 }, { "Wood", 2 },{"Popularity",1 },{"ArmyPower",1 } })})
            };
            Shuffle(trades);
            Queue<Trade> tradeQueue = new Queue<Trade>();
            trades.ForEach(x => tradeQueue.Enqueue(x));
            return tradeQueue;
        }

        public bool HasSufficientResources(int offerindex)
        {
            bool HasEnoughResources = true;
            int counter = 0;
            while (counter < Players.Where(t => t.PlayerID == ClientID).FirstOrDefault().Trade.Offers[offerindex].Cost.Count && HasEnoughResources)
            {
                var cost = Players.Where(t => t.PlayerID == ClientID).FirstOrDefault().Trade.Offers[offerindex].Cost.ElementAt(counter);
                Player player = Players.Where(t => t.PlayerID == ClientID).FirstOrDefault();
                switch (cost.Key)
                {
                    case "Gold":
                        if (player.Gold < cost.Value)
                        {
                            HasEnoughResources = false;
                        }
                        break;
                    case "ArmyPower":
                        if (player.ArmyPower < cost.Value)
                        {
                            HasEnoughResources = false;
                        }
                        break;
                    case "Popularity":
                        if (player.Popularity < cost.Value)
                        {
                            HasEnoughResources = false;
                        }
                        break;
                    case "Stone":
                        if (player.Stone < cost.Value)
                        {
                            HasEnoughResources = false;
                        }
                        break;
                    case "Wood":
                        if (player.Wood < cost.Value)
                        {
                            HasEnoughResources = false;
                        }
                        break;
                    case "Wheat":
                        if (player.Wheat < cost.Value)
                        {
                            HasEnoughResources = false;
                        }
                        break;
                    default:
                        break;
                }
                counter++;
            }
            return HasEnoughResources;
        }
        public bool HasSufficientResources(string resource, int cost)
        {
            bool HasEnoughResources = true;
            Player player = Players.Where(t => t.PlayerID == ClientID).FirstOrDefault();
            switch (resource)
            {
                case "Gold":
                    if (player.Gold < cost)
                    {
                        HasEnoughResources = false;
                        //NotEnoughResource event meghivasa
                    }
                    break;
                case "ArmyPower":
                    if (player.ArmyPower < cost)
                    {
                        HasEnoughResources = false;
                    }
                    break;
                case "Popularity":
                    if (player.Popularity < cost)
                    {
                        HasEnoughResources = false;
                    }
                    break;
                case "Stone":
                    if (player.Stone < cost)
                    {
                        HasEnoughResources = false;
                    }
                    break;
                case "Wood":
                    if (player.Wood < cost)
                    {
                        HasEnoughResources = false;
                    }
                    break;
                case "Wheat":
                    if (player.Wheat < cost)
                    {
                        HasEnoughResources = false;
                    }
                    break;
            }
            return HasEnoughResources;
        }
        public void GetResourcesFromMysteryEvent()
        {
            switch (CurrentMystery.Resource)
            {
                case "Gold":
                    if (HasSufficientResources("Gold", Math.Abs(CurrentMystery.Number)))
                    {
                        Players.Where(t => t.PlayerID == ClientID).FirstOrDefault().Gold += CurrentMystery.Number;
                    }
                    else
                    {
                        Players.Where(t => t.PlayerID == ClientID).FirstOrDefault().Gold = 0;
                    }
                    break;
                case "ArmyPower":
                    if (HasSufficientResources("ArmyPower", Math.Abs(CurrentMystery.Number)))
                    {
                        Players.Where(t => t.PlayerID == ClientID).FirstOrDefault().ArmyPower += CurrentMystery.Number;
                    }
                    else
                    {
                        Players.Where(t => t.PlayerID == ClientID).FirstOrDefault().ArmyPower = 0;
                    }
                    break;
                case "Popularity":
                    if (HasSufficientResources("Popularity", Math.Abs(CurrentMystery.Number)))
                    {
                        Players.Where(t => t.PlayerID == ClientID).FirstOrDefault().Popularity += CurrentMystery.Number;
                    }
                    else
                    {
                        Players.Where(t => t.PlayerID == ClientID).FirstOrDefault().Popularity = 0;
                    }
                    break;
                case "Stone":
                    if (HasSufficientResources("Stone", Math.Abs(CurrentMystery.Number)))
                    {
                        Players.Where(t => t.PlayerID == ClientID).FirstOrDefault().Stone += CurrentMystery.Number;
                    }
                    else
                    {
                        Players.Where(t => t.PlayerID == ClientID).FirstOrDefault().Stone = 0;
                    }
                    break;
                case "Wood":
                    if (HasSufficientResources("Wood", Math.Abs(CurrentMystery.Number)))
                    {
                        Players.Where(t => t.PlayerID == ClientID).FirstOrDefault().Wood += CurrentMystery.Number;
                    }
                    else
                    {
                        Players.Where(t => t.PlayerID == ClientID).FirstOrDefault().Wood = 0;
                    }
                    Players.Where(t => t.PlayerID == ClientID).FirstOrDefault().Wood += CurrentMystery.Number;
                    break;
                case "Wheat":
                    if (HasSufficientResources("Wheat", Math.Abs(CurrentMystery.Number)))
                    {
                        Players.Where(t => t.PlayerID == ClientID).FirstOrDefault().Wheat += CurrentMystery.Number;
                    }
                    else
                    {
                        Players.Where(t => t.PlayerID == ClientID).FirstOrDefault().Wheat = 0;
                    }
                    Players.Where(t => t.PlayerID == ClientID).FirstOrDefault().Wheat += CurrentMystery.Number;
                    break;
                default:
                    break;
            }
        }
        public void MakeTrade()
        {
            foreach (var selectedoffer in Players.Where(t => t.PlayerID == ClientID).FirstOrDefault().Trade.SelectedOfferIndexes)
            {
                foreach (var cost in Players.Where(t => t.PlayerID == ClientID).FirstOrDefault().Trade.Offers[selectedoffer].Cost)
                {
                    switch (cost.Key)
                    {
                        case "Gold":
                            Players.Where(t => t.PlayerID == ClientID).FirstOrDefault().Gold -= cost.Value;
                            break;
                        case "ArmyPower":
                            Players.Where(t => t.PlayerID == ClientID).FirstOrDefault().ArmyPower -= cost.Value;
                            break;
                        case "Popularity":
                            Players.Where(t => t.PlayerID == ClientID).FirstOrDefault().Popularity -= cost.Value;
                            break;
                        case "Stone":
                            Players.Where(t => t.PlayerID == ClientID).FirstOrDefault().Stone -= cost.Value;
                            break;
                        case "Wood":
                            Players.Where(t => t.PlayerID == ClientID).FirstOrDefault().Wood -= cost.Value;
                            break;
                        case "Wheat":
                            Players.Where(t => t.PlayerID == ClientID).FirstOrDefault().Wheat -= cost.Value;
                            break;
                        default:
                            break;
                    }
                }
                foreach (var gain in Players.Where(t => t.PlayerID == ClientID).FirstOrDefault().Trade.Offers[selectedoffer].Gain)
                {
                    switch (gain.Key)
                    {
                        case "Gold":
                            Players.Where(t => t.PlayerID == ClientID).FirstOrDefault().Gold += gain.Value;
                            break;
                        case "ArmyPower":
                            Players.Where(t => t.PlayerID == ClientID).FirstOrDefault().ArmyPower += gain.Value;
                            break;
                        case "Popularity":
                            Players.Where(t => t.PlayerID == ClientID).FirstOrDefault().Popularity += gain.Value;
                            break;
                        case "Stone":
                            Players.Where(t => t.PlayerID == ClientID).FirstOrDefault().Stone += gain.Value;
                            break;
                        case "Wood":
                            Players.Where(t => t.PlayerID == ClientID).FirstOrDefault().Wood += gain.Value;
                            break;
                        case "Wheat":
                            Players.Where(t => t.PlayerID == ClientID).FirstOrDefault().Wheat += gain.Value;
                            break;
                        default:
                            break;
                    }
                }
                Players.Where(t => t.PlayerID == ClientID).FirstOrDefault().NumOfTradesMade++;
            }
        }
        public void ClearCompass(HexagonTile hexagon)
        {
            Players.Where(t => t.PlayerID == ClientID).FirstOrDefault().Trade = hexagon.Compass;
            hexagon.Compass = null;
        }
        public void MysteryBoxEvent(HexagonTile hexagonTile)
        {
            Point[] points = SelectedHexagonTile.NeighborCoords();
            Point point = new Point();
            point.X = hexagonTile.Position[0];
            point.Y = hexagonTile.Position[1];

            MysteryEvent mysteryEvent = null;
            if (hexagonTile != null)
            {
                //If: Forest, Mountain, WheatField
                if (hexagonTile.FieldType == FieldType.forest ||
                    hexagonTile.FieldType == FieldType.mountain ||
                    hexagonTile.FieldType == FieldType.wheat)
                {
                    int rnd = RandomNumber.RandomNumberGenerator(1, 21);
                    if (true && Players.Where(t => t.PlayerID == ClientID).FirstOrDefault().RemainingMoves != 0)//5% chance    //for normal: if (rnd == 1)     //for testing: if(true)
                    {
                        //Dequeue
                        if (MysteryEvents.Count() != 0)
                        {
                            mysteryEvent = MysteryEvents.Dequeue();

                            var player = Players.Where(t => t.PlayerID == ClientID).FirstOrDefault();

                            switch (mysteryEvent.Resource)
                            {
                                case "Gold":
                                    Players.Where(t => t.PlayerID == ClientID).FirstOrDefault().Gold += mysteryEvent.Number;
                                    if (Players.Where(t => t.PlayerID == ClientID).FirstOrDefault().Gold < 0)//cannot be negative! -> 0 default
                                    {
                                        Players.Where(t => t.PlayerID == ClientID).FirstOrDefault().Gold = 0;
                                    }
                                    break;
                                case "Wood":
                                    Players.Where(t => t.PlayerID == ClientID).FirstOrDefault().Wood += mysteryEvent.Number;
                                    if (Players.Where(t => t.PlayerID == ClientID).FirstOrDefault().Wood < 0)
                                    {
                                        Players.Where(t => t.PlayerID == ClientID).FirstOrDefault().Wood = 0;
                                    }
                                    break;
                                case "Stone":
                                    Players.Where(t => t.PlayerID == ClientID).FirstOrDefault().Stone += mysteryEvent.Number;
                                    if (Players.Where(t => t.PlayerID == ClientID).FirstOrDefault().Stone < 0)
                                    {
                                        Players.Where(t => t.PlayerID == ClientID).FirstOrDefault().Stone = 0;
                                    }
                                    break;
                                case "Popularity":
                                    Players.Where(t => t.PlayerID == ClientID).FirstOrDefault().Popularity += mysteryEvent.Number;
                                    if (Players.Where(t => t.PlayerID == ClientID).FirstOrDefault().Popularity < 0)
                                    {
                                        Players.Where(t => t.PlayerID == ClientID).FirstOrDefault().Popularity = 0;
                                    }
                                    break;
                                case "ArmyPower":
                                    Players.Where(t => t.PlayerID == ClientID).FirstOrDefault().ArmyPower += mysteryEvent.Number;
                                    if (Players.Where(t => t.PlayerID == ClientID).FirstOrDefault().ArmyPower < 0)
                                    {
                                        Players.Where(t => t.PlayerID == ClientID).FirstOrDefault().ArmyPower = 0;
                                    }
                                    break;
                                case "Wheat":
                                    Players.Where(t => t.PlayerID == ClientID).FirstOrDefault().Wheat += mysteryEvent.Number;
                                    if (Players.Where(t => t.PlayerID == ClientID).FirstOrDefault().Wheat < 0)
                                    {
                                        Players.Where(t => t.PlayerID == ClientID).FirstOrDefault().Wheat = 0;
                                    }
                                    break;
                                case "Moves":
                                    Players.Where(t => t.PlayerID == ClientID).FirstOrDefault().DefaultNumOfMoves += mysteryEvent.Number;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }
            CurrentMystery = mysteryEvent;
        }
        public Queue<MysteryEvent> LoadMysteryEvents()
        {
            Queue<MysteryEvent> mysteryEvents = new Queue<MysteryEvent>();
            List<MysteryEvent> mysteryEventslist = new List<MysteryEvent>();
            //beolvasás
            var file = File.ReadAllLines("Resources/Maps/MysteryEvents.txt");
            mysteryEventslist = new List<MysteryEvent>();
            foreach (var item in file)
            {
                string[] line = item.Split(';');
                mysteryEventslist.Add(new MysteryEvent(line[0], int.Parse(line[1]), line[2]));
            }

            Shuffle(mysteryEventslist);
            mysteryEventslist.ForEach(x => mysteryEvents.Enqueue(x));
            return mysteryEvents;
        }

        public void ReloadHexagonObjects()
        {
            if (GameMap != null)
            {
                foreach (var item in GameMap)
                {
                    item.OwnerId = 0;
                    item.Objects.Clear();
                }
                foreach (var player in Players.ToList())
                {
                    if (player != null)
                    {
                        foreach (var item in player.Units.ToList())
                        {
                            GameMap[item.Position[0], item.Position[1]].Objects.Add(item);
                            GameMap[item.Position[0], item.Position[1]].OwnerId = item.OwnerId;
                        }
                        foreach (var item in player.Villages.ToList())
                        {
                            GameMap[item.Position[0], item.Position[1]].Objects.Add(item);
                            GameMap[item.Position[0], item.Position[1]].OwnerId = item.OwnerId;
                        }
                        //foreach (var item in player.Trade.ToList())
                        //{
                        //    GameMap[item.Position[0], item.Position[1]].Compass = item;
                        //    GameMap[item.Position[0], item.Position[1]].OwnerId = item.OwnerId;
                        //}
                    }
                }
            }
        }
        public void ReadQuests()
        {
            var file = File.ReadAllLines("Resources/Maps/Quests.txt");
            int counter = 1;
            Quests = new List<Quest>();
            foreach (var item in file)
            {
                string[] line = item.Split(';');
                Quests.Add(new Quest(counter,line[0], line[1] == "false" ? false : true));
                counter++;
            }
        }
        public List<Quest> RandomQuestSelector(int n)
        {
            ReadQuests();
            List<Quest> curr_quests = new List<Quest>();
            List<int> indexes = new List<int>();
            int db = 0;
            while (db != n)
            {
                int ind = RandomNumber.RandomNumberGenerator(0, Quests.Count - 1);
                if (!indexes.Contains(ind))
                {
                    indexes.Add(ind);
                    db++;
                    curr_quests.Add(Quests.ElementAt(ind));
                }
            }
            return curr_quests;
        }
        public void IsQuestDone()
        {
            foreach (var item in Players.Where(x => x.PlayerID == ClientID).FirstOrDefault().Quests)
            {
                switch (item.Id)
                {
                    case 1:
                        if (!Quests.Where(x => x.Id == 1).FirstOrDefault().Done && Players.Where(x => x.PlayerID == ClientID).FirstOrDefault().Villages.Count >= 3)
                        {
                            Players.Where(x => x.PlayerID == ClientID).FirstOrDefault().Quests.Where(x => x.Id == 1).FirstOrDefault().Done = true;
                        }
                        break;
                    case 2:
                        if (!Quests.Where(x => x.Id == 2).FirstOrDefault().Done && Players.Where(x => x.PlayerID == ClientID).FirstOrDefault().ArmyPower >= 15)
                        {
                            Players.Where(x => x.PlayerID == ClientID).FirstOrDefault().Quests.Where(x => x.Id == 2).FirstOrDefault().Done = true;
                        }
                        break;
                    case 3:
                        if (!Quests.Where(x => x.Id == 3).FirstOrDefault().Done && Players.Where(x => x.PlayerID == ClientID).FirstOrDefault().Popularity >= 12)
                        {
                            Players.Where(x => x.PlayerID == ClientID).FirstOrDefault().Quests.Where(x => x.Id == 3).FirstOrDefault().Done = true;
                        }
                        break;
                    case 4:
                        if (!Quests.Where(x => x.Id == 4).FirstOrDefault().Done && Players.Where(x => x.PlayerID == ClientID).FirstOrDefault().BattlesWon >= 3)
                        {
                            Players.Where(x => x.PlayerID == ClientID).FirstOrDefault().Quests.Where(x => x.Id == 4).FirstOrDefault().Done = true;
                        }
                        break;
                    case 5:
                        if (!Quests.Where(x => x.Id == 5).FirstOrDefault().Done && Players.Where(x => x.PlayerID == ClientID).FirstOrDefault().Heroes.Count == 2)
                        {
                            Players.Where(x => x.PlayerID == ClientID).FirstOrDefault().Quests.Where(x => x.Id == 5).FirstOrDefault().Done = true;
                        }
                        break;
                    case 6:
                        if (!Quests.Where(x => x.Id == 6).FirstOrDefault().Done && Players.Where(x => x.PlayerID == ClientID).FirstOrDefault().Gold >= 20)
                        {
                            Players.Where(x => x.PlayerID == ClientID).FirstOrDefault().Quests.Where(x => x.Id == 6).FirstOrDefault().Done = true;
                        }
                        break;
                    case 7:
                        if (!Quests.Where(x => x.Id == 7).FirstOrDefault().Done && Players.Where(x => x.PlayerID == ClientID).FirstOrDefault().Villages.Any(x => x.Level == 3))
                        {
                            Players.Where(x => x.PlayerID == ClientID).FirstOrDefault().Quests.Where(x => x.Id == 7).FirstOrDefault().Done = true;
                        }
                        break;
                    case 8:
                        if (!Quests.Where(x => x.Id == 8).FirstOrDefault().Done && Players.Where(x => x.PlayerID == ClientID).FirstOrDefault().Units.Count >= 7)
                        {
                            Players.Where(x => x.PlayerID == ClientID).FirstOrDefault().Quests.Where(x => x.Id == 8).FirstOrDefault().Done = true;
                        }
                        break;
                    case 9:
                        if (!Quests.Where(x => x.Id == 9).FirstOrDefault().Done && Players.Where(x => x.PlayerID == ClientID).FirstOrDefault().HasEnteredGoldMine)
                        {
                            Players.Where(x => x.PlayerID == ClientID).FirstOrDefault().Quests.Where(x => x.Id == 9).FirstOrDefault().Done = true;
                        }
                        break;
                    case 10:
                        if (!Quests.Where(x => x.Id == 10).FirstOrDefault().Done && Players.Where(x => x.PlayerID == ClientID).FirstOrDefault().NumOfTradesMade >= 3)
                        {
                            Players.Where(x => x.PlayerID == ClientID).FirstOrDefault().Quests.Where(x => x.Id == 10).FirstOrDefault().Done = true;
                        }
                        break;
                }
            }
        }
        public void AddUnit()
        {
            if (SelectedHexagonTile != null)
            {
                if (SelectedHexagonTile.OwnerId == ClientID && SelectedHexagonTile.Objects.Where(t => t.CanMove == false).ToList().Count > 0)
                {
                    var item = Players.Where(t => t.PlayerID == ClientID).FirstOrDefault();
                    if (item != null && item.RemainingMoves != 0)
                    {
                        Unit newUnit = new Unit();
                        newUnit.FactionType = item.Faction;
                        newUnit.Position = SelectedHexagonTile.Position;
                        newUnit.Name = item.Faction.ToString();
                        newUnit.OwnerId = item.PlayerID;

                        SelectedHexagonTile.Objects.Add(newUnit);
                        SelectedHexagonTile.OwnerId = ClientID;

                        item.Units.Add(newUnit);
                    }
                }
            }
        }
        public void AddVillage()
        {
            if (SelectedHexagonTile != null && SelectedHexagonTile.FieldType == FieldType.grass
                && SelectedHexagonTile.Objects.Where(t => t.CanMove == false).ToList().Count == 0)
            {
                if (SelectedHexagonTile.OwnerId == ClientID || SelectedHexagonTile.OwnerId == 0 && HasSufficientResources("Wood", 3) && HasSufficientResources("Stone", 2))
                {
                    var item = Players.Where(t => t.PlayerID == ClientID).FirstOrDefault();
                    if (item != null && item.RemainingMoves != 0)
                    {
                        Village newVillage = new Village();

                        newVillage.Position = SelectedHexagonTile.Position;
                        newVillage.Level = 1;
                        newVillage.FactionType = item.Faction;
                        newVillage.OwnerId = item.PlayerID;

                        SelectedHexagonTile.Objects.Add(newVillage);
                        Players.Where(t => t.PlayerID == ClientID).FirstOrDefault().Wood -= 3;
                        Players.Where(t => t.PlayerID == ClientID).FirstOrDefault().Stone -= 2;
                        SelectedHexagonTile.OwnerId = item.PlayerID;
                        item.Villages.Add(newVillage);
                    }
                }

            }
        }
        public void UpgradeVillage()
        {
            if (SelectedHexagonTile != null)
            {
                if ((SelectedHexagonTile.OwnerId == ClientID || SelectedHexagonTile.OwnerId == 0)
                    && HasSufficientResources("Gold", 3)
                    && HasSufficientResources("Wheat", 2))
                {
                    var player = Players.Where(t => t.PlayerID == ClientID).FirstOrDefault();
                    var item = SelectedHexagonTile.Objects.Where(t => t.CanMove == false).FirstOrDefault() as Village;
                    if (item != null && player.RemainingMoves != 0 && item.Level < 3)
                    {
                        SelectedHexagonTile.Objects.Remove(item);
                        item.Level++;
                        Players.Where(t => t.PlayerID == ClientID).FirstOrDefault().Gold -= 3;
                        Players.Where(t => t.PlayerID == ClientID).FirstOrDefault().Wheat -= 2;
                    }
                    if (item.Level==3)
                    {
                        Unit newUnit = new Unit();
                        newUnit.FactionType = player.Faction;
                        newUnit.Position = SelectedHexagonTile.Position;
                        newUnit.Name = player.Faction.ToString();
                        newUnit.OwnerId = player.PlayerID;

                        SelectedHexagonTile.Objects.Add(newUnit);
                        SelectedHexagonTile.OwnerId = ClientID;

                        player.Units.Add(newUnit);
                    }
                }
            }
        }

        public void MoveUnit(HexagonTile hexagonTile)
        {
            if (SelectedHexagonTile != null && SelectedHexagonTile.OwnerId == ClientID)
            {
                Point[] points = SelectedHexagonTile.NeighborCoords();
                Point point = new Point();
                point.X = hexagonTile.Position[0];
                point.Y = hexagonTile.Position[1];

                if (points.Contains(point))
                {
                    var player = Players.Where(t => t.PlayerID == ClientID).FirstOrDefault();
                    var item = SelectedHexagonTile.Objects.Where(t => t.CanMove).FirstOrDefault();

                    if (item != null && player.RemainingMoves != 0)
                    {

                        if (hexagonTile.Objects.ToList().Count == 0)
                        {
                            item.Move(hexagonTile.Position);
                            hexagonTile.Objects.Add(item);
                            hexagonTile.OwnerId = item.OwnerId;

                            SelectedHexagonTile.Objects.Remove(item);
                            if (SelectedHexagonTile.Objects.Count == 0)
                            {
                                SelectedHexagonTile.OwnerId = 0;
                            }
                            //MysteryBoxEvent(hexagonTile);
                            SelectedHexagonTile = null;

                            DecreaseMoves();
                        }
                        else
                        {
                            var enemy = hexagonTile.Objects.Where(t => t.CanMove && t.FactionType != player.Faction).FirstOrDefault();
                            if (enemy != null)
                            {
                                var enemyPlayer = Players.Where(t => t.PlayerID == enemy.OwnerId).FirstOrDefault();

                                if (player.ArmyPower * (item as Unit).Level >= enemy.Level * enemyPlayer.ArmyPower)
                                {
                                    hexagonTile.Objects.Remove(enemy);

                                    if (hexagonTile.Objects.Count == 0)
                                    {
                                        hexagonTile.OwnerId = 0;
                                    }

                                    enemy.Move(enemyPlayer.Villages.FirstOrDefault().Position);
                                    GameMap[enemy.Position[0], enemy.Position[1]].Objects.Add(enemy);

                                    item.Move(hexagonTile.Position);
                                    hexagonTile.Objects.Add(item);
                                    hexagonTile.OwnerId = item.OwnerId;
                                }
                                else
                                {
                                    item.Move(player.Villages.FirstOrDefault().Position);
                                    GameMap[item.Position[0], item.Position[1]].Objects.Add(item);
                                }

                                SelectedHexagonTile.Objects.Remove(item);

                                if (SelectedHexagonTile.Objects.Count == 0)
                                {
                                    SelectedHexagonTile.OwnerId = 0;
                                }
                                SelectedHexagonTile = null;

                                DecreaseMoves();
                            }
                        }
                    }
                }
            }

        }
        public void GetResources()
        {
            bool success = false;
            var player = Players.Where(t => t.PlayerID == ClientID).FirstOrDefault();
            if (player != null && player.RemainingMoves != 0)
            {
                foreach (var tile in GameMap)
                {
                    if (tile.OwnerId == ClientID)
                    {
                        foreach (var item2 in tile.Objects.ToList())
                        {
                            if (item2.CanMove)
                            {
                                tile.GiveResources(player);
                                if (success is false)
                                {
                                    success = true;
                                }
                            }
                        }
                    }
                }
            }
        }
        public void DecreaseMoves()
        {
            var item = Players.Where(t => t.PlayerID == ClientID).FirstOrDefault();
            if (item != null)
            {
                item.RemainingMoves--;
            }
        }
        public void ResetMoves()
        {
            var item = Players.Where(t => t.PlayerID == ClientID).FirstOrDefault();
            if (item != null)
            {
                item.RemainingMoves = item.DefaultNumOfMoves;
            }
        }


    }
    public static class RandomNumber
    {
        private static readonly RNGCryptoServiceProvider _generator = new RNGCryptoServiceProvider();

        public static int RandomNumberGenerator(int minimumValue, int maximumValue)
        {
            byte[] randomNumber = new byte[1];
            _generator.GetBytes(randomNumber);
            double asciiValueOfRandomCharacter = Convert.ToDouble(randomNumber[0]);
            double multiplier = Math.Max(0, (asciiValueOfRandomCharacter / 255d) - 0.00000000001d);
            int range = maximumValue - minimumValue + 1;
            double randomValueInRange = Math.Floor(multiplier * range);
            return (int)(minimumValue + randomValueInRange);
        }
    }
}
