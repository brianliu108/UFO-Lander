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

namespace Prog2370_Final.Drawable.Sprites
{

    public class Sprite : DrawableGameComponent
    {
        protected SpriteBatch spriteBatch;
        protected Texture2D tex;
        protected Resources resources;

        public Sprite(Game game, SpriteBatch spriteBatch, Texture2D tex, Vector2 position) : base(game)
        {
            resources = ((Game1)(Game)).Resources;
        }
        
    }
}
