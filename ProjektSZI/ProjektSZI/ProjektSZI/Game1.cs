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
using System.Diagnostics;

namespace ProjektSZI
{

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Level level1 = new Level(); //klasa z plansz¹
        Player player = new Player(Level.S.X, Level.S.Y); //klasa z graczem
    
        Texture2D side;
        Texture2D placeA, placeB, placeC, placeD, placeE, placeF, ghost, box, start;
        bool boxOnPaka = true;
        bool pD1 = false;
        bool pD2 = false;
        public Point dest = new Point();
        decimal[] tablicaodleglosci = new decimal[6];
        bool idle;
        Random rnd = new Random(Guid.NewGuid().GetHashCode());
        bool returnride;
        SpriteFont defaultFont;
        SpriteFont defaultFontTitle;
        Shelf shelfA, shelfB, shelfC, shelfD, shelfE, shelfF;
        Product currentProduct = new Product(0);
        private Dictionary<Point, Shelf> pointToShelfMap = new Dictionary<Point, Shelf>();


        Fuzzy fazzy = new Fuzzy();

        float timer = 0.001f;         //Initialize a 10 second timer
        const float TIMER = 0.2f;


        List<Point> droga;

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
            tablicaodleglosci[0] = level1.Pathfind(Level.S, Level.A).Count;
            tablicaodleglosci[1] = level1.Pathfind(Level.S, Level.B).Count;
            tablicaodleglosci[2] = level1.Pathfind(Level.S, Level.C).Count;
            tablicaodleglosci[3] = level1.Pathfind(Level.S, Level.D).Count;
            tablicaodleglosci[4] = level1.Pathfind(Level.S, Level.E).Count;
            tablicaodleglosci[5] = level1.Pathfind(Level.S, Level.F).Count;
            fazzy.setTABodleglosc(tablicaodleglosci);
            idle = true;
            returnride = false;
            defaultFont = Content.Load<SpriteFont>("CourierWozek");
            defaultFontTitle = Content.Load<SpriteFont>("SpriteFont1");
            shelfA = new Shelf(Level.A, defaultFont);
            shelfB = new Shelf(Level.B, defaultFont);
            shelfC = new Shelf(Level.C, defaultFont);
            shelfD = new Shelf(Level.D, defaultFont);
            shelfE = new Shelf(Level.E, defaultFont);
            shelfF = new Shelf(Level.F, defaultFont);
            pointToShelfMap.Add(Level.A, shelfA);
            pointToShelfMap.Add(Level.B, shelfB);
            pointToShelfMap.Add(Level.C, shelfC);
            pointToShelfMap.Add(Level.D, shelfD);
            pointToShelfMap.Add(Level.E, shelfE);
            pointToShelfMap.Add(Level.F, shelfF);
        }

        protected override void LoadContent()
        {

            spriteBatch = new SpriteBatch(GraphicsDevice);
            side = Content.Load<Texture2D>("GFX/side");
            start = Content.Load<Texture2D>("GFX/start");
            placeA = Content.Load<Texture2D>("GFX/placeA");
            placeB = Content.Load<Texture2D>("GFX/placeB");
            placeC = Content.Load<Texture2D>("GFX/placeC");
            placeD = Content.Load<Texture2D>("GFX/placeD");
            placeE = Content.Load<Texture2D>("GFX/placeE");
            placeF = Content.Load<Texture2D>("GFX/placeF");
            ghost = Content.Load<Texture2D>("GFX/ghost");
            box = Content.Load<Texture2D>("GFX/Box");
            level1.Load(Content);
            player.Load(Content);
        }

        protected override void UnloadContent()
        {

        }




        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            
            ////////////////////////////////////////

            player.Update();

            
            if (idle)
            {
                currentProduct = new Product(rnd.Next(Fuzzy.nazwa.Length));    
                int cel = fazzy.wybierz(currentProduct.typeIndex);
                if (cel == 0)
                    dest = Level.A;
                if (cel == 1)
                    dest = Level.B;
                if (cel == 2)
                    dest = Level.C;
                if (cel == 3)
                    dest = Level.D;
                if (cel == 4)
                    dest = Level.E;
                if (cel == 5)
                    dest = Level.F;
                idle = !idle;
            }
            droga = level1.Pathfind(new Point(player.positionX, player.positionY), dest);
            

            ////motoda wykonywana co timer
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            timer -= elapsed;
            if (timer < 0)
            {
                //Timer expired, execute action
                if ((dest.X != player.positionX) || (dest.Y != player.positionY))
                {
                    player.positionX = droga[1].X;
                    player.positionY = droga[1].Y;
                }
                else
                {
                    pointToShelfMap[dest].addProduct(currentProduct);
                    dest = Level.S;
                    returnride = true;
                };

                if (returnride)
                    if ((Level.S.X == player.positionX) && (Level.S.Y == player.positionY))
                    {
                        returnride = false;
                        idle = true;
                    }
                timer = TIMER;   //Reset Timer
            }
            shelfA.Update(gameTime);
            shelfB.Update(gameTime);
            shelfC.Update(gameTime);
            shelfD.Update(gameTime);
            shelfE.Update(gameTime);
            shelfF.Update(gameTime);

            //////////////////////////////////////
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

            /////////////
            ////Rysowanie próbnej drogi
            for (int i = 0; i < droga.Count; i++)
            {
                spriteBatch.Draw(ghost, new Rectangle(droga[i].Y * 32, droga[i].X * 32, 32, 32), Color.White);
            }


            //Rysowanie wszystkiego z metody znajduj¹cej siê w klasie obiekcie level1
            level1.Draw(spriteBatch);
            ////////
            

            //Drukowanie panelu z pó³kami
            spriteBatch.Draw(side, new Rectangle(1025, 0, 200, 700), Color.White);
            if (!returnride)
                spriteBatch.DrawString(defaultFontTitle, currentProduct.ShelfLabel(), new Vector2(1072, 28), Color.White);

            ///////////RYSOWANIE PUDE£///////////
            //drukowanie czy wózek posiada pud³o
            if (!returnride)
                spriteBatch.Draw(box, new Rectangle(1040, 25, 26, 26), Color.White);

            shelfA.Draw(spriteBatch);
            shelfB.Draw(spriteBatch);
            shelfC.Draw(spriteBatch);
            shelfD.Draw(spriteBatch);
            shelfE.Draw(spriteBatch);
            shelfF.Draw(spriteBatch);


            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
