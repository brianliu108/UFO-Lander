using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static System.Math;

namespace Prog2370_Final.Drawable {
    /// <summary>
    /// A dynamically generated (based on different given variables) vector based terrain
    /// </summary>
    public class Terrain : DrawableGameComponent {
        private Game game;
        private SpriteBatch spriteBatch;
        public VectorImage terrain;

        public float domain;
        public float range;
        public int samples;
        public float period;
        public float seed;
        public int width;
        public Color color;
        public Vector2 offset;

        /// <summary>
        /// Creates and generates a Vector based Terrain.
        /// </summary>
        /// <param name="game">A reference to the main game</param>
        /// <param name="spriteBatch">Spritebatch for drawing.</param>
        /// <param name="domain">The x range. Essentially it starts at 0 pixels, ends at _ pixels.</param>
        /// <param name="range">Essentially the y scale. How far up and down will the graph go.</param>
        /// <param name="samples">How many points will be used to make the curve. More points means smoother curve.</param>
        /// <param name="period">The period of the base sine wave. Essentially how dense will the hills be.</param>
        /// <param name="seed">The starting point of the curve. Different values will give different terrains.</param>
        /// <param name="width">The width of the lines</param>
        /// <param name="color">The color of the lines.</param>
        /// <param name="offset">The offset from the drawn origin/</param>
        public Terrain(Game game, SpriteBatch spriteBatch,
            float domain, float range, int samples, float period, float seed, int width, Color color, Vector2 offset)
            : base(game) {
            this.game = game;
            this.spriteBatch = spriteBatch;
            this.samples = samples;
            this.range = range;
            this.domain = domain;
            this.period = period;
            this.seed = seed;
            this.width = width;
            this.color = color;
            Offset = offset;
            Generate();
        }

        /// <summary>
        /// The offset of the origin of the image relative to the origin
        /// </summary>
        public Vector2 Offset {
            get => offset;
            set {
                offset = value;
                if (terrain != null)
                    terrain.Offset = value;
            }
        }

        /// <summary>
        /// Generates the vectors that form the terrain.
        /// </summary>
        public void Generate() {
            var vertices = new Vector2[samples + 1];
            for (float i = 0, x = seed; i <= samples; i++, x = period * (float) PI * i / samples + seed)
                vertices[(int) i] = new Vector2(
                    domain * i / samples,
                    range * -(float) (Cos(.95 * x)
                                      - Cos(2.11 * (x - PI / 2)) / 2
                                      + Cos(11 * x) / 7
                                      + Cos(5.5 * x) / 4 * Cos(x - PI / 2)
                                      - Cos(0.75 * x) / 2
                                      + Cos(x / 3) / 2
                                      - Cos(17 * x) / 13
                                      + Cos(34 * x) / 13));
            terrain = new VectorImage(game, spriteBatch, vertices, width, color) {
                Offset = Offset
            };
        }

        /// <summary>
        /// Creates new terrain identical to the current terrain.
        /// </summary>
        /// <returns></returns>
        public Terrain Clone() {
            return new Terrain(game, spriteBatch, domain, range, samples, period, seed, width, color, offset);
        }

        /// <summary>
        /// Draws the terrain.
        /// </summary>
        /// <param name="gameTime">Unused.</param>
        public override void Draw(GameTime gameTime) {
            terrain.Draw(gameTime);
        }

        /// <summary>
        /// Creates a new Terrain that, when placed directly to the right of this one, connects perfectly.
        /// </summary>
        /// <returns>A new terrain that fits to the right of the given terrain.</returns>
        public Terrain NewAdjacentRight() {
            var ter = Clone();
            ter.seed += (float) (PI * ter.period);
            ter.Generate();
            return ter;
        }

        /// <summary>
        /// Creates a new Terrain that, when placed directly to the left of this one, connects perfectly.
        /// </summary>
        /// <returns>A new terrain that fits to the left of the given terrain.</returns>
        public Terrain NewAdjacentLeft() {
            var ter = Clone();
            ter.seed -= (float) (PI * ter.period);
            ter.Generate();
            return ter;
        }
    }
}