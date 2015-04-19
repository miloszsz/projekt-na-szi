using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapszi
{
    class Program
    {
        static void Main(string[] args)
        {
            string[,] map = new string[22,32]; // size 22 x 32

            // crating borders
            for (int i = 0; i < 32; i++) map[0, i] = "z";
            for (int i = 0; i < 32; i++) map[21, i] = "z";
            for (int i = 1; i < 21; i++) map[i, 0] = "z";
            for (int i = 1; i < 21; i++) map[i, 31] = "z";
           


            for (int i = 1; i < 21; i++)
            {
                for (int j = 1; j < 31; j++)
                {
                    map[i,j] = "x";
                }
            }

            for (int i = 0; i < 22; i++)
            {
                for (int j = 0; j < 32; j++)
                {
                    Console.Write(map[i, j]);
                }
                Console.WriteLine();
            }
            


            StreamWriter mapFile = new StreamWriter("level1.txt");
            for (int i = 0; i < 22; i++)
            {
                for (int j = 0; j < 32; j++)
                {
                    mapFile.Write(map[i, j]);
                }
                mapFile.WriteLine();
            }
            mapFile.Close();

        }
    }
}
