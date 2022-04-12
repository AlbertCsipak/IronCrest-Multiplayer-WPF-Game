﻿using GUI_20212022_Z6O9JF.Models;
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
        public List<Trade> trades;
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
            return map;
        }
        public void LoadTrades()
        {
            trades = new List<Trade>()
            {
                new Trade(new Offer[3]{ new Offer("You get 3 gold.", 0 , new Dictionary<string, int>() { { "Gold", 3 } }), new Offer("You get 2 logs and 1 wheat for 2 gold.", 2, new Dictionary<string, int>() { { "Wood", 2 }, { "Wheat", 1 } }), new Offer("You get 3 stones and 3 logs for 4 gold.", 4, new Dictionary<string, int>() { { "Stone", 3 }, { "Wood", 3 } })}),
                new Trade(new Offer[3]{ new Offer("You get 3 gold.", 0 , new Dictionary<string, int>() { { "Gold", 3 } }), new Offer("You get 2 logs and 1 wheat for 2 gold.", 2, new Dictionary<string, int>() { { "Wood", 2 }, { "Wheat", 1 } }), new Offer("You get 3 stones and 3 logs for 4 gold.", 4, new Dictionary<string, int>() { { "Stone", 3 }, { "Wood", 3 } })}),
                new Trade(new Offer[3]{ new Offer("You get 3 gold.", 0 , new Dictionary<string, int>() { { "Gold", 3 } }), new Offer("You get 2 logs and 1 wheat for 2 gold.", 2, new Dictionary<string, int>() { { "Wood", 2 }, { "Wheat", 1 } }), new Offer("You get 3 stones and 3 logs for 4 gold.", 4, new Dictionary<string, int>() { { "Stone", 3 }, { "Wood", 3 } })})
            };
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

                                    enemy.Position[0] = 3;
                                    enemy.Position[1] = 3;
                                    //enemy.move

                                    hexagonTile.Objects.Remove(enemy);

                                    item.Move(hexagonTile.Position);
                                    hexagonTile.Objects.Add(item);
                                }
                                else
                                {
                                    item.Position[0] = 6;
                                    item.Position[1] = 6;
                                    GameMap[6, 6].Objects.Add(item);
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
