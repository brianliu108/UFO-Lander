using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
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
        public SpriteBatch spriteBatch;
        private Resources _resources;
        public KeyboardState oldState;
        private StartScene startScene;
        private GameScene playScene;
        private CreditsScene creditsScene;
        private HelpScene helpScene;
        private HighScoreScene highScoreScene;
        private SoundEffectInstance enterSoundIns;
        public Song menuMusic;

        public Resources Resources => _resources;

        public int ForcefulSceneChange { get; set; }

        public Game1() {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
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

            _resources = new Resources(this);

            if (!File.Exists(Resources.SaveFileLocation)) File.Create(Resources.SaveFileLocation);

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

            // Add helpScene
            helpScene = new HelpScene(this, spriteBatch);
            Components.Add(helpScene);
            helpScene.Show(false);

            // Add highScoreScene
            highScoreScene = new HighScoreScene(this, spriteBatch);
            Components.Add(highScoreScene);
            highScoreScene.Show(false);

            // Create enterSound
            enterSoundIns = this.Content.Load<SoundEffect>("Sounds/enterSound").CreateInstance();

            // Create and play music
            menuMusic = this.Content.Load<Song>("Sounds/pauseMenu");
            MediaPlayer.Play(menuMusic);
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
                if (playScene.Enabled || creditsScene.Enabled || helpScene.Enabled || highScoreScene.Enabled) {
                    HideAllScenes();
                    startScene.Show(true);
                } else {
                    Exit();
                }
            }

            // TODO: Add your update logic here

            var ks = Keyboard.GetState();

            if (startScene.Enabled || ForcefulSceneChange != 0) {
                var selectedIndex = startScene.Menu.SelectedIndex;
                if (ks.IsKeyDown(Keys.Enter) && oldState.IsKeyUp(Keys.Enter) || ForcefulSceneChange != 0) {
                    if (ForcefulSceneChange != 0) {
                        selectedIndex = ForcefulSceneChange;
                        ForcefulSceneChange = 0;
                    }
                    switch (selectedIndex) {
                        case 0:
                            HideAllScenes();
                            playScene.Show(true);
                            enterSoundIns.Play();
                            break;
                        case 1:
                            HideAllScenes();
                            Components.Remove(playScene);
                            Components.Add(playScene = new GameScene(this, spriteBatch));
                            playScene.Show(true);
                            enterSoundIns.Play();

                            MediaPlayer.Play(menuMusic);
                            break;
                        case 2:
                            HideAllScenes();
                            helpScene.Show(true);
                            enterSoundIns.Play();
                            break;
                        case 3:
                            HideAllScenes();
                            highScoreScene.Show(true);
                            enterSoundIns.Play();
                            highScoreScene.ReadFromFile();
                            break;
                        case 4:
                            HideAllScenes();
                            creditsScene.Show(true);
                            enterSoundIns.Play();
                            break;
                        case 5:
                            Exit();
                            break;
                    }
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