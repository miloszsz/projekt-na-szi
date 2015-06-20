using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
//using FuzzyLogicLibrary;

namespace ProjektSZI
{

    class Player
    {

        public int positionX;
        public int positionY;
        public int oldpositionX;
        public int oldpositionY;
        private Keys keyPressed = Keys.None;

        public Player(int x, int y) //inicjalizacja pozycji startowych
        {
            positionX = x;
            positionY = y;
        }

        public void Load(ContentManager content)
        {

        }

        public void Update() //poruszanie się graczem
        {

            oldpositionX = positionX;
            oldpositionY = positionY;
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyUp(keyPressed))
            {
                keyPressed = Keys.None;
            }

            if (keyboardState.IsKeyUp(keyPressed))
            {
                keyPressed = Keys.None;
            }

            if (keyboardState.IsKeyDown(keyPressed))
            {
                return;
            }

            if (keyboardState.IsKeyDown(Keys.Up))
            {
                keyPressed = Keys.Up;
                positionX--;
            }
            else if (keyboardState.IsKeyDown(Keys.Down))
            {
                keyPressed = Keys.Down;
                positionX++;
            }
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                keyPressed = Keys.Left;
                positionY--;
            }
            else if (keyboardState.IsKeyDown(Keys.Right))
            {
                keyPressed = Keys.Right;
                positionY++;
                
            }
        }

        public void InterfaceDraw(SpriteBatch spriteBatch)
        {

        }
    }
}
