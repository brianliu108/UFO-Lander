using System;
using System.Collections.Generic;
using System.Xml.Schema;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Prog2370_Final.Drawable;
using Prog2370_Final.Drawable.Sprites;


namespace Prog2370_Final.Scenes {
    public class GameScene : Scene {
        
        private InfiniteTerrain terrain;
        private CollisionManager collisionManager;
        private UFO ufo;
        private KeyboardState ks;
        int deadCounter = 0;
        SimpleString died;

        private MeterBar mb;
        private MeterBar gasBar;
        private SimpleString distance;

        private Explosion explosion;
        private MouseState oldState; // temp

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


            ufo = new UFO(Game, spriteBatch, new Vector2(50, 200));
            // GasCan gasCan = new GasCan(Game, spriteBatch, new Vector2(200, GraphicsDevice.Viewport.Bounds.Height - 50));
            // this.Components.Add(gasCan);
            this.Components.Add(ufo);

            Components.Add(mb = new MeterBar(
                new SimpleString(game, spriteBatch, resources.RegularFont, 
                    new Vector2(20, 70), "Speed: ", Color.Black),
                0, ufo.MaxVelocity
                ));

            // Create gas meter
            Components.Add(gasBar = new MeterBar(
                new SimpleString(game, spriteBatch, resources.RegularFont, new Vector2(20, 120),
                "Gas: ", Color.Black), 0, ufo.Gas));
            
            Components.Add(distance = new SimpleString(game, spriteBatch, resources.BoldFont,new Vector2(20,20),"Distance: 0",ColourSchemes.pink ));
            
            // Create collision manager
            Components.Add(collisionManager = new CollisionManager(Game));
            collisionManager.Add(ufo);
            // collisionManager.Add(gasCan);

            explosion = new Explosion(game, spriteBatch, resources.Explosion, Vector2.Zero, 3);
            this.Components.Add(explosion);

            died = new SimpleString(game, spriteBatch, resources.BoldFont, new Vector2(Shared.stage.X / 2, Shared.stage.Y / 2), "You Died", ColourSchemes.boldColour);
                     
        }

        public override void Update(GameTime gameTime) {
            Components.RemoveAll(component => component is IPerishable p && p.Perished);
            foreach (Terrain chunk in terrain.Chunks)
                if (!collisionManager.Contains(chunk.terrain))
                    collisionManager.Add(chunk.terrain);
            collisionManager.Update(gameTime);
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
            if (terrain.HasNewGasCan(out GasCan gasCan)) {
                Components.Add(gasCan);
                collisionManager.Add(gasCan);
            }
            mb.current = ufo.Speed;
            gasBar.current = ufo.Gas;
            distance.message = "Distance " + ((int) ufo.position.X + (int) -terrain.MasterOffset);
            mb.Update(gameTime);
            gasBar.Update(gameTime);

            

            if (ufo.Dead && deadCounter == 0)
            {
                explosion.Position = new Vector2(ufo.position.X - (explosion.Dimension.X / 2), ufo.position.Y - (explosion.Dimension.Y / 2));
                explosion.Show(true);
                deadCounter++;
                
            }            

            base.Update(gameTime);

        }

        private void DeathScene()
        {

        }
    }
}