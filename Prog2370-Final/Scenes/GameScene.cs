using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Xml.Schema;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Prog2370_Final.Drawable;
using Prog2370_Final.Drawable.Sprites;
using System.IO;
using System.Linq;
using System.Text;

namespace Prog2370_Final.Scenes {
    /// <summary>
    /// The scene to show the gameplay
    /// </summary>
    public class GameScene : Scene {
        private InfiniteTerrain terrain;
        private CollisionManager collisionManager;
        private UFO ufo;
        private KeyboardState ks;
        private bool startFrameCount = false;
        private int frameCount = 0;
        private int deadCounter = 0;
        
        private SoundEffectInstance deathSouthIns;

        private MeterBar meterSpeed;
        private MeterBar meterGas;
        private SimpleString distance;
        private KbInputString inputNameString;
        
        private Explosion explosion;

        private int totalDistance;
        private int finalDistance;

        private bool recorded = false;

        /// <summary>
        /// Creates the GameScene scene
        /// </summary>
        /// <param name="game">Current game reference</param>
        /// <param name="spriteBatch">spriteBatch to draw scene</param>
        public GameScene(Game game, SpriteBatch spriteBatch) : base(game) {
            this.spriteBatch = spriteBatch;

            inputNameString = new KbInputString(Game, spriteBatch,
                resources.MonoFont,
                new Vector2(GraphicsDevice.Viewport.Width / 2f,
                    GraphicsDevice.Viewport.Height / 2f + resources.MonoFont.LineSpacing * 2),
                Color.Wheat,
                SimpleString.TextAlignH.Middle);
            
            // Add the infinite terrain (which also does gas cans)
            var tempTerrain = new Terrain(Game, spriteBatch,
                GraphicsDevice.Viewport.Bounds.Width / 3f, 50,
                80, 1, 0, 4,
                ColourSchemes.brown, new Vector2(0, GraphicsDevice.Viewport.Bounds.Height * 0.75f));
            Components.Add(terrain = new InfiniteTerrain(Game, spriteBatch, tempTerrain, 3, 3));
            // Add our UFO
            this.Components.Add(ufo = new UFO(Game, spriteBatch,
                new Vector2(50, terrain.ExtremeHeightAt(25, 50, false))));

            // Add distance string
            Components.Add(
                distance = new SimpleString(game, spriteBatch,
                    resources.BoldFont,
                    new Vector2(20, 20),
                    "Distance: 0",
                    ColourSchemes.pink));            

            // Create Speed meter & string
            Components.Add(
                meterSpeed = new MeterBar(
                    new SimpleString(game, spriteBatch,
                        resources.RegularFont,
                        new Vector2(215, 70),
                        "", Color.Black,
                        SimpleString.TextAlignH.Right),
                    new Rectangle(20, 70, 200, resources.RegularFont.LineSpacing),
                    0, ufo.MaxVelocity));
            Components.Add(new SimpleString(game, spriteBatch,
                resources.RegularFont,
                new Vector2(25, 70),
                "Speed", Color.Black));

            // Create Gas meter & string
            Components.Add(
                meterGas = new MeterBar(
                    new SimpleString(game, spriteBatch,
                        resources.RegularFont,
                        new Vector2(215, 120),
                        "", Color.Black,
                        SimpleString.TextAlignH.Right),
                    new Rectangle(20, 120, 200, resources.RegularFont.LineSpacing),
                    0, ufo.Gas));
            Components.Add(new SimpleString(game, spriteBatch,
                resources.RegularFont,
                new Vector2(25, 120),
                "Gas: ", Color.Black));


            // Create collision manager
            Components.Add(collisionManager = new CollisionManager(Game));
            collisionManager.Add(ufo);

            // Create explosion
            explosion = new Explosion(game, spriteBatch, resources.Explosion, Vector2.Zero, 3);
            this.Components.Add(explosion);

            // Death sound
            deathSouthIns = resources.deathSound.CreateInstance();
            deathSouthIns.Volume = .2f;
        }

        /// <summary>
        /// Get the total distance travelled
        /// </summary>
        public int TotalDistance {
            get => finalDistance == 0 ? totalDistance : finalDistance;
        }

        /// <summary>
        /// Updates the gamescene logic
        /// </summary>
        /// <param name="gameTime">snapshot of game timing</param>
        public override void Update(GameTime gameTime) {
            // removing all IPerishable items if perished
            Components.RemoveAll(component => component is IPerishable p && p.Perished);
            // creating terrain chunks when moving
            foreach (Terrain chunk in terrain.Chunks)
                if (!collisionManager.Contains(chunk.terrain))
                    collisionManager.Add(chunk.terrain);
            collisionManager.Update(gameTime);
            ks = Keyboard.GetState();
            ufo.Update(gameTime, ks);
            int ufoMinPos = 0, ufoMaxPos = 500;
            // moving the terrain
            if (ufo.position.X > ufoMaxPos) {
                float dif = ufo.position.X - ufoMaxPos;
                terrain.MasterOffset -= dif;
                ufo.position.X -= dif;
            } else if (ufo.position.X < ufoMinPos) {
                float dif = ufo.position.X - ufoMinPos;
                terrain.MasterOffset -= dif;
                ufo.position.X -= dif;
            }
            if (terrain.HasNewGasCan(out GasCan gasCan)) {
                Components.Add(gasCan);
                collisionManager.Add(gasCan);
            }

            totalDistance = ((int) ufo.position.X + (int) -terrain.MasterOffset);

            meterSpeed.current = ufo.Speed;
            meterGas.current = ufo.Gas;
            distance.Message = "Distance " + TotalDistance;
            meterSpeed.Update(gameTime);
            meterGas.Update(gameTime);

            // creating explosion on death
            if (ufo.Dead && deadCounter == 0) {
                explosion.Position = new Vector2(ufo.position.X - (explosion.Dimension.X / 2),
                    ufo.position.Y - (explosion.Dimension.Y / 2));
                if (ufo.Speed > 5)
                    explosion.ScaleUp = true;
                else
                    explosion.ScaleUp = false;

                explosion.Show(true);


                deadCounter++;
                startFrameCount = true;
                finalDistance = totalDistance;
            }
            // creating a buffer between death and related events
            if (startFrameCount) {
                frameCount++;

                // Play deathsound
                if (frameCount == 60) {
                    deathSouthIns.Play();
                }
                // Show you died
                if (frameCount == 120) {
                    ShowDeathText();
                }
                // After deathsound finishes
                if (frameCount == 550) {
                    // calculating if player was a top scorer
                    var topScorers = Resources.ParseHighScores();
                    if (topScorers.Count < 10 || TotalDistance > topScorers.Min(tuple => tuple.Item2)) {
                        bool topScorer = topScorers.Count == 0 || 
                                         TotalDistance > topScorers.Max(tuple => tuple.Item2);
                        Components.Add(new SimpleString(Game, spriteBatch,
                            resources.MonoFont,
                            new Vector2(GraphicsDevice.Viewport.Width / 2f, GraphicsDevice.Viewport.Height / 2f),
                            (topScorer ? "Top Score!" : "Top 10 score! ") +
                            "\nEnter your name to be added to the leaderboard:\n\n[Press enter]",
                            Color.Wheat,
                            SimpleString.TextAlignH.Middle));
                        Components.Add(inputNameString);
                    } else {                        
                        ((UfoLander) Game).ForcefulSceneChange = 3; 
                    }
                    // end of timing related events                    
                    startFrameCount = false;
                }
            }
            // recording the player's score
            if (ufo.Dead && Keyboard.GetState().IsKeyDown(Keys.Enter) && !recorded) {
                recorded = inputNameString.locked = true;
                string name = inputNameString.Message == "" ? "PLAYER" : inputNameString.Message;
                Resources.AddToHighScoreFile(name,TotalDistance);                
                ((UfoLander) Game).ForcefulSceneChange = 3; 
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// Show the death message and total score to player
        /// </summary>
        private void ShowDeathText() {
            Components.Add(new SimpleString(Game, spriteBatch,
                resources.DeathFont,
                new Vector2(GraphicsDevice.Viewport.Width / 2f,
                    GraphicsDevice.Viewport.Height / 2f - resources.DeathFont.LineSpacing * 2),
                "You Died",
                Color.Gray,
                SimpleString.TextAlignH.Middle,
                SimpleString.TextAlignV.Bottom));
            Components.Add(new SimpleString(Game, spriteBatch,
                resources.DeathFont,
                new Vector2(GraphicsDevice.Viewport.Width / 2f,
                    GraphicsDevice.Viewport.Height / 2f - resources.DeathFont.LineSpacing),
                "Score: " + TotalDistance,
                Color.Gray,
                SimpleString.TextAlignH.Middle,
                SimpleString.TextAlignV.Bottom));
        }
    }
}