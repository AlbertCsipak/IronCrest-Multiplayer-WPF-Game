using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomMapGenerator
{
    public enum MapObject
    {
        Field, Forest, Hill, WheatField, Water
    }
    public class RandomMapObjectGenerator
    {
        static Random r = new Random();
        int rows;
        int columns;
        string[,] map { get; set; }
        public RandomMapObjectGenerator(int x, int y)
        {
            rows = x;
            columns = y;
            map = new string[rows, columns];
        }
        public string[,] Generator()
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    int num = r.Next(1, 6);
                    switch (num)
                    {
                        case 1:
                            map[i, j] = "b"; break;
                        case 2:
                            map[i, j] = "m"; break;
                        case 3:
                            map[i, j] = "v"; break;
                        case 4:
                            map[i, j] = "h"; break;
                        case 5: 
                            map[i, j] = "e"; break;
                    }       
                }
            }
            map[rows / 2, columns / 2] = "A";
            return map;
        }
    }
}
