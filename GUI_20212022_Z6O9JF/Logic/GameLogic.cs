using GUI_20212022_Z6O9JF.Models;
using Microsoft.Toolkit.Mvvm.Messaging;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;

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
        public enum FieldType { field, water, village, hill, forest, wheat }
        public FieldType[,] GameMap { get; set; }
        public FieldType[,] GameMapSetup(string path)
        {
            string[] lines = File.ReadAllLines(path);
            ;
            FieldType[,] map = new FieldType[int.Parse(lines[0]), int.Parse(lines[1])];

            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    switch (lines[i + 2][j])
                    {
                        case 'm':
                            map[i, j] = FieldType.field;
                            break;
                        case 'v':
                            map[i, j] = FieldType.water;
                            break;
                        case 'e':
                            map[i, j] = FieldType.forest;
                            break;
                        case 'h':
                            map[i, j] = FieldType.hill;
                            break;
                        case 'b':
                            map[i, j] = FieldType.wheat;
                            break;
                        default:
                            break;
                    }
                }
            }
            return map;
        }
        public void Move(double[,][] HexagonPoints, Point point, double width, double height)
        {
            double Height = height / GameMap.GetLength(0);
            double Width = width / GameMap.GetLength(1);
            //double a = (point.X - Width) / Width;
            //double b = (point.Y - Height) / Height;

            //int x = (int)Math.Round(a);
            //int y = (int)Math.Round(b);

            int x = -1;
            int y = -1;

            for (int i = 0; i < HexagonPoints.GetLength(0); i++)
            {
                for (int j = 0; j < HexagonPoints.GetLength(1); j++)
                {
                    if (point.X > HexagonPoints[i, j][0] - Width && point.X < HexagonPoints[i, j][0] + Width
                        && point.Y > HexagonPoints[i, j][1] - Height && point.Y < HexagonPoints[i, j][1] + Height)
                    {
                        ;
                        x = i;
                        y = j;
                    }
                }
            }

            if (x != -1)
            {
                GameMap[x, y] = FieldType.water;

            }
            ;
        }
    }
}
