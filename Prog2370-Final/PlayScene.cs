using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Prog2370_Final
{
    public class PlayScene : GameScene
    {
        private SpriteBatch spriteBatch;
        private Terrain terrain;

        public PlayScene(Game game,
            SpriteBatch spriteBatch) : base(game)
        {
            this.spriteBatch = spriteBatch;
            this.terrain = terrain;

            terrain = new Terrain(
                Game, spriteBatch,
                GraphicsDevice.Viewport.Bounds.Width, 50,
                80, 5, 0,
                ColourSchemes.normRed, new Vector2(0, GraphicsDevice.Viewport.Bounds.Height * 0.75f));

            this.Components.Add(terrain);
        }

        
    }
}
