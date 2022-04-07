using GUI_20212022_Z6O9JF.Models;
using Microsoft.Toolkit.Mvvm.Messaging;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace GUI_20212022_Z6O9JF.Logic
{
    public class GameLogic : IGameLogic
    {
        public int ClientID { get; set; }
        public HexagonTile SelectedHexagonTile { get; set; }
        public ObservableCollection<Player> Players { get; set; }
        public List<Faction> AvailableFactions { get; set; }
        IMessenger messenger;
        public GameLogic(IMessenger messenger)
        {
            this.messenger = messenger;
            this.Players = new ObservableCollection<Player>();

            AvailableFactions = new List<Faction>();
        }
        public void SelectableFactions()
        {
            if (Players.Count < 1)
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
        public string Map { get; set; }
        public HexagonTile[,] GameMap { get; set; }
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
                        default:
                            break;
                    }
                }
            }
            return map;
        }
        public void HexagonObjects()
        {
            if (GameMap != null)
            {
                foreach (var item in GameMap)
                {
                    item.OwnerId = 0;
                    item.Objects.Clear();
                }
                //System.InvalidOperationException: 'Collection was modified; enumeration operation may not execute.'
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
                foreach (var item in Players)
                {
                    if (item.PlayerID == ClientID)
                    {
                        Unit newUnit = new Unit();
                        newUnit.UnitType = UnitType.Viking;
                        newUnit.Position = SelectedHexagonTile.Position;
                        newUnit.Name = "Barni";
                        newUnit.OwnerId = ClientID;
                        SelectedHexagonTile.Objects.Add(newUnit);
                        SelectedHexagonTile.OwnerId = ClientID;
                        item.Units.Add(newUnit);
                    }
                }
            }
        }
        public void AddVilage()
        {
            if (SelectedHexagonTile != null)
            {
                foreach (var item in Players)
                {
                    if (item.PlayerID == ClientID)
                    {
                        Village newVillage = new Village();
                        newVillage.Position = SelectedHexagonTile.Position;
                        newVillage.Level = 1;
                        newVillage.VillageType = VillageType.Viking;
                        newVillage.OwnerId = ClientID;
                        SelectedHexagonTile.Objects.Add(newVillage);
                        SelectedHexagonTile.OwnerId = ClientID;
                        item.Villages.Add(newVillage);
                    }
                }
            }
        }
    }
}
