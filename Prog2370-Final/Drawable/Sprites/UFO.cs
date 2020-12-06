using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Prog2370_Final.Drawable.Sprites
{
    public class UFO : Sprite , ICollidable
    {
        public Vector2 position = new Vector2(50, 50);        
        public Vector2 velocity = new Vector2(0f,0f);
        public Rectangle drawPos;
        public float gravity = .05f;
        public float acceleration = 0.15f;
        public float lightAcceleration = 0.075f;
        public float maxVelocity = 10f;
        public float drag = 0.02f;
        public double angle = (Math.PI/2);
        public double changeInAngle = (Math.PI / 100);
        public float gas = 100;
        
        public bool CanCollide => true;

        public Rectangle AABB => new Rectangle((int)position.X - (drawPos.Width/2),(int)position.Y - (drawPos.Height/2),drawPos.Width,drawPos.Height - 12);

        public CollisionNotificationLevel CollisionNotificationLevel => CollisionNotificationLevel.Location;

        public List<CollisionLog> CollisionLogs { get; set; }

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
            drawPos = new Rectangle((int)position.X, (int)position.Y, tex.Width / 3, tex.Height / 3);

            spriteBatch.Draw(tex,drawPos,null,Color.White,(float)angle - (float)(Math.PI / 2),new Vector2(tex.Width/2,tex.Height/2),SpriteEffects.None,0);
            

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
                    this.Dispose();
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

            if (position.Y < 0)
            {
                position.Y = 0;
                velocity.Y = 0;
            }
            if (position.Y > Shared.stage.Y - 0)
            {
                position.Y = Shared.stage.Y - 0;
                velocity.Y = 0;
            }

        }

        private static Vector2 UnitVectorFromAngle(float angle)
            => new Vector2(
                (float)Math.Cos(angle),
                (float)Math.Sin(angle));
    }
}