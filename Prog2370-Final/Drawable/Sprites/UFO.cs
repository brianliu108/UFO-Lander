using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Prog2370_Final.Drawable.Sprites {
    /// <summary>
    /// UFO Player Object. Arrow keys and spacebar to control
    /// </summary>
    public class UFO : Sprite, ICollidable {
        public Vector2 position = new Vector2(50, 50);
        private Vector2 velocity = new Vector2(0f, 0f);
        private Rectangle drawPos;
        private static float gravity = .05f;
        private static float acceleration = 0.15f;
        private static float lightAcceleration = 0.075f;
        private static float maxVelocity = 10f;
        private static float drag = 0.02f;
        private float highestYValue = 0;
        private double angle = (Math.PI / 2);
        private double changeInAngle = (Math.PI / 100);
        private float gas = 100;
        private int framesStill = 0;

        private SoundEffect thrust;
        private SoundEffectInstance thrustIns, hugeExplosionIns, softExplosionIns, landIns;
        private bool dead = false;
        private bool landed = false;

        private const float SPEED_MARGIN = 0.1f;
        private const int FRAMES_STILL_MARGIN = 15;

        private Vector2 dimension;
        private static List<Rectangle> frames;
        private int frameIndex = -1;
        private int delay;
        private int delayCounter;

        private const int ROW = 3;
        private const int COL = 3;

        /// <summary>
        /// Get current speed of UFO
        /// </summary>
        public float Speed => (float) Math.Sqrt(velocity.X * velocity.X + velocity.Y * velocity.Y);

        /// <summary>
        /// if UFO can currently collide with an ICollidable
        /// </summary>
        public bool CanCollide => true;
        /// <summary>
        /// the Axis Aligned Bounding Box for the UFO
        /// </summary>
        public Rectangle AABB => new Rectangle((int) position.X - (drawPos.Width / 2),
            (int) position.Y - (drawPos.Height / 2), drawPos.Width, drawPos.Height - 12);
        /// <summary>
        /// The type of collisions the UFO can have
        /// </summary>
        public CollisionNotificationLevel CollisionNotificationLevel => CollisionNotificationLevel.Location;
        /// <summary>
        /// Log of collisions
        /// </summary>
        public List<CollisionLog> CollisionLogs { get; set; }
        /// <summary>
        /// The gas level
        /// </summary>
        public float Gas {
            get => gas;
        }
        /// <summary>
        /// The UFO's max velocity
        /// </summary>
        public float MaxVelocity {
            get => maxVelocity;
        }
        /// <summary>
        /// Whether the UFO is dead or not
        /// </summary>
        public bool Dead {
            get => dead;
        }

        /// <summary>
        /// Dimensions of the sprite frame
        /// </summary>
        public Vector2 Dimension {
            get => dimension;
            set => dimension = value;
        }

        /// <summary>
        /// Creates the UFO object
        /// </summary>
        /// <param name="game">reference to main game</param>
        /// <param name="spriteBatch">spritebatch to draw with?</param>
        /// <param name="position">initial position of the UFO</param>
        public UFO(Game game,
            SpriteBatch spriteBatch,
            Vector2 position) : base(game, spriteBatch, ((Game1) game).Resources.UFOSprite, position) {
            this.spriteBatch = spriteBatch;
            this.position = position;
            resources = ((Game1) game).Resources;
            thrust = resources.thrust;

            // Creating the sounds
            thrustIns = thrust.CreateInstance();
            hugeExplosionIns = resources.hugeExplosion.CreateInstance();
            hugeExplosionIns.Volume = .1f;
            landIns = resources.land.CreateInstance();
            landIns.Volume = .1f;
            softExplosionIns = resources.softExplosion.CreateInstance();
            softExplosionIns.Volume = .1f;

            // Creating sprite frames
            delay = 5;
            dimension = new Vector2(tex.Width / COL, tex.Height / ROW);
            CreateFrames();
        }

        /// <summary>
        /// Extract the frames from the sprite
        /// </summary>
        private void CreateFrames() {
            frames = new List<Rectangle>();

            for (int i = 0; i < ROW; i++) {
                for (int j = 0; j < COL; j++) {
                    int x = j * (int) dimension.X;
                    int y = i * (int) dimension.Y;

                    Rectangle r = new Rectangle(x, y, (int) dimension.X, (int) dimension.Y);
                    frames.Add(r);
                }
            }
        }

        /// <summary>
        /// Draws the UFO to the screen
        /// </summary>
        /// <param name="gameTime">Common update sequence</param>
        public override void Draw(GameTime gameTime) {
            spriteBatch.Begin();
            drawPos = new Rectangle((int) position.X, (int) position.Y, tex.Width / 3, tex.Height / 3);

            // 
            if (frameIndex < 0)
                spriteBatch.Draw(resources.UFOSprite, drawPos, frames[0], Color.White,
                    (float) angle - (float) (Math.PI / 2), new Vector2(tex.Width / 2, tex.Height / 2),
                    SpriteEffects.None, 0);
            else {
                spriteBatch.Draw(resources.UFOSprite, drawPos, frames[frameIndex], Color.White,
                    (float) angle - (float) (Math.PI / 2),
                    new Vector2(frames[frameIndex].Width / 2, frames[frameIndex].Height / 2), SpriteEffects.None, 0);
                //spriteBatch.Draw(resources.UFOSprite, position, frames[frameIndex], Color.White);
            }            

            spriteBatch.End();
        }

        public void Update(GameTime gameTime, KeyboardState ks) {
            if (Speed < SPEED_MARGIN) framesStill++;
            else framesStill = 0;
            if (framesStill > FRAMES_STILL_MARGIN) {
                foreach (var log in CollisionLogs) {
                    if (log.collisionPartner is GasCan gasCan) {
                        gas = 100; //TODO make a const?
                        gasCan.Perished = true;
                    }
                }
            }
            this.tex = resources.UFO;
            delayCounter = -1;

            if (!dead) {
                UpdateMovement(ks);
            } else {
                thrustIns.Stop();
            }


            // Environmental effects on speed
            ApplyEnvironmentEffects();

            if (gas <= 0) {
                thrustIns.Stop();
                frameIndex = 0;
            }

            // Check if UFO is colliding with the terrain at all
            // Apply death logic
            if (CollisionLogs.Count(log => log.collisionPartner is VectorImage) > 0) {
                if (!dead && !landed) {
                    if (Speed <= 1.5) {
                        landIns.Play();
                    } else if (Speed > 1.5 && Speed <= 5) {
                        softExplosionIns.Play();
                        dead = true;
                    } else if (Speed > 5) {
                        //softExplosionIns.Play();
                        hugeExplosionIns.Play();
                        dead = true;
                    }
                    landed = true;

                    this.velocity.Y = 0f;
                    highestYValue = this.position.Y;

                    if (dead) {
                        MediaPlayer.Stop();
                        frameIndex = 0;
                    }
                }

                if (landed && gas <= 0) {
                    dead = true;
                }
            } else {
                landed = false;
                highestYValue = 0;
            }

            if (landed) {
                if (this.position.Y > highestYValue)
                    this.position.Y = highestYValue;
            }


            base.Update(gameTime);
        }

        private static Vector2 UnitVectorFromAngle(float angle)
            => new Vector2(
                (float) Math.Cos(angle),
                (float) Math.Sin(angle));

        /// <summary>
        /// Applies the UFO movement operations upon a key input.
        /// </summary>
        /// <param name="ks">Applies movement from arrow keys and spacebar</param>
        private void UpdateMovement(KeyboardState ks) {
            if (ks.IsKeyDown(Keys.Right)) {
                angle += changeInAngle;
            } else if (ks.IsKeyDown(Keys.Left)) {
                angle -= changeInAngle;
            }

            if (ks.IsKeyDown(Keys.Up)) {
                if (gas >= 0) {
                    // Max acceleration
                    Thrust(acceleration, 0.15f, 1.0f);
                }
            } else if (ks.IsKeyDown(Keys.Space)) {
                if (gas >= 0) {
                    // Half acceleration
                    Thrust(lightAcceleration, 0.075f, 0.5f);
                }
            } else {
                thrustIns.Stop();
                frameIndex = 0;
            }
        }

        /// <summary>
        /// Changes the UFO to a moving state
        /// </summary>
        /// <param name="accel">Rate of acceleration</param>
        /// <param name="gasConsumption">Rate of decrease in gas level when accelerating</param>
        /// <param name="vol">Volume of thrust sound</param>
        private void Thrust(float accel, float gasConsumption, float vol) {
            velocity -= UnitVectorFromAngle((float) angle) * accel;
            if (velocity.X > maxVelocity) {
                velocity.X = maxVelocity;
            } else if (velocity.X < (maxVelocity * -1)) {
                velocity.X = (maxVelocity * -1);
            }
            // Reduce gas level 
            gas = gas - gasConsumption;
            this.tex = resources.UFO_thrust;
            delayCounter++;

            // Animation
            if (delayCounter < delay) {
                frameIndex++;
                if (frameIndex > ROW * COL - 1) {
                    frameIndex = -1;
                    Show(true);
                }

                delayCounter = 0;
            }

            // Play soundeffect
            thrustIns.Volume = vol;
            thrustIns.Play();
        }

        /// <summary>
        /// Applies the constant effects of the environment to the UFO
        /// </summary>
        private void ApplyEnvironmentEffects() {
            if (!landed) {
                this.velocity.Y += gravity;
            }

            if (velocity.X > 0) {
                this.velocity.X -= drag;
            } else if (velocity.X < 0) {
                this.velocity.X += drag;
            }
            this.position += this.velocity;

            if (position.Y < 0) {
                position.Y = 0;
                velocity.Y = 0;
            }
            if (position.Y > Shared.stage.Y - 0) {
                position.Y = Shared.stage.Y - 0;
                velocity.Y = 0;
            }
        }
    }
}