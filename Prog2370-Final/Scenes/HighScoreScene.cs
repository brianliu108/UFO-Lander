using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Xml.Schema;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Prog2370_Final.Drawable;
using Prog2370_Final.Drawable.Sprites;
using System.IO;
using System.Text;

namespace Prog2370_Final.Scenes {
    /// <summary>
    /// The scene to show top 5 high scores
    /// </summary>
    public class HighScoreScene : Scene {
        private SimpleString highScores;        

        /// <summary>
        /// Create the HighScoreScene
        /// </summary>
        /// <param name="game"></param>
        /// <param name="spriteBatch"></param>
        public HighScoreScene(Game game, SpriteBatch spriteBatch) : base(game) {
            this.spriteBatch = spriteBatch;

            ReadFromFile();
        }

        /// <summary>
        /// draws the scene
        /// </summary>
        /// <param name="gameTime">Snapshot of game time</param>
        public override void Draw(GameTime gameTime) {
            highScores.Draw(gameTime);
        }

        /// <summary>
        /// Finding and showing the highscores
        /// </summary>
        public void ReadFromFile() {
            highScores = new SimpleString(Game, spriteBatch, resources.MonoFont,
                new Vector2(GraphicsDevice.Viewport.Width / 2f, GraphicsDevice.Viewport.Height / 2f),
                Resources.FormattedHighScores(),
                ColourSchemes.boldColour,
                SimpleString.TextAlignH.Middle,
                SimpleString.TextAlignV.Middle);
            this.Components.Add(highScores);
        }

        /// <summary>
        /// Updating the scene
        /// </summary>
        /// <param name="gameTime">Snapshot of time</param>
        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
        }
    }
}