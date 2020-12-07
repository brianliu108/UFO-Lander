using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Prog2370_Final.Drawable.Sprites
{
    public class UFO : Sprite, ICollidable
    {
        public Vector2 position = new Vector2(50, 50);
        private Vector2 velocity = new Vector2(0f, 0f);
        private Rectangle drawPos;
        private float gravity = .05f;
        private float acceleration = 0.15f;
        private float lightAcceleration = 0.075f;
        private float maxVelocity = 10f;
        private float drag = 0.02f;
        private double angle = (Math.PI / 2);
        private double changeInAngle = (Math.PI / 100);
        private float gas = 100;
        private int framesStill = 0;
        private SoundEffect thrust;
        private SoundEffectInstance thrustIns, hugeExplosionIns, softExplosionIns;
        private bool dead = false;
        private Explosion explosion;

        private const float SPEED_MARGIN = 0.1f;
        private const int FRAMES_STILL_MARGIN = 15;

        public float Speed => (float)Math.Sqrt(velocity.X * velocity.X + velocity.Y * velocity.Y);

        public bool CanCollide => true;

        public Rectangle AABB => new Rectangle((int)position.X - (drawPos.Width / 2), (int)position.Y - (drawPos.Height / 2), drawPos.Width, drawPos.Height - 12);

        public CollisionNotificationLevel CollisionNotificationLevel => CollisionNotificationLevel.Location;

        public List<CollisionLog> CollisionLogs { get; set; }

        public bool Perished => false;

        public float Gas { get => gas; set => gas = value; }
        public float MaxVelocity { get => maxVelocity; }



        //public static float maxGravity = .2f;
        public UFO(Game game,
            SpriteBatch spriteBatch,
            Texture2D tex,
            Vector2 position) : base(game, spriteBatch, tex, position)
        {
            this.spriteBatch = spriteBatch;
            this.tex = tex;
            resources = ((Game1)game).Resources;
            thrust = resources.thrust;
            thrustIns = thrust.CreateInstance();
            hugeExplosionIns = resources.hugeExplosion.CreateInstance();
            hugeExplosionIns.Volume = .1f;

            softExplosionIns = resources.softExplosion.CreateInstance();

        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            drawPos = new Rectangle((int)position.X, (int)position.Y, tex.Width / 3, tex.Height / 3);

            spriteBatch.Draw(tex, drawPos, null, Color.White, (float)angle - (float)(Math.PI / 2), new Vector2(tex.Width / 2, tex.Height / 2), SpriteEffects.None, 0);


            spriteBatch.End();
        }

        public void Update(GameTime gameTime, KeyboardState ks)
        {
            if (Speed < SPEED_MARGIN) framesStill++;
            else framesStill = 0;
            if (framesStill > FRAMES_STILL_MARGIN)
            {
                foreach (var log in CollisionLogs)
                {
                    if (log.collisionPartner is GasCan gasCan)
                    {
                        gas = 100; //TODO make a const?
                        gasCan.Perished = true;
                    }
                }
            }
            this.tex = resources.UFO;
            if (!dead)
            {
                UpdateMovement(ks);
            }
            else
            {
                thrustIns.Stop();
            }
            
            // Environmental effects on speed
            ApplyEnvironmentEffects();

            if (gas <= 0)
            {
                thrustIns.Stop();
            }
            if (CollisionLogs.Count > 0 && !dead)
            {
                if (CollisionLogs[0].collisionPartner is VectorImage && Speed > 5)
                {
                    //softExplosionIns.Play();
                    hugeExplosionIns.Play();
                    dead = true;
                }
            }

        }

        private static Vector2 UnitVectorFromAngle(float angle)
            => new Vector2(
                (float)Math.Cos(angle),
                (float)Math.Sin(angle));

        /// <summary>
        /// Applies the UFO movement operations upon a key input.
        /// </summary>
        /// <param name="ks">Applies movement from arrow keys and spacebar</param>
        private void UpdateMovement(KeyboardState ks)
        {
            if (ks.IsKeyDown(Keys.Right))
            {
                angle += changeInAngle;
            }
            else if (ks.IsKeyDown(Keys.Left))
            {
                angle -= changeInAngle;
            }

            if (ks.IsKeyDown(Keys.Up))
            {
                if (gas >= 0)
                {
                    // Max acceleration
                    Thrust(acceleration, 0.05f, 1.0f);

                }
            }
            else if (ks.IsKeyDown(Keys.Space))
            {
                if (gas >= 0)
                {
                    // Half acceleration
                    Thrust(lightAcceleration, 0.025f, 0.5f);
                }
            }
        }

        /// <summary>
        /// Changes the UFO to a moving state
        /// </summary>
        /// <param name="accel">Rate of acceleration</param>
        /// <param name="gasConsumption">Rate of decrease in gas level when accelerating</param>
        /// <param name="vol">Volume of thrust sound</param>
        private void Thrust(float accel, float gasConsumption, float vol)
        {
            velocity -= UnitVectorFromAngle((float)angle) * accel;
            if (velocity.X > maxVelocity)
            {
                velocity.X = maxVelocity;
            }
            else if (velocity.X < (maxVelocity * -1))
            {
                velocity.X = (maxVelocity * -1);
            }
            // Reduce gas level 
            gas = gas - gasConsumption;
            this.tex = resources.UFO_thrust;
            // Play soundeffect
            thrustIns.Volume = vol;
            thrustIns.Play();
        }

        /// <summary>
        /// Applies the constant effects of the environment to the UFO
        /// </summary>
        private void ApplyEnvironmentEffects()
        {
            this.velocity.Y += gravity;
            if (velocity.X > 0)
            {
                this.velocity.X -= drag;
            }
            else if (velocity.X < 0)
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
    }
}