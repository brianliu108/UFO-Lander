using System;
using System.Xml.Schema;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Prog2370_Final.Drawable;
using Prog2370_Final.Drawable.Sprites;


namespace Prog2370_Final.Scenes {
    public class GameScene : Scene {
        private SpriteBatch spriteBatch;
        private InfiniteTerrain terrain;
        private UFO ufo;
        private GasCan gasCan;
        private KeyboardState ks;

        private MeterBar mb;

        public GameScene(Game game,
            SpriteBatch spriteBatch) : base(game) {
            this.spriteBatch = spriteBatch;

            var tempTerrain = new Terrain(
                Game, spriteBatch,
                GraphicsDevice.Viewport.Bounds.Width / 3f, 50,
                80, 1, 0, 4,
                ColourSchemes.brown, new Vector2(0, GraphicsDevice.Viewport.Bounds.Height * 0.75f));

            terrain = new InfiniteTerrain(Game, spriteBatch, tempTerrain, 3, 3);

            Components.Add(terrain);


            ufo = new UFO(Game, spriteBatch, Game.Content.Load<Texture2D>("Images/UFO"), new Vector2(200, 200));
            gasCan = new GasCan(Game, spriteBatch, Game.Content.Load<Texture2D>("Images/gascan"),
                new Vector2(100, 100));
            this.Components.Add(gasCan);
            this.Components.Add(ufo);

            Components.Add(mb = new MeterBar(
                new SimpleString(game, spriteBatch, resources.RegularFont, 
                    new Vector2(20, 200), "Speed: ", Color.Black),
                0, ufo.maxVelocity
                ));
        }

        public override void Update(GameTime gameTime) {
            ks = Keyboard.GetState();
            ufo.Update(gameTime, ks);
            int ufoMinPos = 0, ufoMaxPos = 500;
            if (ufo.position.X > ufoMaxPos) {
                float dif = ufo.position.X - ufoMaxPos;
                terrain.MasterOffset -= dif;
                ufo.position.X -= dif;
            } else if (ufo.position.X < ufoMinPos) {
                float dif = ufo.position.X - ufoMinPos;
                terrain.MasterOffset -= dif;
                ufo.position.X -= dif;
            }
            mb.current = (float) Math.Sqrt(ufo.velocity.X * ufo.velocity.X + ufo.velocity.Y * ufo.velocity.Y);
            mb.Update(gameTime);
        }
    }
}