using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Prog2370_Final.Drawable;
using Prog2370_Final.Drawable.Sprites;


namespace Prog2370_Final.Scenes {
    public class GameScene : Scene {
        private SpriteBatch spriteBatch;
        private InfiniteTerrain terrain;
        private UFO ufo;
        private GasCan gasCan;
        private Resources resource;

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

        protected override void LoadContent() {
            resource = new Resources(Game);
            ufo = new UFO(Game, spriteBatch, Game.Content.Load<Texture2D>("Images/UFO"), new Vector2(200, 200));
            gasCan = new GasCan(Game, spriteBatch, Game.Content.Load<Texture2D>("Images/gascan"),
                new Vector2(100, 100));
            this.Components.Add(gasCan);
            this.Components.Add(ufo);
        }
    }
}