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
using System.Text;

namespace Prog2370_Final.Scenes
{
    public class GameScene : Scene
    {
        private InfiniteTerrain terrain;
        private CollisionManager collisionManager;
        private UFO ufo;
        private KeyboardState ks;
        private bool startFrameCount = false;
        private int frameCount = 0;
        private int deadCounter = 0;
        private SimpleString died;
        private SoundEffectInstance deathSouthIns;

        private MeterBar meterSpeed;
        private MeterBar meterGas;
        private SimpleString distance;

        private Explosion explosion;

        private int totalDistance;

        public GameScene(Game game, SpriteBatch spriteBatch) : base(game)
        {
            this.spriteBatch = spriteBatch;


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

            explosion = new Explosion(game, spriteBatch, resources.Explosion, Vector2.Zero, 3);
            this.Components.Add(explosion);

            // Death components
            died = new SimpleString(game, spriteBatch, resources.DeathFont,
                new Vector2(Shared.stage.X / 2 - 120, Shared.stage.Y / 2 - 100), "You Died", Color.Gray);
            deathSouthIns = resources.deathSound.CreateInstance();
            deathSouthIns.Volume = .2f;
        }

        public int TotalDistance { get => totalDistance; }

        public override void Update(GameTime gameTime)
        {
            Components.RemoveAll(component => component is IPerishable p && p.Perished);
            foreach (Terrain chunk in terrain.Chunks)
                if (!collisionManager.Contains(chunk.terrain))
                    collisionManager.Add(chunk.terrain);
            collisionManager.Update(gameTime);
            ks = Keyboard.GetState();
            ufo.Update(gameTime, ks);
            int ufoMinPos = 0, ufoMaxPos = 500;
            if (ufo.position.X > ufoMaxPos)
            {
                float dif = ufo.position.X - ufoMaxPos;
                terrain.MasterOffset -= dif;
                ufo.position.X -= dif;
            }
            else if (ufo.position.X < ufoMinPos)
            {
                float dif = ufo.position.X - ufoMinPos;
                terrain.MasterOffset -= dif;
                ufo.position.X -= dif;
            }
            if (terrain.HasNewGasCan(out GasCan gasCan))
            {
                Components.Add(gasCan);
                collisionManager.Add(gasCan);
            }

            totalDistance = ((int)ufo.position.X + (int)-terrain.MasterOffset);

            meterSpeed.current = ufo.Speed;
            meterGas.current = ufo.Gas;
            distance.Message = "Distance " + totalDistance;
            meterSpeed.Update(gameTime);
            meterGas.Update(gameTime);

            if (ufo.Dead && deadCounter == 0)
            {
                explosion.Position = new Vector2(ufo.position.X - (explosion.Dimension.X / 2),
                    ufo.position.Y - (explosion.Dimension.Y / 2));
                if (ufo.Speed > 5)
                    explosion.ScaleUp = true;
                else
                    explosion.ScaleUp = false;

                explosion.Show(true);


                deadCounter++;
                startFrameCount = true;
            }
            if (startFrameCount)
            {
                frameCount++;

                // Play deathsound
                if (frameCount == 60)
                {
                    deathSouthIns.Play();
                }
                // Show you died
                if (frameCount == 120)
                {
                    DeathScene();
                }
                // After deathsound finishes
                if (frameCount == 550) {
                    ((Game1) Game).ForcefulSceneChange = 3; //FLAG this may cause interesting bugs if we forget about it
                    startFrameCount = false;
                }
            }
            base.Update(gameTime);
        }

        private void DeathScene()
        {
            this.Components.Add(died);

        }
    }
}