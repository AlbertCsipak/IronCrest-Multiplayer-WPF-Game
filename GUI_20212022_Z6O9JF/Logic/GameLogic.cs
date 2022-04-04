using GUI_20212022_Z6O9JF.Models;
using Microsoft.Toolkit.Mvvm.Messaging;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace GUI_20212022_Z6O9JF.Logic
{
    public class GameLogic : IGameLogic
    {
        public ObservableCollection<Player> Players { get; set; }
        public List<Faction> AvailableFactions { get; set; }
        IMessenger messenger;
        public GameLogic(IMessenger messenger)
        {
            this.messenger = messenger;
            this.Players = new ObservableCollection<Player>();

            AvailableFactions = new List<Faction>();
            AvailableFactions.Add(Faction.Viking);
            AvailableFactions.Add(Faction.Crusader);
            AvailableFactions.Add(Faction.Arabian);
            AvailableFactions.Add(Faction.Mongolian);
        }
        public string Map { get; set; }
        public HexagonTile[,] GameMap { get; set; }
        public HexagonTile[,] GameMapSetup(string path)
        {
            string[] lines = File.ReadAllLines(path);
            ;
            HexagonTile[,] map = new HexagonTile[int.Parse(lines[0]), int.Parse(lines[1])];

            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    map[i, j] = new HexagonTile();
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
    }
}
