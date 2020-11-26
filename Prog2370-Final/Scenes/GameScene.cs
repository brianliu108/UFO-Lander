using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Prog2370_Final.Drawable;

namespace Prog2370_Final.Scenes {
    public class GameScene : Scene {
        private SpriteBatch spriteBatch;
        private InfiniteTerrain terrain;

        public GameScene(Game game,
            SpriteBatch spriteBatch) : base(game) {
            this.spriteBatch = spriteBatch;

            var tempTerrain = new Terrain(
                Game, spriteBatch,
                GraphicsDevice.Viewport.Bounds.Width / 3f, 50,
                80, 1, 0,
                ColourSchemes.normRed, new Vector2(0, GraphicsDevice.Viewport.Bounds.Height * 0.75f));

            terrain = new InfiniteTerrain(Game, spriteBatch, tempTerrain, 3, 3);

            Components.Add(terrain);
        }

        protected override void LoadContent()
        {
            //var gasCan = new GasCan(this, spriteBatch, gasCanTex, new Vector2(200, 200));
            //this.Components.Add(gasCan);
        }
    }
}