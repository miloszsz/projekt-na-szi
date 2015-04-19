using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ProjektSZI
{

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Level level1 = new Level(); //klasa z plansz¹
        Player player = new Player(); //klasa z graczem
        Texture2D side;
        Texture2D placeA, placeB, placeC, placeD, placeE, placeF;

        public Game1() //w³aœciwoœci okna
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.IsFullScreen = false; 
            graphics.PreferredBackBufferHeight = 704;
            graphics.PreferredBackBufferWidth = 1224;
        }

        protected override void Initialize()
        {
            level1.LoadFromFile();
            base.Initialize();
        }

        protected override void LoadContent()
        {

            spriteBatch = new SpriteBatch(GraphicsDevice);
            side = Content.Load<Texture2D>("GFX/side");
            placeA = Content.Load<Texture2D>("GFX/placeA");
            placeB = Content.Load<Texture2D>("GFX/placeB");
            placeC = Content.Load<Texture2D>("GFX/placeC");
            placeD = Content.Load<Texture2D>("GFX/placeD");
            placeE = Content.Load<Texture2D>("GFX/placeE");
            placeF = Content.Load<Texture2D>("GFX/placeF");
            level1.Load(Content);
            player.Load(Content);
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            player.Update();

            if (level1.screenGridScheme[player.positionX, player.positionY] == ' ')
            {
                level1.screenGridScheme[player.oldpositionX, player.oldpositionY] = ' ';
                level1.screenGridScheme[player.positionX, player.positionY] = 'p';
            }
            else if (level1.screenGridScheme[player.positionX, player.positionY] == 'c')
            {
                level1.screenGridScheme[player.oldpositionX, player.oldpositionY] = ' ';
                level1.screenGridScheme[player.positionX, player.positionY] = 'p';
            }
            else
            {
                player.positionX = player.oldpositionX;
                player.positionY = player.oldpositionY;
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DimGray);
            spriteBatch.Begin();

            level1.Draw(spriteBatch);
            spriteBatch.Draw(side, new Rectangle(1025, 0, 200, 700), Color.White);

            if (level1.screenGridSchemeExtra[player.positionX, player.positionY] == 'a')
            {
                spriteBatch.Draw(placeA, new Rectangle(1050, 30, 32, 32), Color.White);

            }
            if (level1.screenGridSchemeExtra[player.positionX, player.positionY] == 'b')
            {
                spriteBatch.Draw(placeB, new Rectangle(1050, 30, 32, 32), Color.White);

            }
            if (level1.screenGridSchemeExtra[player.positionX, player.positionY] == 'c')
            {
                spriteBatch.Draw(placeC, new Rectangle(1050, 30, 32, 32), Color.White);

            }
            if (level1.screenGridSchemeExtra[player.positionX, player.positionY] == 'd')
            {
                spriteBatch.Draw(placeD, new Rectangle(1050, 30, 32, 32), Color.White);

            }
            if (level1.screenGridSchemeExtra[player.positionX, player.positionY] == 'e')
            {
                spriteBatch.Draw(placeE, new Rectangle(1050, 30, 32, 32), Color.White);

            }
            if (level1.screenGridSchemeExtra[player.positionX, player.positionY] == 'f')
            {
                spriteBatch.Draw(placeF, new Rectangle(1060, 30, 32, 32), Color.White);

            }


            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
