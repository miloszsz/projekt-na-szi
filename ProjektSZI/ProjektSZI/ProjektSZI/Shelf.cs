using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;

namespace ProjektSZI
{
    class Shelf
    {
        public readonly Point shelfMapLocation;
        public List<Product> productList;
        public List<String> knapsackContent;
        public SpriteFont spriteFont;
        private float timerPacking = 5.0f;
        const float TIMER_PACKING = 5.0f;
        private float timerPackedKnapsack = 5.0f;
        const float TIMER_PACKED_KNAPSACK = 5.0f;
        private bool isPacked = false;

        public Shelf(Point shelfMapLocation, SpriteFont spriteFont)
        {
            productList = new List<Product>();
            knapsackContent = new List<String>();
            this.shelfMapLocation = shelfMapLocation;
            this.spriteFont = spriteFont;
        }

        public bool addProduct(Product product)
        {
            if (productList.Count < SHELF_CAPACITY)
            {
                productList.Add(product);
                return true;
            }
            return false;
        }

        public List<String> ShelfContentText()
        {
            List<String> result = new List<String>();
            productList.ForEach(product => result.Add(product.ShelfLabel()));
            return result;
        }

        private Vector2 getContentTextBaseLocation()
        {
            if (shelfMapLocation.Equals(Level.A))
            {
                return new Vector2(1035, 78);
            }
            else if (shelfMapLocation.Equals(Level.B))
            {
                return new Vector2(1035, 183);
            }
            else if (shelfMapLocation.Equals(Level.C))
            {
                return new Vector2(1035, 288);
            }
            else if (shelfMapLocation.Equals(Level.D))
            {
                return new Vector2(1035, 398);
            }
            else if (shelfMapLocation.Equals(Level.E))
            {
                return new Vector2(1035, 509);
            }
            else if (shelfMapLocation.Equals(Level.F))
            {
                return new Vector2(1035, 615);
            }
            return new Vector2(500, 500);
        }

        private Vector2 getKnapsackTextBaseLocation()
        {
            if (shelfMapLocation.Equals(Level.A))
            {
                return new Vector2(1158, 78);
            }
            else if (shelfMapLocation.Equals(Level.B))
            {
                return new Vector2(1158, 183);
            }
            else if (shelfMapLocation.Equals(Level.C))
            {
                return new Vector2(1158, 288);
            }
            else if (shelfMapLocation.Equals(Level.D))
            {
                return new Vector2(1158, 398);
            }
            else if (shelfMapLocation.Equals(Level.E))
            {
                return new Vector2(1158, 509);
            }
            else if (shelfMapLocation.Equals(Level.F))
            {
                return new Vector2(1158, 615);
            }
            return new Vector2(500, 500);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < productList.Count; i++)
            {
                spriteBatch.DrawString(spriteFont, i+1 + "." + productList.ElementAt(i).ShelfLabel(),
                    new Vector2(getContentTextBaseLocation().X, getContentTextBaseLocation().Y + i * 11), Color.GreenYellow);
            }
            for (int i = 0; i < knapsackContent.Count; i++)
            {
                spriteBatch.DrawString(spriteFont, knapsackContent[i],
                    new Vector2(getKnapsackTextBaseLocation().X, getKnapsackTextBaseLocation().Y + i * 11), Color.White);
            }
            
        }

        public void Update(GameTime gameTime)
        {
            if (productList.Count() == SHELF_CAPACITY)
            {
                if (!isPacked)
                {
                    float elapsedFromFill = (float)gameTime.ElapsedGameTime.TotalSeconds;
                    timerPacking -= elapsedFromFill;
                    if (timerPacking < 0)
                    {
                        knapsackContent.Clear();
                        //BitArray knapsackPacking = new Knapsack(productList).getSolution();
                        /*for (int i = 0; i < knapsackPacking.Count; i++)
                        {
                            if (knapsackPacking[i])
                                knapsackContent.Add(productList[i]);
                        }*/
                        knapsackContent= new Knapsack(productList).getSolution();
                        timerPacking = TIMER_PACKING;
                        isPacked = true;
                    }
                }
                else
                {
                    float elapsedFromPack = (float)gameTime.ElapsedGameTime.TotalSeconds;
                    timerPackedKnapsack -= elapsedFromPack;
                    if (timerPackedKnapsack < 0)
                    {
                        productList.Clear();
                        knapsackContent.Clear();
                        timerPackedKnapsack = TIMER_PACKED_KNAPSACK;
                        isPacked = false;
                    }
                }
            }
        }

        public bool shouldGameBePaused()
        {
            return true;
        }

        private static readonly int SHELF_CAPACITY = 7;
    }
}
