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
        public char[,] screenGridScheme = new char[22, 32]; //tutaj tworzymy tablicę w której upchamy planszę

        public Level()
        {

        }   //nic nie robiący kontruktor w razie czego

        public void Load(ContentManager content)
        {
            wall = content.Load<Texture2D>("GFX/wall");
            player = content.Load<Texture2D>("GFX/player");
        }

        public void LoadFromFile() //metoda która odczytuje nam plik z planszą i zapisuje dane do tablicy
        {

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
        }

        public void Draw(SpriteBatch spriteBatch) //rysujemy całą planszę, razem w graczem czy paczkami
        {
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
