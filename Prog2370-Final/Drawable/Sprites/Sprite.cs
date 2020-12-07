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
    public class Sprite : DrawableGameComponent {
        protected SpriteBatch spriteBatch;
        public Texture2D tex;
        protected Resources resources;
        public Rectangle hitbox; // TODO: Find hitboxes for each of our sprites=-

        public Sprite(Game game, SpriteBatch spriteBatch, Texture2D tex, Vector2 position) : base(game) {
            resources = ((Game1) (Game)).Resources;
        }

        public static void DrawBoundingBox(Rectangle boundingBox, Game1 game, Color color) {
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

        public void Show(bool enable)
        {
            this.Enabled = enable;
            this.Visible = enable;
        }
    }
}