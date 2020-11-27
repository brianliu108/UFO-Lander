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
            LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            ks = Keyboard.GetState();

            if (ks.IsKeyDown(Keys.Up))
            {
                //ufo.Tex = 
            }
        }

        protected override void LoadContent() {
           
            ufo = new UFO(Game, spriteBatch, Game.Content.Load<Texture2D>("Images/UFO"), new Vector2(200, 200));
            gasCan = new GasCan(Game, spriteBatch, Game.Content.Load<Texture2D>("Images/gascan"),
                new Vector2(100, 100));
            this.Components.Add(gasCan);
            this.Components.Add(ufo);
        }
    }
}