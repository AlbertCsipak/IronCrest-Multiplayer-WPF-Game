using GUI_20212022_Z6O9JF.Models;
using Microsoft.Toolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
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
        public Game Game { get; set; }
        public List<Faction> AvailableFactions { get; set; }
        public HexagonTile[,] GameMap { get; set; }
        public List<Quest> Quests;
        public Trade CurrentTrade { get; set; }
        public Queue<MysteryEvent> MysteryEvents { get; set; }
        public MysteryEvent CurrentMystery { get; set; }
        public Hero FirstHero { get; set; }
        public Hero SecondaryHero { get; set; }
        public static Random Random = new Random();

        public static event EventHandler Move;
        public bool IsGameEnded;

        public GameLogic(IMessenger messenger)
        {
            this.messenger = messenger;
            MysteryEvents = new Queue<MysteryEvent>();
            Game = new Game();
            CurrentMystery = null;
            CurrentTrade = null;
            AvailableFactions = new List<Faction>();
            FirstHero = null;
            SecondaryHero = null;
        }
        public void SelectableFactions()
        {
            if (Game.Players.Count == 0)
            {
                AvailableFactions.Add(Faction.Viking);
                AvailableFactions.Add(Faction.Crusader);
                AvailableFactions.Add(Faction.Arabian);
                AvailableFactions.Add(Faction.Mongolian);
            }
            else
            {
                foreach (var item in Game.Players)
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
                        default:
                            break;
                    }
                }
            }
            if (Game.Trades.Count == 0)
            {
                Game.Trades = LoadTrades();
                foreach (var item in Game.Trades)
                {
                    while (item.Position[0] == 0)
                    {
                        int i = Random.Next(0, map.GetLength(0));
                        int j = Random.Next(0, map.GetLength(1));
                        if (map[i, j].FieldType == FieldType.grass && !map[i, j].Objects.Any(x=>x is Village) && map[i, j].Compass == null)
                        {
                            map[i, j].Compass = item;
                            item.Position[0] = i;
                            item.Position[1] = j;
                        }
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
        public void MakeTrade()
        {
            var player = Game.Players.Where(t => t.PlayerID == ClientID).FirstOrDefault();
            foreach (var selectedoffer in player.Trade.SelectedOfferIndexes)
            {
                foreach (var cost in player.Trade.Offers[selectedoffer].Cost)
                {
                    switch (cost.Key)
                    {
                        case "Gold":
                            player.Gold -= cost.Value;
                            break;
                        case "ArmyPower":
                            player.ArmyPower -= cost.Value;
                            break;
                        case "Popularity":
                            player.Popularity -= cost.Value;
                            break;
                        case "Stone":
                            player.Stone -= cost.Value;
                            break;
                        case "Wood":
                            player.Wood -= cost.Value;
                            break;
                        case "Wheat":
                            player.Wheat -= cost.Value;
                            break;
                        default:
                            break;
                    }
                }
                foreach (var gain in player.Trade.Offers[selectedoffer].Gain)
                {
                    switch (gain.Key)
                    {
                        case "Gold":
                            player.Gold += gain.Value;
                            break;
                        case "ArmyPower":
                            player.ArmyPower += gain.Value;
                            break;
                        case "Popularity":
                            player.Popularity += gain.Value;
                            break;
                        case "Stone":
                            player.Stone += gain.Value;
                            break;
                        case "Wood":
                            player.Wood += gain.Value;
                            break;
                        case "Wheat":
                            player.Wheat += gain.Value;
                            break;
                        default:
                            break;
                    }
                }
                player.NumOfTradesMade++;
            }
        }
        public List<Trade> LoadTrades()
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
            return trades;
        }





        public bool HasSufficientResources(int offerindex)
        {
            var player = Game.Players.Where(t => t.PlayerID == ClientID).FirstOrDefault();
            bool HasEnoughResources = true;
            int counter = 0;
            while (counter < player.Trade.Offers[offerindex].Cost.Count && HasEnoughResources)
            {
                var cost = player.Trade.Offers[offerindex].Cost.ElementAt(counter);
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
            var player = Game.Players.Where(t => t.PlayerID == ClientID).FirstOrDefault();
            bool HasEnoughResources = true;
            switch (resource)
            {
                case "Gold":
                    if (player.Gold < cost)
                    {
                        HasEnoughResources = false;
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
            var player = Game.Players.Where(t => t.PlayerID == ClientID).FirstOrDefault();
            switch (CurrentMystery.Resource)
            {
                case "Gold":
                    if (HasSufficientResources("Gold", Math.Abs(CurrentMystery.Number)))
                    {
                        player.Gold += CurrentMystery.Number;
                    }
                    else
                    {
                        player.Gold = 0;
                    }
                    break;
                case "ArmyPower":
                    if (HasSufficientResources("ArmyPower", Math.Abs(CurrentMystery.Number)))
                    {
                        player.ArmyPower += CurrentMystery.Number;
                    }
                    else
                    {
                        player.ArmyPower = 0;
                    }
                    break;
                case "Popularity":
                    if (HasSufficientResources("Popularity", Math.Abs(CurrentMystery.Number)))
                    {
                        player.Popularity += CurrentMystery.Number;
                    }
                    else
                    {
                        player.Popularity = 0;
                    }
                    break;
                case "Stone":
                    if (HasSufficientResources("Stone", Math.Abs(CurrentMystery.Number)))
                    {
                        player.Stone += CurrentMystery.Number;
                    }
                    else
                    {
                        player.Stone = 0;
                    }
                    break;
                case "Wood":
                    if (HasSufficientResources("Wood", Math.Abs(CurrentMystery.Number)))
                    {
                        player.Wood += CurrentMystery.Number;
                    }
                    else
                    {
                        player.Wood = 0;
                    }
                    //player.Wood += CurrentMystery.Number;
                    break;
                case "Wheat":
                    if (HasSufficientResources("Wheat", Math.Abs(CurrentMystery.Number)))
                    {
                        player.Wheat += CurrentMystery.Number;
                    }
                    else
                    {
                        player.Wheat = 0;
                    }
                    //Players.Where(t => t.PlayerID == ClientID).FirstOrDefault().Wheat += CurrentMystery.Number;
                    break;
                default:
                    break;
            }
        }
        public void ClearCompass(HexagonTile hexagon)
        {
            var player = Game.Players.Where(t => t.PlayerID == ClientID).FirstOrDefault();
            player.Trade = hexagon.Compass;
            player.Trade.OwnerId = player.PlayerID;
            hexagon.Compass = null;
        }
        public void MysteryBoxEvent(HexagonTile hexagonTile)
        {
            var player = Game.Players.Where(t => t.PlayerID == ClientID).FirstOrDefault();
            Point[] points = SelectedHexagonTile.NeighborCoords();
            Point point = new Point();
            point.X = hexagonTile.Position[0];
            point.Y = hexagonTile.Position[1];
            MysteryEvent mysteryEvent = null;
            if (hexagonTile != null && points.Contains(point))
            {
                if (hexagonTile.FieldType == FieldType.forest ||
                    hexagonTile.FieldType == FieldType.mountain ||
                    hexagonTile.FieldType == FieldType.wheat ||
                    hexagonTile.FieldType == FieldType.grass)
                {
                    int rnd = RandomNumber.RandomNumberGenerator(1, 101);
                    if (rnd < 51)//5% chance    //for normal: if (rnd == 1)     //for testing: if(true)
                    {
                        //Dequeue
                        if (MysteryEvents.Count() != 0)
                        {
                            mysteryEvent = MysteryEvents.Dequeue();
                            mysteryEvent.FieldType = hexagonTile.FieldType;

                            switch (mysteryEvent.Resource)
                            {
                                case "Gold":
                                    player.Gold += mysteryEvent.Number;
                                    if (player.Gold < 0)//cannot be negative! -> 0 default
                                    {
                                        player.Gold = 0;
                                    }
                                    break;
                                case "Wood":
                                    player.Wood += mysteryEvent.Number;
                                    if (player.Wood < 0)
                                    {
                                        player.Wood = 0;
                                    }
                                    break;
                                case "Stone":
                                    player.Stone += mysteryEvent.Number;
                                    if (player.Stone < 0)
                                    {
                                        player.Stone = 0;
                                    }
                                    break;
                                case "Popularity":
                                    player.Popularity += mysteryEvent.Number;
                                    if (player.Popularity < 0)
                                    {
                                        player.Popularity = 0;
                                    }
                                    break;
                                case "ArmyPower":
                                    player.ArmyPower += mysteryEvent.Number;
                                    if (player.ArmyPower < 0)
                                    {
                                        player.ArmyPower = 0;
                                    }
                                    break;
                                case "Wheat":
                                    player.Wheat += mysteryEvent.Number;
                                    if (player.Wheat < 0)
                                    {
                                        player.Wheat = 0;
                                    }
                                    break;
                                case "Moves":
                                    player.DefaultNumOfMoves += mysteryEvent.Number;
                                    player.RemainingMoves++;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    if (rnd > 50)//5% chance    //for normal: if (rnd == 1)     //for testing: if(true)
                    {
                        MysteryHeroEvent();
                    }
                }
            }
            CurrentMystery = mysteryEvent;
        }
        public void MysteryHeroEvent()
        {
            var player = Game.Players.Where(t => t.PlayerID == ClientID).FirstOrDefault();
            Hero firsthero = null;
            Hero secondaryhero = null;
            if (player.Heroes.Count == 2)
            {
                firsthero = null;
                secondaryhero = null;
            }
            else if (player.Heroes.Count == 0)
            {
                switch (player.Faction)
                {
                    case Faction.Viking:
                        if (player.Heroes.Count == 0)
                        {
                            int random = RandomNumber.RandomNumberGenerator(1, 2);
                            if (random == 1)
                            {
                                firsthero = new Hero() { Name = "Bjorn", Damage = 5, FactionType = Faction.Viking, OwnerId = player.PlayerID, HeroType = HeroType.First };
                                player.Heroes.Add(firsthero);
                            }
                            else
                            {
                                secondaryhero = new Hero() { Name = "Sigurd", Damage = 3, FactionType = Faction.Viking, OwnerId = player.PlayerID, HeroType = HeroType.Secondary };
                                player.Heroes.Add(secondaryhero);
                            }
                        }
                        break;
                    case Faction.Crusader:
                        if (player.Heroes.Count == 0)
                        {
                            int random = RandomNumber.RandomNumberGenerator(1, 2);
                            if (random == 1)
                            {
                                firsthero = new Hero() { Name = "Dark Knight", Damage = 4, FactionType = Faction.Crusader, OwnerId = player.PlayerID, HeroType = HeroType.First };
                                player.Heroes.Add(firsthero);
                            }
                            else
                            {
                                secondaryhero = new Hero() { Name = "Crusader Knight", Damage = 3, FactionType = Faction.Crusader, OwnerId = player.PlayerID, HeroType = HeroType.Secondary };
                                player.Heroes.Add(secondaryhero);
                            }
                        }
                        break;
                    case Faction.Mongolian:
                        if (player.Heroes.Count == 0)
                        {
                            int random = RandomNumber.RandomNumberGenerator(1, 2);
                            if (random == 1)
                            {
                                firsthero = new Hero() { Name = "Genghis Khan", Damage = 4, FactionType = Faction.Mongolian, OwnerId = player.PlayerID, HeroType = HeroType.First };
                                player.Heroes.Add(firsthero);
                            }
                            else
                            {
                                secondaryhero = new Hero() { Name = "Mongolian Mouse", Damage = 2, FactionType = Faction.Mongolian, OwnerId = player.PlayerID, HeroType = HeroType.Secondary };
                                player.Heroes.Add(secondaryhero);
                            }
                        }
                        break;
                    case Faction.Arabian:
                        if (player.Heroes.Count == 0)
                        {
                            int random = RandomNumber.RandomNumberGenerator(1, 2);
                            if (random == 1)
                            {
                                firsthero = new Hero() { Name = "Jhin", Damage = 3, FactionType = Faction.Arabian, OwnerId = player.PlayerID, HeroType = HeroType.First };
                                player.Heroes.Add(firsthero);
                            }
                            else
                            {
                                secondaryhero = new Hero() { Name = "Prophet", Damage = 2, FactionType = Faction.Arabian, OwnerId = player.PlayerID, HeroType = HeroType.Secondary };
                                player.Heroes.Add(secondaryhero);
                            }
                        }
                        break;
                }
            }
            else
            {
                if (!player.Heroes.Exists(x => x.HeroType == HeroType.First))
                {
                    switch (player.Faction)
                    {
                        case Faction.Viking:
                            firsthero = new Hero() { Name = "Bjorn", Damage = 5, FactionType = Faction.Viking, OwnerId = player.PlayerID, HeroType = HeroType.First };
                            player.Heroes.Add(firsthero);
                            break;
                        case Faction.Crusader:
                            firsthero = new Hero() { Name = "Dark Knight", Damage = 4, FactionType = Faction.Crusader, OwnerId = player.PlayerID, HeroType = HeroType.First };
                            player.Heroes.Add(firsthero);
                            break;
                        case Faction.Mongolian:
                            firsthero = new Hero() { Name = "Genghis Khan", Damage = 4, FactionType = Faction.Mongolian, OwnerId = player.PlayerID, HeroType = HeroType.First };
                            player.Heroes.Add(firsthero);
                            break;
                        case Faction.Arabian:
                            firsthero = new Hero() { Name = "Jhin", Damage = 3, FactionType = Faction.Arabian, OwnerId = player.PlayerID, HeroType = HeroType.First };
                            player.Heroes.Add(firsthero);
                            break;
                    }
                }
                else if (!player.Heroes.Exists(x => x.HeroType == HeroType.Secondary))
                {
                    switch (player.Faction)
                    {
                        case Faction.Viking:
                            secondaryhero = new Hero() { Name = "Sigurd", Damage = 3, FactionType = Faction.Viking, OwnerId = player.PlayerID, HeroType = HeroType.Secondary };
                            player.Heroes.Add(secondaryhero);
                            break;
                        case Faction.Crusader:
                            secondaryhero = new Hero() { Name = "Crusader Knight", Damage = 3, FactionType = Faction.Crusader, OwnerId = player.PlayerID, HeroType = HeroType.Secondary };
                            player.Heroes.Add(secondaryhero);
                            break;
                        case Faction.Mongolian:
                            secondaryhero = new Hero() { Name = "Mongolian Mouse", Damage = 2, FactionType = Faction.Mongolian, OwnerId = player.PlayerID, HeroType = HeroType.Secondary };
                            player.Heroes.Add(secondaryhero);
                            break;
                        case Faction.Arabian:
                            secondaryhero = new Hero() { Name = "Prophet", Damage = 2, FactionType = Faction.Arabian, OwnerId = player.PlayerID, HeroType = HeroType.Secondary };
                            player.Heroes.Add(secondaryhero);
                            break;
                        default:
                            break;
                    }
                }
            }
            FirstHero = firsthero;
            SecondaryHero = secondaryhero;
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
                mysteryEventslist.Add(new MysteryEvent(line[0], int.Parse(line[1]), line[2], FieldType.grass));
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
                    item.Compass = null;
                    item.Objects.Clear();
                }
                foreach (var player in Game.Players.ToList())
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
                    }
                }
                foreach (var item in Game.Trades)
                {
                    if (item.OwnerId==0)
                    {
                        GameMap[item.Position[0], item.Position[1]].Compass = item;
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
                Quests.Add(new Quest(counter, line[0], line[1] == "false" ? false : true));
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
        public bool IsQuestDone(int id)
        {
            var player = Game.Players.Where(t => t.PlayerID == id).FirstOrDefault();
            bool IsQuestDone = false;
            foreach (var item in player.Quests)
            {
                switch (item.Id)
                {
                    case 1:
                        if (!player.Quests.Where(x => x.Id == 1).FirstOrDefault().Done && player.Villages.Count >= 3)
                        {
                            player.Quests.Where(x => x.Id == 1).FirstOrDefault().Done = true;
                            IsQuestDone = true;
                        }
                        break;
                    case 2:
                        if (!player.Quests.Where(x => x.Id == 2).FirstOrDefault().Done && player.ArmyPower >= 15)
                        {
                            player.Quests.Where(x => x.Id == 2).FirstOrDefault().Done = true;
                            IsQuestDone = true;
                        }
                        break;
                    case 3:
                        if (!player.Quests.Where(x => x.Id == 3).FirstOrDefault().Done && player.Popularity >= 12)
                        {
                            player.Quests.Where(x => x.Id == 3).FirstOrDefault().Done = true;
                            IsQuestDone = true;
                        }
                        break;
                    case 4:
                        if (!player.Quests.Where(x => x.Id == 4).FirstOrDefault().Done && player.BattlesWon >= 3)
                        {
                            player.Quests.Where(x => x.Id == 4).FirstOrDefault().Done = true;
                            IsQuestDone = true;
                        }
                        break;
                    case 5:
                        if (!player.Quests.Where(x => x.Id == 5).FirstOrDefault().Done && player.Heroes.Count == 2)
                        {
                            player.Quests.Where(x => x.Id == 5).FirstOrDefault().Done = true;
                            IsQuestDone = true;
                        }
                        break;
                    case 6:
                        if (!player.Quests.Where(x => x.Id == 6).FirstOrDefault().Done && player.Gold >= 20)
                        {
                            player.Quests.Where(x => x.Id == 6).FirstOrDefault().Done = true;
                            IsQuestDone = true;
                        }
                        break;
                    case 7:
                        if (!player.Quests.Where(x => x.Id == 7).FirstOrDefault().Done)
                        {
                            foreach (var village in player.Villages)
                            {
                                if (village.Level == 3)
                                {
                                    player.Quests.Where(x => x.Id == 7).FirstOrDefault().Done = true;
                                    IsQuestDone = true;
                                }
                            }
                        }
                        break;
                    case 8:
                        if (!player.Quests.Where(x => x.Id == 8).FirstOrDefault().Done && player.Units.Count >= 7)
                        {
                            player.Quests.Where(x => x.Id == 8).FirstOrDefault().Done = true;
                            IsQuestDone = true;
                        }
                        break;
                    case 9:
                        if (!player.Quests.Where(x => x.Id == 9).FirstOrDefault().Done && player.HasEnteredGoldMine)
                        {
                            player.Quests.Where(x => x.Id == 9).FirstOrDefault().Done = true;
                            IsQuestDone = true;
                        }
                        break;
                    case 10:
                        if (!player.Quests.Where(x => x.Id == 10).FirstOrDefault().Done && player.NumOfTradesMade >= 3)
                        {
                            player.Quests.Where(x => x.Id == 10).FirstOrDefault().Done = true;
                            IsQuestDone = true;
                        }
                        break;
                }
            }
            return IsQuestDone;
        }
        public void SetGameEndOrder()
        {
            if (!IsGameEnded)
            {
                Game.WinOrder.Add(Game.Players.First(x => x.Quests.All(x => x.Done)));
                var playerWith2QuestsDoneAndMostGold = Game.Players.Where(x => x.Quests.Select(x => x.Done).Count() == 2).OrderBy(x => x.Gold).ToList();
                if (playerWith2QuestsDoneAndMostGold != null)
                {
                    playerWith2QuestsDoneAndMostGold.ForEach(x => Game.WinOrder.Add(x));
                }
                var playerWith1QuestsDoneAndMostGold = Game.Players.Where(x => x.Quests.Select(x => x.Done).Count() == 1).OrderBy(x => x.Gold).ToList();
                if (playerWith1QuestsDoneAndMostGold != null)
                {
                    playerWith1QuestsDoneAndMostGold.ForEach(x => Game.WinOrder.Add(x));
                }
                var playerWith0QuestsDoneAndMostGold = Game.Players.Where(x => x.Quests.Select(x => x.Done).Count() == 0).OrderBy(x => x.Gold).ToList();
                if (playerWith0QuestsDoneAndMostGold != null)
                {
                    playerWith0QuestsDoneAndMostGold.ForEach(x => Game.WinOrder.Add(x));
                }
                IsGameEnded = true;
            }

        }



        public void AddUnit()
        {
            var player = Game.Players.Where(t => t.PlayerID == ClientID).FirstOrDefault();
            player.TurnActivity = TurnActivity.Move;
            //if (SelectedHexagonTile != null)
            //{
            //    if (SelectedHexagonTile.OwnerId == ClientID && SelectedHexagonTile.Objects.Where(t => t.CanMove == false).ToList().Count > 0)
            //    {
            //        if (player != null && player.RemainingMoves != 0)
            //        {
            //            Unit newUnit = new Unit();
            //            newUnit.FactionType = player.Faction;
            //            newUnit.Position = SelectedHexagonTile.Position;
            //            newUnit.Name = player.Faction.ToString();
            //            newUnit.OwnerId = player.PlayerID;

            //            SelectedHexagonTile.Objects.Add(newUnit);
            //            SelectedHexagonTile.OwnerId = ClientID;

            //            player.Units.Add(newUnit);
            //        }
            //    }
            //}
        }
        public void AddVillage()
        {
            var player = Game.Players.Where(t => t.PlayerID == ClientID).FirstOrDefault();
            if (SelectedHexagonTile != null && SelectedHexagonTile.FieldType == FieldType.grass
                && !SelectedHexagonTile.Objects.Any(t => t is Unit))
            {
                if (SelectedHexagonTile.OwnerId == ClientID || SelectedHexagonTile.OwnerId == 0 && HasSufficientResources("Wood", 3) && HasSufficientResources("Stone", 2))
                {
                    if (player != null && player.RemainingMoves != 0)
                    {
                        Village newVillage = new Village();

                        newVillage.Position = SelectedHexagonTile.Position;
                        newVillage.Level = 1;
                        newVillage.FactionType = player.Faction;
                        newVillage.OwnerId = player.PlayerID;

                        SelectedHexagonTile.Objects.Add(newVillage);
                        player.Wood -= 3;
                        player.Stone -= 2;
                        SelectedHexagonTile.OwnerId = player.PlayerID;
                        player.Villages.Add(newVillage);
                        player.TurnActivity = TurnActivity.Build;
                        player.IsRecentTurnActivityMove = false;
                    }
                }

            }
        }
        public void UpgradeVillage()
        {
            var player = Game.Players.Where(t => t.PlayerID == ClientID).FirstOrDefault();

            if (SelectedHexagonTile != null)
            {
                if ((SelectedHexagonTile.OwnerId == ClientID || SelectedHexagonTile.OwnerId == 0)
                    && HasSufficientResources("Gold", 3)
                    && HasSufficientResources("Wheat", 2))
                {
                    //var item = SelectedHexagonTile.Objects.Where(t => t.CanMove == false).FirstOrDefault() as Village;
                    var village = SelectedHexagonTile.Objects.Where(t => t is Village).FirstOrDefault();
                    if (village != null && village.Level < 3)
                    {
                        player.TurnActivity = TurnActivity.Upgrade;
                        player.IsRecentTurnActivityMove = false;
                        SelectedHexagonTile.Objects.Remove(village);
                        village.Level++;
                        player.Gold -= 3;
                        player.Wheat -= 2;
                        if (village.Level == 3)
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
        }
        public void AddGold()
        {
            var player = Game.Players.Where(t => t.PlayerID == ClientID).FirstOrDefault();
            if (player != null && Game.CurrentGoldMineOwner == player)
            {
                if (player.Faction == Faction.Mongolian)
                {
                    Game.Players.Where(t => t.PlayerID == ClientID).FirstOrDefault().Gold += 3 * GameMap[5, 10].Objects.Where(x => x is Unit).FirstOrDefault().Level;
                }
                else
                {
                    Game.Players.Where(t => t.PlayerID == ClientID).FirstOrDefault().Gold += GameMap[5, 10].Objects.Where(x => x is Unit).FirstOrDefault().Level;
                }
            }
        }

        public void MoveUnit(HexagonTile hexagonTile)
        {
            var player = Game.Players.Where(t => t.PlayerID == ClientID).FirstOrDefault();
            //if (!player.IsRecentTurnActivityMove && player.RemainingMoves!=0)
            //{
                Move?.Invoke(this, EventArgs.Empty);
                if (SelectedHexagonTile != null && SelectedHexagonTile.OwnerId == ClientID)
                {
                    Point[] points = SelectedHexagonTile.NeighborCoords();
                    Point point = new Point();
                    point.X = hexagonTile.Position[0];
                    point.Y = hexagonTile.Position[1];

                    if (points.Contains(point))
                    {
                        var unit = SelectedHexagonTile.Objects.Where(t => t is Unit && t.OwnerId == ClientID).FirstOrDefault();

                        if (unit != null && player.RemainingMoves != 0)
                        {
                            if (hexagonTile.FieldType == FieldType.goldMine)
                            {
                                Game.CurrentGoldMineOwner = player;
                            }
                            if (SelectedHexagonTile.FieldType == FieldType.goldMine)
                            {
                                Game.CurrentGoldMineOwner = null;
                            }
                            if (hexagonTile.Objects.ToList().Count == 0)
                            {
                                unit.Move(hexagonTile.Position);

                                hexagonTile.Objects.Add(unit);
                                hexagonTile.OwnerId = unit.OwnerId;
                                SelectedHexagonTile.Objects.Remove(unit);
                                if (SelectedHexagonTile.Objects.Count == 0)
                                {
                                    SelectedHexagonTile.OwnerId = 0;
                                }
                                //SelectedHexagonTile = null;
                                DecreaseMoves();
                            }
                            else if (hexagonTile.Objects.ToList().Where(x => x is Unit && x.OwnerId == ClientID).FirstOrDefault() != null)
                            {
                                if (hexagonTile.Objects.First(x => x.OwnerId == ClientID).Level + unit.Level <= 3)
                                {
                                    hexagonTile.Objects.First(x => x.OwnerId == ClientID).Level += unit.Level;
                                    SelectedHexagonTile.Objects.Remove(unit);
                                    player.Units.Remove(unit as Unit);
                                    if (SelectedHexagonTile.Objects.Count == 0)
                                    {
                                        SelectedHexagonTile.OwnerId = 0;
                                    }
                                    SelectedHexagonTile = null;
                                    DecreaseMoves();
                                }
                            }
                            else if (hexagonTile.Objects.ToList().Any(x => x is Unit && x.OwnerId != ClientID))
                            {
                            unit.Move(hexagonTile.Position);
                            Battle(hexagonTile);
                                DecreaseMoves();
                            }
                            //{
                            //    //battle
                            //    var enemy = hexagonTile.Objects.Where(t => t.CanMove && t.FactionType != player.Faction).FirstOrDefault();
                            //    if (enemy != null)
                            //    {
                            //        var enemyPlayer = Players.Where(t => t.PlayerID == enemy.OwnerId).FirstOrDefault();
                            //        //CurrentBattle = new Battle();
                            //        //CurrentBattle.Defender = enemyPlayer;
                            //        //CurrentBattle.Attacker = player;
                            //        //clientLogic.BattleViewChange("battle");

                            //        if (player.ArmyPower * (item as Unit).Level >= enemy.Level * enemyPlayer.ArmyPower)
                            //        {
                            //            hexagonTile.Objects.Remove(enemy);

                            //            if (hexagonTile.Objects.Count == 0)
                            //            {
                            //                hexagonTile.OwnerId = 0;
                            //            }

                            //            enemy.Move(enemyPlayer.Villages.FirstOrDefault().Position);
                            //            GameMap[enemy.Position[0], enemy.Position[1]].Objects.Add(enemy);

                            //            item.Move(hexagonTile.Position);
                            //            hexagonTile.Objects.Add(item);
                            //            hexagonTile.OwnerId = item.OwnerId;
                            //        }
                            //        else
                            //        {
                            //            item.Move(player.Villages.FirstOrDefault().Position);
                            //            GameMap[item.Position[0], item.Position[1]].Objects.Add(item);
                            //        }

                            //        SelectedHexagonTile.Objects.Remove(item);

                            //        if (SelectedHexagonTile.Objects.Count == 0)
                            //        {
                            //            SelectedHexagonTile.OwnerId = 0;
                            //        }
                            //        SelectedHexagonTile = null;

                            //        DecreaseMoves();
                            //    }
                            //}
                        }
                    }
                }
                if (player.RemainingMoves==0)
                {
                    player.IsRecentTurnActivityMove = true;
                }
            //}

        }
        public void Battle(HexagonTile hexagonTile)
        {
            var attackerplayer = Game.Players.Where(t => t.PlayerID == ClientID).FirstOrDefault();
            var defenderenemy = Game.Players.Where(x => x.Units.Any(x => x.Position == hexagonTile.Position) && x.Faction != attackerplayer.Faction).FirstOrDefault();
            if (defenderenemy != null)
            {
                //var enemyPlayer = Game.Players.Where(t => t.PlayerID == defenderenemy.OwnerId).FirstOrDefault();
                Game.CurrentBattle = new Battle();
                Game.CurrentBattle.Defender = defenderenemy as Player;
                Game.CurrentBattle.Attacker = attackerplayer;
                ;
            }
        }
        public void GetResources()
        {
            var player = Game.Players.Where(t => t.PlayerID == ClientID).FirstOrDefault();
            player.TurnActivity = TurnActivity.Harvest;
            player.IsRecentTurnActivityMove = false;
            //bool success = false;
            if (player != null && player.RemainingMoves != 0)
            {
                foreach (var tile in GameMap)
                {
                    if (tile.OwnerId == ClientID)
                    {
                        foreach (var item2 in tile.Objects.ToList())
                        {
                            if (item2 is Unit)
                            {

                                tile.GiveResources(player, (item2 as Unit));

                                //if (!success)
                                //{
                                //    success = true;
                                //}
                            }
                        }
                    }
                }
            }
        }



        public void DecreaseMoves()
        {
            if (Game.Players.Where(t => t.PlayerID == ClientID).FirstOrDefault() != null)
            {
                Game.Players.Where(t => t.PlayerID == ClientID).FirstOrDefault().RemainingMoves--;
            }
        }
        public void ResetMoves()
        {
            if (Game.Players.Where(t => t.PlayerID == ClientID).FirstOrDefault() != null)
            {
                Game.Players.Where(t => t.PlayerID == ClientID).FirstOrDefault().RemainingMoves = Game.Players.Where(t => t.PlayerID == ClientID).FirstOrDefault().DefaultNumOfMoves;
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