using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static System.Math;

namespace Prog2370_Final {
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private VectorImage star;
        private KeyboardState oldState;

        private readonly Color
            DARK_RED = new Color(60, 44, 49);

        private StartScene startScene;
        private PlayScene playScene;
        private CreditsScene creditsScene;

        private SpriteFont boldFont;
        private SimpleString creditsString;
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
            Texture2D gasCanTex = this.Content.Load<Texture2D>("Images/gascan");

            // Add SimpleString to creditsScene
            boldFont = this.Content.Load<SpriteFont>("Fonts/BoldFont");
            creditsString = new SimpleString(this,spriteBatch, boldFont, new Vector2(220,220), "Made By:\nTim Skibik\nBrian Liu", ColourSchemes.boldColour);
            

            startScene = new StartScene(this, spriteBatch);
            Components.Add(startScene);
            startScene.Show(true);

            playScene = new PlayScene(this, spriteBatch);
            Components.Add(playScene);
            playScene.Show(false);
            GasCan gasCan = new GasCan(this, spriteBatch, gasCanTex, new Vector2(200,200));
            playScene.Components.Add(gasCan);


            creditsScene = new CreditsScene(this, spriteBatch);
            this.Components.Add(creditsScene);
            creditsScene.Show(false);

            creditsScene.Components.Add(creditsString);
            
            star = new VectorImage(this, spriteBatch,
                new[] {
                    new Vector2(0, -100),
                    new Vector2(58.7785f, 80.9017f),
                    new Vector2(-95.1057f, -30.9017f),
                    new Vector2(95.1057f, -30.9017f),
                    new Vector2(-58.7785f, 80.9017f),
                    new Vector2(0, -100)
                }, 2, new Color(130, 52, 65)) {
                offset = new Vector2(200, 200),
                scale = new Vector2(0.15f, 0.15f)
            };
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
            if (Keyboard.GetState().IsKeyDown(Keys.Escape) && oldState.IsKeyUp(Keys.Escape))
            {
                if (playScene.Enabled || creditsScene.Enabled)
                {
                    HideAllScenes();
                    startScene.Show(true);
                }
                else
                {
                    Exit();
                }
            }

            // TODO: Add your update logic here

            var ks = Keyboard.GetState();
            var selectedIndex = 0;

            if (startScene.Enabled) {
                selectedIndex = startScene.Menu.SelectedIndex;
                if (selectedIndex == 0 && ks.IsKeyDown(Keys.Enter) && oldState.IsKeyUp(Keys.Enter))
                {
                    HideAllScenes();
                    playScene.Show(true);
                }
                else if (selectedIndex == 1 && ks.IsKeyDown(Keys.Enter) && oldState.IsKeyUp(Keys.Enter))
                {
                    HideAllScenes();
                    creditsScene.Show(true);
                }
                else if (selectedIndex == 2 && ks.IsKeyDown(Keys.Enter) && oldState.IsKeyUp(Keys.Enter))
                {
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
            GraphicsDevice.Clear(DARK_RED);

            // TODO: Add your drawing code here

            star.Draw(gameTime);
            
            base.Draw(gameTime);
        }

        private void HideAllScenes() {
            foreach (GameScene item in Components) item.Show(false);
        }
    }
}