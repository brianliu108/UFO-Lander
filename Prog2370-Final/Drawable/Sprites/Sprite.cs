using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Prog2370_Final;

namespace Prog2370_Final.Drawable.Sprites {
    /// <summary>
    /// Generic drawable game component with a texture
    /// </summary>
    public class Sprite : DrawableGameComponent {
        protected SpriteBatch spriteBatch;
        public Texture2D tex;
        protected Resources resources; // collection of various resources

        /// <summary>
        /// Creation of generic DrawableGameComponent
        /// </summary>
        /// <param name="game">reference to current game</param>
        /// <param name="spriteBatch">spritebatch to draw with</param>
        /// <param name="tex">texture to import</param>
        /// <param name="position">texture of sprite</param>
        public Sprite(Game game, SpriteBatch spriteBatch, Texture2D tex, Vector2 position) : base(game) {
            this.spriteBatch = spriteBatch;
            resources = ((UfoLander) (Game)).Resources;
            this.tex = tex;
        }

        /// <summary>
        /// Draw an Axis Aligned Bounding Box
        /// </summary>
        /// <param name="boundingBox">Box dimensions</param>
        /// <param name="game">Current game reference</param>
        /// <param name="color">Color of box</param>
        public static void DrawBoundingBox(Rectangle boundingBox, UfoLander game, Color color) {
            game.spriteBatch.Begin();
            Rectangle line, bb = line = boundingBox;
            // Y
            line = bb;
            line.Height = 1;
            game.spriteBatch.Draw(game.Resources.WhitePixel, line, color);
            line.Y += bb.Height;
            game.spriteBatch.Draw(game.Resources.WhitePixel, line, color);
            // X
            line = bb;
            line.Width = 1;
            game.spriteBatch.Draw(game.Resources.WhitePixel, line, color);
            line.X += bb.Width;
            game.spriteBatch.Draw(game.Resources.WhitePixel, line, color);
            game.spriteBatch.End();
        }

        /// <summary>
        /// Whether to show and enable the sprite or not
        /// </summary>
        /// <param name="enable">Whether to execute the method</param>
        public void Show(bool enable) {
            this.Enabled = enable;
            this.Visible = enable;
        }
    }
}