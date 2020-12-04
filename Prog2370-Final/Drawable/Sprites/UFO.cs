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
        public float acceleration = 0.15f;
        public float lightAcceleration = 0.075f;
        public float maxVelocity = 10f;
        public float drag = 0.02f;
        public double angle = (Math.PI/2);
        public double changeInAngle = (Math.PI / 100);
        public float gas = 100;        
                        
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
            Rectangle r = new Rectangle((int)position.X + 50, (int)position.Y+50, tex.Width / 3, tex.Height / 3);

            spriteBatch.Draw(tex,r,null,Color.White,(float)angle - (float)(Math.PI / 2),new Vector2(tex.Width/2,tex.Height/2),SpriteEffects.None,0);

            
            spriteBatch.End();
        }

        public void Update(GameTime gameTime, KeyboardState ks)
        {
            this.tex = resources.UFO;
            if (ks.IsKeyDown(Keys.Up))
            {
                if(gas >= 0)
                {
                    velocity -= UnitVectorFromAngle((float)angle) * acceleration;

                    if (velocity.X > maxVelocity)
                    {
                        velocity.X = maxVelocity;
                    }
                    else if (velocity.X < (maxVelocity * -1))
                    {
                        velocity.X = (maxVelocity * -1);
                    }

                    gas = gas - 0.05f;
                    this.tex = resources.UFO_thrust;
                    
                }
                                                
            }
            else if (ks.IsKeyDown(Keys.Space))
            {
                if (gas >= 0)
                {
                    velocity -= UnitVectorFromAngle((float)angle) * lightAcceleration;

                    if (velocity.X > maxVelocity)
                    {
                        velocity.X = maxVelocity;
                    }
                    else if (velocity.X < (maxVelocity * -1))
                    {
                        velocity.X = (maxVelocity * -1);
                    }
                    gas = gas - 0.025f;
                    this.tex = resources.UFO_thrust;                    
                }
            }
            if (ks.IsKeyDown(Keys.Right))
            {
                angle += changeInAngle;
            }
            if (ks.IsKeyDown(Keys.Left))
            {
                angle -= changeInAngle;
            }

            // Environmental effects on speed
            this.velocity.Y += gravity;
            if(velocity.X > 0)
            {
                this.velocity.X -= drag;
            }
            else if(velocity.X < 0)
            {
                this.velocity.X += drag;
            }
            this.position += this.velocity;

            if (position.Y < -30)
            {
                position.Y = -30;
                velocity.Y = 0;
            }
            if (position.Y > Shared.stage.Y - 50)
            {
                position.Y = Shared.stage.Y - 50;
                velocity.Y = 0;
            }

        }

        private static Vector2 UnitVectorFromAngle(float angle)
            => new Vector2(
                (float)Math.Cos(angle),
                (float)Math.Sin(angle));
    }
}