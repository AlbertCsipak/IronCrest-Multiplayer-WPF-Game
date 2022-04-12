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
        public List<Quest> quests;

        public Queue<MysteryEvent> MysteryEvents { get; set; }

        public GameLogic(IMessenger messenger)
        {
            this.messenger = messenger;
            this.Players = new ObservableCollection<Player>();

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
            Queue<Trade> trades = LoadTrades();
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
                            break;
                        default:
                            break;
                    }
                }
            }
            while (trades.Count!=0)
            {
                int randomI = RandomNumber.RandomNumberGenerator(0, map.GetLength(0)-1);
                int randomJ = RandomNumber.RandomNumberGenerator(0, map.GetLength(1)-1);
                if (map[randomI,randomJ].FieldType==FieldType.compassField && map[randomI, randomJ].Compasses.Count() == 0)
                {
                    map[randomI, randomJ].Compasses.Add(trades.Dequeue());
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
                int k = RandomNumber.RandomNumberGenerator(0,n);
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

        public void MysteryBoxEvent()
        {
            if (SelectedHexagonTile != null)
            {
                //If: Forst, Mountain, WheatField
                if (SelectedHexagonTile.FieldType == FieldType.forest ||
                    SelectedHexagonTile.FieldType == FieldType.mountain || 
                    SelectedHexagonTile.FieldType == FieldType.wheat)
                {
                    int rnd = RandomNumber.RandomNumberGenerator(1, 21);
                    if (true)//5% chance    //for normal: if (rnd == 1)     //for testing: if(true)
                    {
                        //Dequeue
                        MysteryEvent mysteryEvent = MysteryEvents.Dequeue();
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
                            case "Food":
                                Players.Where(t => t.PlayerID == ClientID).FirstOrDefault().Food += mysteryEvent.Number;
                                if (Players.Where(t => t.PlayerID == ClientID).FirstOrDefault().Food < 0)
                                {
                                    Players.Where(t => t.PlayerID == ClientID).FirstOrDefault().Food = 0;
                                }
                                break;

                            default:
                                break;
                        }

                    }
                }
            }
            
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
                        foreach (var item in player.Trade.ToList())
                        {
                            GameMap[item.Position[0], item.Position[1]].Compasses.Add(item);
                            GameMap[item.Position[0], item.Position[1]].OwnerId = item.OwnerId;
                        }
                    }
                }
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
            List<int> indexes = new List<int>();
            int db = 0;
            while (db != n)
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
        public void AddUnit()
        {
            if (SelectedHexagonTile != null)
            {
                if (SelectedHexagonTile.OwnerId == ClientID && SelectedHexagonTile.Objects.Where(t => t.CanMove == false).ToList().Count > 0)
                {
                    var item = Players.Where(t => t.PlayerID == ClientID).FirstOrDefault();
                    if (item != null && item.Moves != 0)
                    {
                        Unit newUnit = new Unit();
                        newUnit.FactionType = item.Faction;
                        newUnit.Position = SelectedHexagonTile.Position;
                        newUnit.Name = item.Faction.ToString();
                        newUnit.OwnerId = item.PlayerID;

                        SelectedHexagonTile.Objects.Add(newUnit);
                        SelectedHexagonTile.OwnerId = ClientID;

                        item.Units.Add(newUnit);
                        DecreaseMoves();
                    }
                }
            }
        }
        public void AddVillage()
        {
            if (SelectedHexagonTile != null && SelectedHexagonTile.FieldType == FieldType.grass
                && SelectedHexagonTile.Objects.Where(t => t.CanMove == false).ToList().Count == 0)
            {
                if (SelectedHexagonTile.OwnerId == ClientID || SelectedHexagonTile.OwnerId == 0 && Players.Where(t => t.PlayerID == ClientID).Select(x=>x.Wood).FirstOrDefault()>=3 && Players.Where(t => t.PlayerID == ClientID).Select(x => x.Stone).FirstOrDefault() >= 2)
                {
                    var item = Players.Where(t => t.PlayerID == ClientID).FirstOrDefault();
                    if (item != null && item.Moves != 0)
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
                        DecreaseMoves();
                    }
                }

            }
        }
        public void UpgradeVillage()
        {
            if (SelectedHexagonTile != null)
            {
                if (SelectedHexagonTile.OwnerId == ClientID || SelectedHexagonTile.OwnerId == 0 
                    && Players.Where(t => t.PlayerID == ClientID).Select(x => x.Gold).FirstOrDefault() >= 3 
                    && Players.Where(t => t.PlayerID == ClientID).Select(x => x.Wood).FirstOrDefault() >= 2)
                {
                    var player = Players.Where(t => t.PlayerID == ClientID).FirstOrDefault();
                    var item = SelectedHexagonTile.Objects.Where(t => t.CanMove == false).FirstOrDefault() as Village;
                    ;
                    if (item != null && player.Moves != 0 && item.Level < 3)
                    {
                        SelectedHexagonTile.Objects.Remove(item);
                        item.Level++;
                        Players.Where(t => t.PlayerID == ClientID).FirstOrDefault().Gold -= 3;
                        Players.Where(t => t.PlayerID == ClientID).FirstOrDefault().Food -= 2;
                        DecreaseMoves();
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

                    if (item != null && player.Moves != 0)
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
            if (player != null &&player.Moves!= 0)
            {
                foreach (var tile in GameMap)
                {
                    if (tile.OwnerId == ClientID)
                    {
                        foreach (var item2 in tile.Objects)
                        {
                            if (item2.CanMove)
                            {
                                tile.GiveResources(player);
                                if (success is false)
                                {
                                    success = true;
                                    DecreaseMoves();
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
                item.Moves--;
            }
        }
        public void ResetMoves()
        {
            var item = Players.Where(t => t.PlayerID == ClientID).FirstOrDefault();
            if (item != null)
            {
                item.Moves = 2;
            }
        }

        public void ChooseOffer()
        {
            
            throw new NotImplementedException();
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
