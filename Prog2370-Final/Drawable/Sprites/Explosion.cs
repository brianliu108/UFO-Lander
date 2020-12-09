using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prog2370_Final.Drawable.Sprites {
    /// <summary>
    /// Creates a small explosion animation
    /// </summary>
    public class Explosion : Sprite, IPerishable {
        private Vector2 position;
        private Vector2 dimension;
        private List<Rectangle> frames;
        private int frameIndex = -1;
        private int delay;
        private int delayCounter; // delay between when to draw frames        

        private bool scaleUp = false; // whether to scale up or not

        private const int ROW = 6;
        private const int COL = 6;

        /// <summary>
        /// Creates a new explosion instance
        /// </summary>
        /// <param name="game">Reference to current game</param>
        /// <param name="spriteBatch">SpriteBatch to draw with</param>
        /// <param name="tex">texture sheet to import</param>
        /// <param name="position">Position of explosion</param>
        /// <param name="delay">delay between frames</param>
        public Explosion(Game game, SpriteBatch spriteBatch, Texture2D tex, Vector2 position, int delay) : base(game,
            spriteBatch, tex, position) {
            this.spriteBatch = spriteBatch;
            this.tex = tex;
            this.position = position;
            this.delay = delay;

            // size of frame on spritesheet
            dimension = new Vector2(tex.Width / COL, tex.Height / ROW);

            this.Show(false);

            CreateFrames();
        }

        /// <summary>
        /// Loads each instance of the texture from the spritesheet to a list; ready to be drawn at any point
        /// </summary>
        private void CreateFrames() {
            frames = new List<Rectangle>();

            for (int i = 0; i < ROW; i++) {
                for (int j = 0; j < COL; j++) {
                    int x = j * (int) dimension.X;
                    int y = i * (int) dimension.Y;

                    Rectangle rec = new Rectangle(x, y, (int) dimension.X, (int) dimension.Y);
                    frames.Add(rec);
                }
            }
        }

        /// <summary>
        /// Draws the explosion
        /// </summary>
        /// <param name="gameTime">Timing of explosion</param>
        public override void Draw(GameTime gameTime) {
            spriteBatch.Begin();

            // Drawing only when frameindex is changed to 0 or more
            if (frameIndex >= 0) {
                if (!scaleUp)
                    spriteBatch.Draw(tex, position, frames[frameIndex], Color.White);
                else
                    spriteBatch.Draw(tex, new Rectangle((int) position.X - (frames[frameIndex].Width / 2),
                        (int) position.Y - (frames[frameIndex].Height / 2), (int) (frames[frameIndex].Width * 2),
                        (int) (frames[frameIndex].Height * 2)), frames[frameIndex], Color.White);
            }

            spriteBatch.End();
        }

        /// <summary>
        /// Updates the explosion timing values
        /// </summary>
        /// <param name="gameTime">Snapshot of game timing</param>
        public override void Update(GameTime gameTime) {
            delayCounter++;
            if (delayCounter < delay) {
                frameIndex++;
                if (frameIndex > ROW * COL - 1) {
                    frameIndex = -1;
                    this.Show(false);
                }

                delayCounter = 0;
            }
        }

        /// <summary>
        /// Get perished
        /// </summary>
        public bool Perished => false;

        /// <summary>
        /// Get & Set position
        /// </summary>
        public Vector2 Position {
            get => position;
            set => position = value;
        }

        /// <summary>
        /// Get & Set dimension
        /// </summary>
        public Vector2 Dimension {
            get => dimension;
            set => dimension = value;
        }
        /// <summary>
        /// Get & Set scaleUp
        /// </summary>
        public bool ScaleUp {
            get => scaleUp;
            set => scaleUp = value;
        }
    }
}