using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.IO;
using Microsoft.Xna.Framework;

namespace ProjektSZI
{
    class Level
    {
        Texture2D wall; //tutaj będzie przechowywana grafika którą później w klasie głównej Game1 będziemy rysować
        Texture2D player;
        Texture2D placeA, placeB, placeC, placeD, placeE, placeF;
        public char[,] screenGridScheme = new char[22, 32]; //tutaj tworzymy tablicę w której upchamy planszę
        public char[,] screenGridSchemeExtra = new char[22, 32]; //tutaj tworzymy tablicę w której upchamy informacje dodatkowe


        public Level()
        {

        }   //nic nie robiący kontruktor w razie czego

        public void Load(ContentManager content)
        {
            wall = content.Load<Texture2D>("GFX/wall");
            player = content.Load<Texture2D>("GFX/player");
            placeA = content.Load<Texture2D>("GFX/placeA");
            placeB = content.Load<Texture2D>("GFX/placeB");
            placeC = content.Load<Texture2D>("GFX/placeC");
            placeD = content.Load<Texture2D>("GFX/placeD");
            placeE = content.Load<Texture2D>("GFX/placeE");
            placeF = content.Load<Texture2D>("GFX/placeF");
        }

        public void LoadFromFile() //metoda która odczytuje nam plik z planszą i zapisuje dane do tablicy
        {
            //najpierw wczutyjemy plik z mapą gdzie są ściany
            string rowLine = "";
            StreamReader fileReader = new StreamReader("level1.txt");
            string sLine = "";
            {
                for (int row = 0; row < 22; row++)
                {
                    rowLine = fileReader.ReadLine();
                    for (int column = 0; column < 32; column++)
                        screenGridScheme[row, column] = rowLine[column];
                }
                sLine = fileReader.ReadLine();
            }
            //a następnie z mapą gdzie znajdują się miejsca dodatkowe

            fileReader = new StreamReader("level1_place.txt");
            {
                for (int row = 0; row < 22; row++)
                {
                    rowLine = fileReader.ReadLine();
                    for (int column = 0; column < 32; column++)
                        screenGridSchemeExtra[row, column] = rowLine[column];
                }
                sLine = fileReader.ReadLine();
            }



        }

        public void Draw(SpriteBatch spriteBatch) //rysujemy całą planszę, razem w graczem czy paczkami
        {
            //najpierw rysujemy oznaczenia gdzie jest miejsce do wysłania paczki,
            int positionX = 0;
            int positionY = 0;
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

            //a tutaj rysujemy całą resztę, kolejność jest ważna ponieważ zależy nam, aby oznaczenia były zawsze rysowane na samym dole
            positionX = 0;
            positionY = 0;
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
