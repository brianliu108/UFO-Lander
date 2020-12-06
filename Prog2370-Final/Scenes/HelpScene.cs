using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prog2370_Final.Drawable;

namespace Prog2370_Final.Scenes
{
    class HelpScene : Scene
    {
        string msg = "To play, use the arrow keys and the spacebar!\n\n" +
            "Left and Right tilts the UFO\n" +
            "Up is for full throttle\n" +
            "Spacebar is for half throttle\n\n" +
            "Must be fully landed to collect gas";
        SimpleString message;
        public HelpScene(Game game, SpriteBatch spriteBatch) : base(game)
        {
            this.spriteBatch = spriteBatch;

            message = new SimpleString(game, spriteBatch, resources.RegularFont, new Vector2(200,200), msg, ColourSchemes.boldColour);
            Components.Add(message);
        }
    }
}
