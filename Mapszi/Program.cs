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
            string[,] map = new string[22,32]; // map size 22 x 32

            // crating borders
            //for (int i = 0; i < 32; i++) map[0, i] = "z";
            //for (int i = 0; i < 32; i++) map[21, i] = "z";
            //for (int i = 1; i < 21; i++) map[i, 0] = "z";
            //for (int i = 1; i < 21; i++) map[i, 31] = "z";

            string[,] targetMap = new string[22, 32];

            for (int i = 0; i < 22; i++)
            {
                for (int j = 0; j < 32; j++)
                {
                    targetMap[i, j] = "x";
                }
            }

            // random points
            Random rnd = new Random();
            
            targetMap[rnd.Next(22), rnd.Next(32)] = "a";
            targetMap[rnd.Next(22), rnd.Next(32)] = "b";
            targetMap[rnd.Next(22), rnd.Next(32)] = "c";
            targetMap[rnd.Next(22), rnd.Next(32)] = "d";
            targetMap[rnd.Next(22), rnd.Next(32)] = "e";
            targetMap[rnd.Next(22), rnd.Next(32)] = "f";
            targetMap[rnd.Next(22), rnd.Next(32)] = "s";

            Console.WriteLine("Target Map");
            for (int i = 0; i < 22; i++)
            {
                for (int j = 0; j < 32; j++)
                {
                    Console.Write(targetMap[i, j]);
                }
                Console.WriteLine();
            }

            StreamWriter targetMapFile = new StreamWriter("j:/level1_place.txt");
            for (int i = 0; i < 22; i++)
            {
                for (int j = 0; j < 32; j++)
                {
                    targetMapFile.Write(targetMap[i, j]);
                }
                targetMapFile.WriteLine();
            }
            targetMapFile.Close();
            
            ///////////////////////////////////////////////////////////////////////////////////
            // Labirynth Map
            
            for (int i = 0; i < 22; i++)
            {
                for (int j = 0; j < 32; j++)
                {
                    map[i,j] = "x";
                }
            }

            // generating aisle
            for (int i = 0; i < 22; i++)
            {
                for (int j = 0; j < 32; j++)
                {
                    if (targetMap[i, j] == "a" ||
                        targetMap[i, j] == "b" ||
                        targetMap[i, j] == "c" ||
                        targetMap[i, j] == "d" ||
                        targetMap[i, j] == "e" ||
                        targetMap[i, j] == "f" ||
                        targetMap[i, j] == "s")
                    {
                        for (int k = 0; k < 32; k++)
                        {
                            map[i, k] = " ";
                        }
                        for (int k = 0; k < 22; k++)
                        {
                            map[k, j] = " ";
                        } 
                    }
                }
            }


            Console.WriteLine("Map");
            for (int i = 0; i < 22; i++)
            {
                for (int j = 0; j < 32; j++)
                {
                    Console.Write(map[i, j]);
                }
                Console.WriteLine();
            }
            
            StreamWriter mapFile = new StreamWriter("j:/level1.txt");
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
