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

        private int samples;
        private float range;
        private float domain;
        private float period;
        private float seed;

        public Terrain(Game game, SpriteBatch spriteBatch,
            int samples, float range, float domain, float period, float seed)
            : base(game) {
            this.game = game;
            this.spriteBatch = spriteBatch;
            this.samples = samples;
            this.range = range;
            this.domain = domain;
            this.period = period;
            this.seed = seed;
            Generate();
            // samples = 80; // How many points will be used to make the curve. More points means smoother curve.
            // range = 50; // Essentially the y scale. How far up and down will the graph go.
            // domain = GraphicsDevice.Viewport.Bounds.Width; // The x range. Starts at 0 pixels, ends at _ pixels.
            // period = 5; // The period of the base sine wave. Essentially how dense will the hills be.
            // seed = 0; // The starting point of the curve. Different values will give different terrains.
        }

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
                offset = new Vector2(0, GraphicsDevice.Viewport.Bounds.Height * 0.75f)
            };
        }

        public override void Draw(GameTime gameTime) {
            terrain.Draw(gameTime);
        }
    }
}