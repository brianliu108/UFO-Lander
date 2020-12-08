using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prog2370_Final.Drawable.Sprites
{
    public class Explosion : Sprite, IPerishable
    {
        private Vector2 position;
        private Vector2 dimension;
        private List<Rectangle> frames;        
        private int frameIndex = -1;
        private int delay;
        private int delayCounter;
        

        private const int ROW = 6;
        private const int COL = 6;

        public Explosion(Game game, SpriteBatch spriteBatch, Texture2D tex, Vector2 position, int delay) : base(game, spriteBatch, tex, position)
        {
            this.spriteBatch = spriteBatch;
            this.tex = tex;
            this.position = position;
            this.delay = delay;

            dimension = new Vector2(tex.Width / COL, tex.Height / ROW);

            this.Show(false);

            CreateFrames();
        }        

        private void CreateFrames()
        {
            frames = new List<Rectangle>();

            for (int i = 0; i < ROW; i++)
            {
                for (int j = 0; j < COL; j++)
                {
                    int x = j * (int)dimension.X;
                    int y = i * (int)dimension.Y;

                    Rectangle rec = new Rectangle(x, y, (int)dimension.X, (int)dimension.Y);
                    frames.Add(rec);
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            if(frameIndex >= 0)
            {
                spriteBatch.Draw(tex, position, frames[frameIndex], Color.White);
                //spriteBatch.Draw(tex, new Rectangle((int)position.X - (frames[frameIndex].Width * .5), 
                //    (int)position.Y - (frames[frameIndex].Height * ), (int)(frames[frameIndex].Width * 1), (int)(frames[frameIndex].Height * 1)), frames[frameIndex], Color.White);
            }

            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            delayCounter++;
            if (delayCounter < delay)
            {
                frameIndex++;
                if(frameIndex > ROW * COL - 1)
                {
                    frameIndex = -1;
                    this.Show(false);
                }

                delayCounter = 0;
                
            }
        }

        public bool Perished => false;

        public Vector2 Position { get => position; set => position = value; }
        public Vector2 Dimension { get => dimension; set => dimension = value; }
    }
}
