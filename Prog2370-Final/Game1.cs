using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static System.Math;

namespace Prog2370_Final {
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private VectorImage terrain;

        private readonly Color
            DARK_RED = new Color(60, 44, 49);

        public Game1() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize() {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            int samples = 80; // How many points will be used to make the curve. More points means smoother curve.
            float range = 50; // Essentially the y scale. How far up and down will the graph go.
            float domain = GraphicsDevice.Viewport.Bounds.Width; // The x range. Starts at 0 pixels, ends at _ pixels.
            float period = 5; // The period of the base sine wave. Essentially how dense will the hills be.
            float seed = 0; // The starting point of the curve. Different values will give different terrains.
            Vector2[] vertices = new Vector2[samples];
            for (float i = 0, x = 0; i < samples; i++, x = period * (float) PI * i / samples + seed)
                vertices[(int) i] = new Vector2(
                    domain * i / (samples - 1),
                    range * -(float) (Cos(.95 * x) - Cos(2.11 * (x - PI / 2)) / 2 + Cos(11 * x) / 7 +
                                      Cos(5.5 * x) / 4 * Cos(x - PI / 2) - Cos(0.75 * x) / 2 + Cos(x / 3) / 2 -
                                      Cos(17 * x) / 13 +
                                      Cos(34 * x) / 13));


            terrain = new VectorImage(this, spriteBatch, vertices, 4, new Color(130, 52, 65));
            terrain.offset = new Vector2(0, GraphicsDevice.Viewport.Bounds.Height * 0.75f);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent() {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(DARK_RED);

            // TODO: Add your drawing code here

            terrain.Draw(gameTime);

            base.Draw(gameTime);
        }
    }
}