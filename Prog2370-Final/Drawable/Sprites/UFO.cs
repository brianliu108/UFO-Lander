using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using System;

namespace Prog2370_Final.Drawable.Sprites
{
    public class UFO : Sprite
    {
        public Vector2 position;        
        public Vector2 velocity = new Vector2(0f,0f);
        public float gravity = .05f;
        public float acceleration = .075f;
        public double angle = Math.PI / 2;

        public static float maxVelocity = 1.4f;
        //public static float maxGravity = .2f;
        public UFO(Game game,
            SpriteBatch spriteBatch,
            Texture2D tex,
            Vector2 position) : base(game, spriteBatch, tex, position)
        {
            this.spriteBatch = spriteBatch;
            this.tex = tex;            
            resources = ((Game1)game).Resources;            
        }
                        
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(tex, new Rectangle((int)position.X,(int)position.Y, tex.Width/3, tex.Height/3),Color.White);

            spriteBatch.End();
        }

        public void Update(GameTime gameTime, KeyboardState ks)
        {
            this.tex = resources.UFO;
            if (ks.IsKeyDown(Keys.Up))
            {
                this.velocity.Y -= acceleration;
                this.tex = resources.UFO_thrust;
            }
            this.velocity.Y += gravity;
            this.position += this.velocity;

            if (Math.Abs(velocity.Y) <= maxVelocity)
            {
                
            }
            
            
        }

    }
}