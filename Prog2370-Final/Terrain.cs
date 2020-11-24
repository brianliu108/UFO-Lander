using System.CodeDom.Compiler;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static System.Math;

namespace Prog2370_Final {
    public class Terrain : DrawableGameComponent {
        private Game game;
        private SpriteBatch spriteBatch;
        private VectorImage terrain;

        public float domain;
        public float range;
        public int samples;
        public float period;
        public float seed;
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
        /// <param name="color">The color of the lines.</param>
        /// <param name="offset">The offset from the drawn origin/</param>
        public Terrain(Game game, SpriteBatch spriteBatch,
            float domain, float range,int samples,  float period, float seed, Color color, Vector2 offset)
            : base(game) {
            this.game = game;
            this.spriteBatch = spriteBatch;
            this.samples = samples;
            this.range = range;
            this.domain = domain;
            this.period = period;
            this.seed = seed;
            this.color = color;
            this.offset = offset;
            Generate();
        }

        /// <summary>
        /// Generates the vectors that form the terrain.
        /// </summary>
        public void Generate() {
            Vector2[] vertices = new Vector2[samples];
            for (float i = 0, x = 0; i < samples; i++, x = period * (float) PI * i / samples + seed)
                vertices[(int) i] = new Vector2(
                    domain * i / (samples - 1),
                    range * -(float) (Cos(.95 * x)
                                      - Cos(2.11 * (x - PI / 2)) / 2
                                      + Cos(11 * x) / 7
                                      + Cos(5.5 * x) / 4 * Cos(x - PI / 2)
                                      - Cos(0.75 * x) / 2
                                      + Cos(x / 3) / 2
                                      - Cos(17 * x) / 13
                                      + Cos(34 * x) / 13));
            terrain = new VectorImage(game, spriteBatch, vertices, 4, new Color(130, 52, 65)) {
                offset = offset
            };
        }

        /// <summary>
        /// Draws the terrain.
        /// </summary>
        /// <param name="gameTime">Unused.</param>
        public override void Draw(GameTime gameTime) {
            terrain.Draw(gameTime);
        }
    }
}