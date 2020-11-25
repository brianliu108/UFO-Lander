using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Prog2370_Final
{
    public class GasCan : DrawableGameComponent
    {
        private SpriteBatch spriteBatch;
        private Texture2D tex;
        private Rectangle size;        

        public GasCan(Game game,
            SpriteBatch spriteBatch,
            Texture2D tex,
            Vector2 position) : base(game)
        {
            this.spriteBatch = spriteBatch;
            this.tex = tex;
            this.size = new Rectangle((int)(position.X),(int)(position.Y), tex.Width/2,tex.Height/2);
        }

        public void Show(bool enable)
        {
            this.Enabled = enable;
            this.Visible = enable;
        }

        //public void Move(Vector2 position)
        //{
        //    this.position = position;
        //}
        //public Rectangle GetBound()
        //{
        //    return new Rectangle((int)(position.X), (int)(position.Y), tex.Width, tex.Height);
        //}

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(tex,size,Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
       
    }
}
