using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.IO;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System.Collections.Specialized;

namespace ProjektSZI
{
    class Level
    {
        Texture2D wall; //tutaj będzie przechowywana grafika którą później w klasie głównej Game1 będziemy rysować
        Texture2D player;
        Texture2D placeA, placeB, placeC, placeD, placeE, placeF, start;
        public char[,] screenGridScheme = new char[22, 32]; //tutaj tworzymy tablicę w której upchamy planszę
        public char[,] screenGridSchemeExtra = new char[22, 32]; //tutaj tworzymy tablicę w której upchamy informacje dodatkowe
        public byte[,] screenGridSchemePF = new byte[22, 32];

        private bool IsEmptyAround(char[,] map, int x, int y)
        {
            if (
                (map[x - 1, y - 1] == 'x') &&
                (map[x - 1, y] == 'x') &&
                (map[x - 1, y + 1] == 'x') &&
                (map[x, y - 1] == 'x') &&
                (map[x, y] == 'x') &&
                (map[x, y + 1] == 'x') &&
                (map[x + 1, y - 1] == 'x') &&
                (map[x + 1, y] == 'x') &&
                (map[x + 1, y + 1] == 'x')
                ) return true;
            else return false;
        }

        private void GeneratePointPosition(ref char[,] map, ref Point _point, char _pointName)
        {
            Random rnd = new Random();
            int x, y;
            do
            {
                x = rnd.Next(19) + 1;
                y = rnd.Next(29) + 1;
            }
            while (!IsEmptyAround(map, x, y));
            _point.X = x;
            _point.Y = y;
            map[_point.X, _point.Y] = _pointName;
        }

        private string GetSector(int[] point)
        {
            if (point[0] < 11)
            {
                if (point[1] < 16) return "NW";
                else return "NE";
            }
            else
            {
                if (point[1] < 16) return "SW";
                else return "SE";
            }
        }

        private string[,] GetExample(char[, ,] map)
        {
            // array for quantity/points/coordinates
            int[,,] point = new int[4, 7, 2];
            for (int q = 0; q < 4; q++)
            {
                for (int i = 0; i < 22; i++)
                {
                    for (int j = 0; j < 32; j++)
                    {
                        if (map[q, i, j] == 'a') point[q, 0, 0] = i; point[q, 0, 1] = j;
                        if (map[q, i, j] == 'b') point[q, 1, 0] = i; point[q, 1, 1] = j;
                        if (map[q, i, j] == 'c') point[q, 2, 0] = i; point[q, 2, 1] = j;
                        if (map[q, i, j] == 'd') point[q, 3, 0] = i; point[q, 3, 1] = j;
                        if (map[q, i, j] == 'e') point[q, 4, 0] = i; point[q, 4, 1] = j;
                        if (map[q, i, j] == 'f') point[q, 5, 0] = i; point[q, 5, 1] = j;
                        if (map[q, i, j] == 's') point[q, 6, 0] = i; point[q, 6, 1] = j;
                    }
                }
            }
            string[,] examples = new string[4, 8];
            for (int i = 0; i < 4; i++)
            {   
                for (int j = 0; j < 7; j++)
			    {
                    int[] p = new int[2];
                    p[0] = point[i,j, 0];
                    p[1] = point[i,j, 1];
			        examples[i, j] = GetSector(p);
			    }
                if (map[i, 21, 31] == 'P') examples[i, 7] = "P"; // positive
                if (map[i, 21, 31] == 'N') examples[i, 7] = "N"; // negative 
                
            }
            return examples;
        }

        private bool IsPositve(string[,] example, int i)
        {
            if (example[i,7] == "P") return true;
            else return false;
        }

        // Learning the concept of "Good storage map"
        private bool CandidateEliminationAlgorithm(string[,] examples, char[,] map)
        {
            // Initialize G to a singleton set that includes everything
            List<string[]> G = new List<string[]>(); //
            string[] example = { "ALL", "ALL", "ALL", "ALL", "ALL", "ALL", "ALL" };
            G.Add(example);

            // Initialize S to a singleton set that includes the first  positive example
            int numOfFirstPositiveExample = 0;
            int i=0;

            for (i = 0; i < 4; i++)
            {
                if (examples[i, 7] == "P")
                {
                    numOfFirstPositiveExample = i;
                    break;
                }
            }


            string[] S = new string[7] { 
                examples[numOfFirstPositiveExample, 0],
                examples[numOfFirstPositiveExample, 1],
                examples[numOfFirstPositiveExample, 2],
                examples[numOfFirstPositiveExample, 3],
                examples[numOfFirstPositiveExample, 4],
                examples[numOfFirstPositiveExample, 5],
                examples[numOfFirstPositiveExample, 6],
            };

            for (int q = 0; q < examples.GetLength(0); q++)
            {
                if (IsPositve(examples, q))
                {
                    // Prune G to exclude descriptions inconsistent with the positive example
                    // obcinamy G aby wykluczyc niezgodne opisy z pozytywnym przykładem
                    for (int l = 0; l < 7; l++)
                    {
                        int g_count = G.Count;
                        for (int o=G.Count-1; o>=0 ; --o)
                        {
                            if (examples[q, l] != G[o][l]) G.RemoveAt(o);
                        }
                        // Generalize S to include the positive example
                        if (S[l] != examples[q, l]) S[l] = "ALL";
                    }
                }
                else
                {
                    // Specialize G to exclude the negative example
                    //G.Clear();
                    foreach (var ex in G)
                    {
                        for (int l = 0; l < 7; l++)
                        {
                            if (examples[q, l] == ex[l])
                            {
                                for (int m = 0; m < 7; m++)
                                {
                                    if (S[m] != ex[m]) ex[m] = S[m];
                                }
                            }
                        }
                    }
                    
                    for (int l = 0; l < 7; l++)
                    {
                        if (examples[q, l] != S[l])
                        {
                            string[] e = { "ALL", "ALL", "ALL", "ALL", "ALL", "ALL", "ALL" };
                            e[l] = S[l];
                            G.Add(e);
                        }
                    }   
                }
            }

            // array for quantity/point/coordinates
            int[,] point = new int[7, 2];
            for (int n = 0; n < 22; n++)
                {
                    for (int j = 0; j < 32; j++)
                    {
                        if (map[n, j] == 'a') point[0, 0] = i; point[0, 1] = j;
                        if (map[n, j] == 'b') point[1, 0] = i; point[1, 1] = j;
                        if (map[n, j] == 'c') point[2, 0] = i; point[2, 1] = j;
                        if (map[n, j] == 'd') point[3, 0] = i; point[3, 1] = j;
                        if (map[n, j] == 'e') point[4, 0] = i; point[4, 1] = j;
                        if (map[n, j] == 'f') point[5, 0] = i; point[5, 1] = j;
                        if (map[n, j] == 's') point[6, 0] = i; point[6, 1] = j;
                    }
             }
           
            string[] test = new string[7];
            for (int j = 0; j < 6; j++)
            {
                    int[] p = new int[2];
                    p[0] = point[j, 0];
                    p[1] = point[j, 1];
                    test[j] = GetSector(p);
            }        
            foreach (var ex in G)
            {
                for (int l = 0; l < 7; l++)
                {
                    if (!((ex[l] == test[l]) || (ex[l] == "ALL"))) return false;
                }
            }
            return true;
        }

              
        //public static Point S = new Point(4, 2);
        //public static Point A = new Point(19, 3);
        //public static Point B = new Point(12, 6);
        //public static Point C = new Point(19, 16);
        //public static Point D = new Point(3, 16);
        //public static Point E = new Point(13, 24);
        //public static Point F = new Point(17, 28);

        static public Point S = new Point();
        static public Point A = new Point();
        static public Point B = new Point();
        static public Point C = new Point();
        static public Point D = new Point();
        static public Point E = new Point();
        static public Point F = new Point();

        private int mapNum;

        public Level()
        {
            // loading examples
            char[, ,] exampleMaps = new char[4, 22, 32];
            string rowLine = "";

            StreamReader fileReader;
            for (int i = 0; i < 4; i++)
            {
                fileReader = new StreamReader("level1_place" + i + ".txt");
                {
                    for (int row = 0; row < 22; row++)
                    {
                        rowLine = fileReader.ReadLine();
                        for (int column = 0; column < 32; column++)
                            exampleMaps[i, row, column] = rowLine[column];
                    }
                    fileReader.ReadLine();
                }
                fileReader.Close();
            }

            string[,] examples = new string[4, 8];

            examples = GetExample(exampleMaps);

            // preparing map with size 22 x 32
            char[,] map = new char[22, 32];

            char[,] targetMap = new char[22, 32];

            for (int i = 0; i < 22; i++)
            {
                for (int j = 0; j < 32; j++)
                {
                    map[i, j] = 'x';
                    targetMap[i, j] = 'x';
                }
            }

           // random points with constrains
           GeneratePointPosition(ref targetMap, ref A, 'a');
           GeneratePointPosition(ref targetMap, ref B, 'b');
           GeneratePointPosition(ref targetMap, ref C, 'c');
           GeneratePointPosition(ref targetMap, ref D, 'd');
           GeneratePointPosition(ref targetMap, ref E, 'e');
           GeneratePointPosition(ref targetMap, ref F, 'f');
           GeneratePointPosition(ref targetMap, ref S, 's');

           if (CandidateEliminationAlgorithm(examples, targetMap))
           {
               int step = 0;
               // generating aisle
               for (int i = 0; i < 22; i++)
               {
                   for (int j = 0; j < 32; j++)
                   {
                       if (targetMap[i, j] == 'a' ||
                           targetMap[i, j] == 'b' ||
                           targetMap[i, j] == 'c' ||
                           targetMap[i, j] == 'd' ||
                           targetMap[i, j] == 'e' ||
                           targetMap[i, j] == 'f' ||
                           targetMap[i, j] == 's')
                       {
                           step++;
                           map[i, j] = ' ';
                           if (step % 2 == 0)
                               for (int k = 0; k < 32; k++)
                               {
                                   map[i, k] = ' ';
                               }
                           if (step % 2 == 1)
                               for (int k = 0; k < 22; k++)
                               {
                                   map[k, j] = ' ';
                               }
                       }
                   }
               }
               // crating borders
               for (int i = 0; i < 32; i++) map[0, i] = 'x';
               for (int i = 0; i < 32; i++) map[21, i] = 'x';
               for (int i = 1; i < 21; i++) map[i, 0] = 'x';
               for (int i = 1; i < 21; i++) map[i, 31] = 'x';

               // cleaning from top
               bool brk = false;

               for (int i = 1; i < 21; i++)
               {
                   for (int j = 1; j < 31; j++)
                   {
                       if (targetMap[i, j] == 'x' && map[i, j] == ' ') map[i, j] = 'x';
                       if (targetMap[i, j] != 'x') brk = true;
                       if (brk) break;
                   }
                   if (brk) break;
               }

               // cleaning from bottom
               brk = false;

               for (int i = 21; i > 1; i--)
               {
                   for (int j = 31; j > 1; j--)
                   {
                       if (targetMap[i, j] == 'x' && map[i, j] == ' ') map[i, j] = 'x';
                       if (targetMap[i, j] != 'x') brk = true;
                       if (brk) break;
                   }
                   if (brk) break;
               }

               //cleaning from left
               brk = false;

               for (int i = 1; i < 31; i++)
               {
                   for (int j = 1; j < 21; j++)
                   {
                       if (targetMap[j, i] == 'x' && map[j, i] == ' ') map[j, i] = 'x';
                       if (targetMap[j, i] != 'x') brk = true;
                       if (brk) break;
                   }
                   if (brk) break;
               }

               // cleaning from right
               brk = false;

               for (int i = 31; i > 1; i--)
               {
                   for (int j = 21; j > 1; j--)
                   {
                       if (targetMap[j, i] == 'x' && map[j, i] == ' ') map[j, i] = 'x';
                       if (targetMap[j, i] != 'x') brk = true;
                       if (brk) break;
                   }
                   if (brk) break;
               }
               // setting as positive example
               targetMap[21, 31] = 'P';
           }
           else
           {
               for (int i = 0; i < 22; i++)
               {
                   for (int j = 0; j < 32; j++)
                   {
                       map[i, j] = ' ';
                   }
               }
               // crating borders
               for (int i = 0; i < 32; i++) map[0, i] = 'x';
               for (int i = 0; i < 32; i++) map[21, i] = 'x';
               for (int i = 1; i < 21; i++) map[i, 0] = 'x';
               for (int i = 1; i < 21; i++) map[i, 31] = 'x';

               // cleaning from top
               bool brk = false;

               for (int i = 1; i < 21; i++)
               {
                   for (int j = 1; j < 31; j++)
                   {
                       if (targetMap[i, j] == 'x' && map[i, j] == ' ') map[i, j] = 'x';
                       if (targetMap[i, j] != 'x') brk = true;
                       if (brk) break;
                   }
                   if (brk) break;
               }

               // cleaning from bottom
               brk = false;

               for (int i = 21; i > 1; i--)
               {
                   for (int j = 31; j > 1; j--)
                   {
                       if (targetMap[i, j] == 'x' && map[i, j] == ' ') map[i, j] = 'x';
                       if (targetMap[i, j] != 'x') brk = true;
                       if (brk) break;
                   }
                   if (brk) break;
               }

               //cleaning from left
               brk = false;

               for (int i = 1; i < 31; i++)
               {
                   for (int j = 1; j < 21; j++)
                   {
                       if (targetMap[j, i] == 'x' && map[j, i] == ' ') map[j, i] = 'x';
                       if (targetMap[j, i] != 'x') brk = true;
                       if (brk) break;
                   }
                   if (brk) break;
               }

               // cleaning from right
               brk = false;

               for (int i = 31; i > 1; i--)
               {
                   for (int j = 21; j > 1; j--)
                   {
                       if (targetMap[j, i] == 'x' && map[j, i] == ' ') map[j, i] = 'x';
                       if (targetMap[j, i] != 'x') brk = true;
                       if (brk) break;
                   }
                   if (brk) break;
               }

               // setting as negative example
               targetMap[21, 31] = 'N';
           }

           StreamWriter newMapFile = new StreamWriter("level1.txt");
           for (int i = 0; i < 22; i++)
           {
               for (int j = 0; j < 32; j++)
               {
                   newMapFile.Write(map[i, j]);
               }
               if (i < 21) newMapFile.WriteLine();
           }
           newMapFile.Close();

           Random rnd = new Random();
           mapNum = rnd.Next(4);
            
           StreamWriter targetMapFile = new StreamWriter("level1_place" + mapNum + ".txt");
           for (int i = 0; i < 22; i++)
           {
               for (int j = 0; j < 32; j++)
               {
                   targetMapFile.Write(targetMap[i, j]);
               }
               if (i < 21) targetMapFile.WriteLine();
           }
           targetMapFile.Close();
        }
        
        public void Load(ContentManager content)
        {
            wall = content.Load<Texture2D>("GFX/wall2");
            start = content.Load<Texture2D>("GFX/start");
            player = content.Load<Texture2D>("GFX/player");
            placeA = content.Load<Texture2D>("GFX/placeA");
            placeB = content.Load<Texture2D>("GFX/placeB");
            placeC = content.Load<Texture2D>("GFX/placeC");
            placeD = content.Load<Texture2D>("GFX/placeD");
            placeE = content.Load<Texture2D>("GFX/placeE");
            placeF = content.Load<Texture2D>("GFX/placeF");
        }

        ////////////////////
        ///Algorytm A*//////
        public List<Point> Pathfind(Point start, Point end)
        {
            // zamknieta lista punktów które już były przeszukane
            var closedSet = new List<Point>();
            // punkty które jeszcze nie zostały przejrzane
            var openSet = new List<Point> { start };
            // punkt poprzedni
            var cameFrom = new Dictionary<Point, Point>();
            // aktualna optymalna odległość
            var currentDistance = new Dictionary<Point, int>();
            // odległość obliczona na podstawie kosztów
            var predictedDistance = new Dictionary<Point, float>();

            currentDistance.Add(start, 0);
            predictedDistance.Add(
                start,
                0 + +Math.Abs(start.X - end.X) + Math.Abs(start.Y - end.Y)
            );

            // analizowanie kolejnych nieprzeszukanych punktów z openSet
            while (openSet.Count > 0)
            {
                // wybieranie najlepszegu punktu na dany moment
                var current = (
                    from p in openSet orderby predictedDistance[p] ascending select p
                ).First();

                // zwracanie ścieżki
                if (current.X == end.X && current.Y == end.Y)
                {
                    // rekonstruowanie ścieżki
                    return ReconstructPath(cameFrom, end);
                }

                // przemieszczanie punktu z openSet do ClosedSet
                openSet.Remove(current);
                closedSet.Add(current);

                foreach (var neighbor in GetNeighborNodes(current))
                {
                    var tempCurrentDistance = currentDistance[current] + 1;

                    // jeżeli znamy lepszą drogę, opuszczamy aktualną i wybieramy nową
                    if (closedSet.Contains(neighbor)
                        && tempCurrentDistance >= currentDistance[neighbor])
                    {
                        continue;
                    }

                    // jeżeli nie znamy lepszej drogi do kontynuujemy aktualną
                    if (!closedSet.Contains(neighbor)
                        || tempCurrentDistance < currentDistance[neighbor])
                    {
                        if (cameFrom.Keys.Contains(neighbor))
                        {
                            cameFrom[neighbor] = current;
                        }
                        else
                        {
                            cameFrom.Add(neighbor, current);
                        }

                        currentDistance[neighbor] = tempCurrentDistance;
                        predictedDistance[neighbor] =
                            currentDistance[neighbor]
                            + Math.Abs(neighbor.X - end.X)
                            + Math.Abs(neighbor.Y - end.Y);

                        // jeżeli do jest nowy punkt, dodajemy do naszej listy openSet
                        if (!openSet.Contains(neighbor))
                        {
                            openSet.Add(neighbor);
                        }
                    }
                }
            }

            // jeżeli algorytm nie znajdzie drogi, to wypisuje błąd
            throw new Exception(
                string.Format(
                    "unable to find a path between {0},{1} and {2},{3}",
                    start.X, start.Y,
                    end.X, end.Y
                )
            );
        }


        // zwracanie listy sąsiadów
        private IEnumerable<Point> GetNeighborNodes(Point node)
        {
            var nodes = new List<Point>();
            // na potrzeby projektu nie analizujemy skosów, ponieważ robot porusza się tylko w 4 kierunkach
            // góra
            if (screenGridSchemePF[node.X, node.Y - 1] > 0)
            {
                nodes.Add(new Point(node.X, node.Y - 1));
            }

            // prawo
            if (screenGridSchemePF[node.X + 1, node.Y] > 0)
            {
                nodes.Add(new Point(node.X + 1, node.Y));
            }

            // dół
            if (screenGridSchemePF[node.X, node.Y + 1] > 0)
            {
                nodes.Add(new Point(node.X, node.Y + 1));
            }

            // lewo
            if (screenGridSchemePF[node.X - 1, node.Y] > 0)
            {
                nodes.Add(new Point(node.X - 1, node.Y));
            }

            return nodes;
        }

        // zwracamy "wsteczną" drogę uzyskaną dzięki funkcji Pathfind
        private List<Point> ReconstructPath(Dictionary<Point, Point> cameFrom, Point current)
        {
            if (!cameFrom.Keys.Contains(current))
            {
                return new List<Point> { current };
            }

            var path = ReconstructPath(cameFrom, cameFrom[current]);
            path.Add(current);
            return path;
        }


        public void LoadFromFile() //metoda która odczytuje nam plik z planszą i zapisuje dane do tablicy
        {
            //najpierw wczutyjemy plik z mapą gdzie są ściany
            string rowLine = "";
            StreamReader fileReader = new StreamReader("level1.txt");
            {
                for (int row = 0; row < 22; row++)
                {
                    rowLine = fileReader.ReadLine();
                    for (int column = 0; column < 32; column++)
                        screenGridScheme[row, column] = rowLine[column];
                }
                fileReader.ReadLine();
            }
            //a następnie z mapą gdzie znajdują się miejsca dodatkowe


            //fileReader = new StreamReader("level1_place" + mapNum + ".txt");
            //{
            //    for (int row = 0; row < 22; row++)
            //    {
            //        rowLine = fileReader.ReadLine();
            //        for (int column = 0; column < 32; column++)
            //            screenGridSchemeExtra[row, column] = rowLine[column];
            //    }
            //    sLine = fileReader.ReadLine();
            //}


            for (int row = 0; row < 22; row++)
            {
                for (int column = 0; column < 32; column++)
                {
                    screenGridSchemePF[row, column] = (byte)' ';
                    if (screenGridScheme[row, column] != ' ')
                        screenGridSchemePF[row, column] = 0;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch) //rysujemy całą planszę, razem w graczem czy paczkami
        {


            //rysowanie miejsc z danych punktów.
            spriteBatch.Draw(placeA, new Rectangle(A.Y * 32, A.X * 32, 32, 32), Color.White);
            spriteBatch.Draw(placeB, new Rectangle(B.Y * 32, B.X * 32, 32, 32), Color.White);
            spriteBatch.Draw(placeC, new Rectangle(C.Y * 32, C.X * 32, 32, 32), Color.White);
            spriteBatch.Draw(placeD, new Rectangle(D.Y * 32, D.X * 32, 32, 32), Color.White);
            spriteBatch.Draw(placeE, new Rectangle(E.Y * 32, E.X * 32, 32, 32), Color.White);
            spriteBatch.Draw(placeF, new Rectangle(F.Y * 32, F.X * 32, 32, 32), Color.White);
            spriteBatch.Draw(start, new Rectangle(S.Y * 32, S.X * 32, 32, 32), Color.White);

            //najpierw rysujemy oznaczenia gdzie jest miejsce do wysłania paczki,

            /*STARE RYSOWANIE Z PLIKU
            for (int y = 0; y < 22; y++)
            {
                for (int x = 0; x < 32; x++)
                {
                    if (screenGridSchemeExtra[y, x] == 'a')
                    {
                        spriteBatch.Draw(placeA, new Rectangle(positionX, positionY, 32, 32), Color.White);
                    }
                    if (screenGridSchemeExtra[y, x] == 'b')
                    {
                        spriteBatch.Draw(placeB, new Rectangle(positionX, positionY, 32, 32), Color.White);
                    }
                    if (screenGridSchemeExtra[y, x] == 'c')
                    {
                        spriteBatch.Draw(placeC, new Rectangle(positionX, positionY, 32, 32), Color.White);
                    }
                    if (screenGridSchemeExtra[y, x] == 'd')
                    {
                        spriteBatch.Draw(placeD, new Rectangle(positionX, positionY, 32, 32), Color.White);
                    }
                    if (screenGridSchemeExtra[y, x] == 'e')
                    {
                        spriteBatch.Draw(placeE, new Rectangle(positionX, positionY, 32, 32), Color.White);
                    }
                    if (screenGridSchemeExtra[y, x] == 'f')
                    {
                        spriteBatch.Draw(placeF, new Rectangle(positionX, positionY, 32, 32), Color.White);
                    }
                    positionX += 32;
                }
                positionY += 32;
                positionX = 0;
            }
            */
            //a tutaj rysujemy całą resztę, kolejność jest ważna ponieważ zależy nam, aby oznaczenia były zawsze rysowane na samym dole
            int positionX = 0;
            int positionY = 0;
            for (int y = 0; y < 22; y++)
            {
                for (int x = 0; x < 32; x++)
                {
                    if (screenGridScheme[y, x] == 'x')
                    {
                        spriteBatch.Draw(wall, new Rectangle(positionX, positionY, wall.Width, wall.Height), Color.White);
                    }
                    if (screenGridScheme[y, x] == 'z')
                    {
                        spriteBatch.Draw(wall, new Rectangle(positionX, positionY, 32, 32), Color.White);
                    }
                    if (screenGridScheme[y, x] == 'p')
                    {
                        spriteBatch.Draw(player, new Rectangle(positionX, positionY, player.Width, player.Height), Color.White);
                    }
                    positionX += 32;
                }
                positionY += 32;
                positionX = 0;
            }
        }
    }
}