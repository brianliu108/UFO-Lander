using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Prog2370_Final.Scenes;
using Prog2370_Final.Drawable;
using Prog2370_Final.Drawable.Sprites;
using static System.Math;
using Prog2370_Final;

namespace Prog2370_Final {
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Resources _resources;
        private KeyboardState oldState;

        private StartScene startScene;
        private GameScene playScene;
        private CreditsScene creditsScene;

        private SimpleString creditsString;

        public Resources Resources => _resources;

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
            Shared.stage = new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);

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

            _resources = new Resources(this);

            // Add startScene
            startScene = new StartScene(this, spriteBatch);
            Components.Add(startScene);
            startScene.Show(true);

            // Add playScene
            playScene = new GameScene(this, spriteBatch);
            Components.Add(playScene);
            playScene.Show(false);

            // Add creditsScene 
            creditsScene = new CreditsScene(this, spriteBatch);
            Components.Add(creditsScene);
            creditsScene.Show(false);


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
            if (Keyboard.GetState().IsKeyDown(Keys.Escape) && oldState.IsKeyUp(Keys.Escape)) {
                if (playScene.Enabled || creditsScene.Enabled) {
                    HideAllScenes();
                    startScene.Show(true);
                } else {
                    Exit();
                }
            }

            // TODO: Add your update logic here

            var ks = Keyboard.GetState();
            var selectedIndex = 0;

            if (startScene.Enabled) {
                selectedIndex = startScene.Menu.SelectedIndex;
                if (selectedIndex == 0 && ks.IsKeyDown(Keys.Enter) && oldState.IsKeyUp(Keys.Enter)) {
                    HideAllScenes();
                    playScene.Show(true);
                } else if (selectedIndex == 1 && ks.IsKeyDown(Keys.Enter) && oldState.IsKeyUp(Keys.Enter)) {
                    HideAllScenes();
                    creditsScene.Show(true);
                } else if (selectedIndex == 2 && ks.IsKeyDown(Keys.Enter) && oldState.IsKeyUp(Keys.Enter)) {
                    Exit();
                }
            }
            oldState = ks;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(ColourSchemes.darkRed);

            // TODO: Add your drawing code here


            base.Draw(gameTime);
        }

        private void HideAllScenes() {
            foreach (Scene item in Components) item.Show(false);
        }
    }
}