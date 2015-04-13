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
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            level1.Draw(spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
