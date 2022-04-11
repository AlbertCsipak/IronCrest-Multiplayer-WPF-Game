﻿using GUI_20212022_Z6O9JF.Models;
using Microsoft.Toolkit.Mvvm.Messaging;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;

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
                        case 'm':
                            map[i, j].FieldType = FieldType.field;
                            break;
                        case 'v':
                            map[i, j].FieldType = FieldType.water;
                            break;
                        case 'e':
                            map[i, j].FieldType = FieldType.forest;
                            break;
                        case 'h':
                            map[i, j].FieldType = FieldType.hill;
                            break;
                        case 'b':
                            map[i, j].FieldType = FieldType.wheat;
                            break;
                        case 'A':
                            map[i, j].FieldType = FieldType.goldMine;
                            break;
                        default:
                            break;
                    }
                }
            }
            return map;
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
                        if (player.Hero != null)
                        {
                            var item = player.Hero;
                            GameMap[item.Position[0], item.Position[1]].Objects.Add(item);
                            GameMap[item.Position[0], item.Position[1]].OwnerId = item.OwnerId;
                        }
                    }
                }
            }
        }
        public void AddUnit()
        {
            if (SelectedHexagonTile != null)
            {
                if (SelectedHexagonTile.OwnerId == ClientID || SelectedHexagonTile.OwnerId == 0)
                {
                    var item = Players.Where(t => t.PlayerID == ClientID).FirstOrDefault();
                    if (item != null && item.Moves != 0)
                    {
                        Unit newUnit = new Unit();
                        newUnit.FactionType = item.Faction;
                        newUnit.Position = SelectedHexagonTile.Position;
                        newUnit.Name = "Barni";
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
            ;
            if (SelectedHexagonTile != null && SelectedHexagonTile.FieldType == FieldType.field && SelectedHexagonTile.Objects.Where(t => t.CanMove == false).ToList().Count == 0)
            {
                ;
                if (SelectedHexagonTile.OwnerId == ClientID || SelectedHexagonTile.OwnerId == 0)
                {
                    ;
                    var item = Players.Where(t => t.PlayerID == ClientID).FirstOrDefault();
                    ;
                    if (item != null && item.Moves != 0)
                    {
                        ;
                        Village newVillage = new Village();

                        newVillage.Position = SelectedHexagonTile.Position;
                        newVillage.Level = 1;
                        newVillage.FactionType = item.Faction;
                        newVillage.OwnerId = item.PlayerID;

                        SelectedHexagonTile.Objects.Add(newVillage);
                        SelectedHexagonTile.OwnerId = item.PlayerID;

                        item.Villages.Add(newVillage);
                        DecreaseMoves();
                        ;
                    }
                }

            }
        }
        public void UpgradeVillage()
        {
            if (SelectedHexagonTile != null)
            {
                if (SelectedHexagonTile.OwnerId == ClientID || SelectedHexagonTile.OwnerId == 0)
                {
                    var player = Players.Where(t => t.PlayerID == ClientID).FirstOrDefault();
                    var item = SelectedHexagonTile.Objects.Where(t => t.CanMove == false).FirstOrDefault() as Village;
                    ;
                    if (item != null && player.Moves != 0 && item.Level < 3)
                    {
                        SelectedHexagonTile.Objects.Remove(item);
                        item.Level++;
                        DecreaseMoves();
                    }
                }
            }
        }
        public void MoveUnit(HexagonTile hexagonTile)
        {
            if (SelectedHexagonTile != null)
            {
                if (SelectedHexagonTile.OwnerId == ClientID)
                {
                    Point[] points = SelectedHexagonTile.NeighborCoords();
                    Point point = new Point();
                    point.X = hexagonTile.Position[0];
                    point.Y = hexagonTile.Position[1];
                    if (hexagonTile.Objects.ToList().Count == 0 && points.Contains(point))
                    {
                        var player = Players.Where(t => t.PlayerID == ClientID).FirstOrDefault();
                        var item = SelectedHexagonTile.Objects.Where(t => t.CanMove).FirstOrDefault();

                        if (item != null && player.Moves != 0)
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
}
