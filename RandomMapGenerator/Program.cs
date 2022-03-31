using System;
using System.IO;

namespace RandomMapGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            StreamWriter writer = new StreamWriter("Maps/map1.txt");
            RandomMapObjectGenerator r1 = new RandomMapObjectGenerator(9,5);
            string[,] map1=r1.Generator();
            for (int i = 0; i < map1.GetLength(0); i++)
            {
                for (int j = 0; j < map1.GetLength(1); j++)
                {
                    Console.Write(map1[i,j]);
                    writer.Write(map1[i, j]);
                }
                Console.WriteLine();
                writer.Write("\n");
            }
            Console.ReadLine();
            writer.Close();
        }
    }
}
