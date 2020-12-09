using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Prog2370_Final.Drawable {
    /// <summary>
    /// A drawable string object.
    /// </summary>
    public class SimpleString : DrawableGameComponent {
        public readonly SpriteBatch spriteBatch;

        public SpriteFont spriteFont;
        private Vector2 fakePos;
        private Vector2 position;
        private string message;
        public Color color;
        private TextAlignH horizontalAlignment;
        private TextAlignV verticalAlignment;

        /// <summary>
        /// The position that the text is drawn at. Note: When text is centered in any other wy than top left, this
        /// position reflects the drawing position, not the apparent position relative to alignment. For that you
        /// want <c>fakePos</c>.
        /// </summary>
        public Vector2 Position {
            get => position;
            set {
                fakePos = position = value;
                if (horizontalAlignment == TextAlignH.Left && verticalAlignment == TextAlignV.Top)
                    return;
                Vector2 size = spriteFont.MeasureString(Message);
                switch (horizontalAlignment) {
                    case TextAlignH.Middle:
                        position.X -= size.X / 2f;
                        break;
                    case TextAlignH.Right:
                        position.X -= size.X;
                        break;
                }
                switch (verticalAlignment) {
                    case TextAlignV.Middle:
                        position.Y -= size.Y / 2f;
                        break;
                    case TextAlignV.Bottom:
                        position.Y -= size.Y;
                        break;
                }
            }
        }

        /// <summary>
        /// The text that the string holds.
        /// </summary>
        public string Message {
            get => message;
            set {
                message = value;
                Position = fakePos;
            }
        }

        /// <summary>
        /// Constructor for a displayable string.
        /// </summary>
        /// <param name="game">The game that is creating the string</param>
        /// <param name="spriteBatch">The spritebatch used to draw the string</param>
        /// <param name="spriteFont">The font to draw the string</param>
        /// <param name="position">The location of the string. (See alignment args)</param>
        /// <param name="message">The text that the string displays</param>
        /// <param name="color">The color of the text</param>
        /// <param name="horizontalAlignment">How to position the string relative to the position</param>
        /// <param name="verticalAlignment">How to position the string relative to the position</param>
        public SimpleString(Game game,
            SpriteBatch spriteBatch,
            SpriteFont spriteFont,
            Vector2 position,
            string message,
            Color color,
            TextAlignH horizontalAlignment = TextAlignH.Left,
            TextAlignV verticalAlignment = TextAlignV.Top)
            : base(game) {
            this.spriteBatch = spriteBatch;
            this.spriteFont = spriteFont;
            this.Message = message;
            this.color = color;
            this.horizontalAlignment = horizontalAlignment;
            this.verticalAlignment = verticalAlignment;
            this.Position = position;
        }

        /// <summary>
        /// Draws the simple string
        /// </summary>
        /// <param name="gameTime">Unused.</param>
        public override void Draw(GameTime gameTime) {
            spriteBatch.Begin();
            spriteBatch.DrawString(spriteFont, Message, Position, color);
            spriteBatch.End();
        }

        /// <summary> Simple enum for the types of horizontal orientation. </summary>
        public enum TextAlignH { Left, Middle, Right }

        /// <summary> Simple enum for the types of vertical orientation. </summary>
        public enum TextAlignV { Top, Middle, Bottom }
    }
}