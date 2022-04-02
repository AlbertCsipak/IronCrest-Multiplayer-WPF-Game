using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI_20212022_Z6O9JF.Logic
{
    public class MapGenLogic
    {
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

        public MapGenLogic()
        {

        }
    }
}
