using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prog2370_Final.Drawable;

namespace Prog2370_Final.Scenes {
    /// <summary>
    /// Scene to show gameplay instructions
    /// </summary>
    class HelpScene : Scene {
        string msg = "To play, use the arrow keys and the spacebar!\n\n" +
                     "Left and Right tilts the UFO\n" +
                     "Up is for full throttle\n" +
                     "Spacebar is for half throttle\n\n" +
                     "Must be fully landed to collect gas";

        SimpleString message;

        /// <summary>
        /// Creates the Scene
        /// </summary>
        /// <param name="game">Current game reference</param>
        /// <param name="spriteBatch">spriteBatch to draw with</param>
        public HelpScene(Game game, SpriteBatch spriteBatch) : base(game) {
            this.spriteBatch = spriteBatch;

            // Creating message
            message = new SimpleString(game, spriteBatch, resources.RegularFont, new Vector2(200, 200), msg,
                ColourSchemes.boldColour);
            Components.Add(message);
        }
    }
}