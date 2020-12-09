using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Prog2370_Final.Drawable {
    /// <summary>
    /// Draws a rectangle that acts as a single bar in a bar graph, growing to the max size when the max value is set,
    /// displaying empty when the min is set, and dynamically sizing for every other value between the two.
    /// </summary>
    public class MeterBar : DrawableGameComponent {
        private Resources resources;

        private SpriteBatch spriteBatch;
        private SimpleString text;
        private readonly string initialMessage;
        private Rectangle drawSize;

        private readonly float min, max;
        public float current;

        /// <summary>
        /// Creates a new MeterBar with the specified simple string, min value, and max value. The string is used
        /// to determine the size and location fo the bar.
        /// </summary>
        /// <param name="text">The simple string to be paired with</param>
        /// <param name="min">Min value. Represents 0% fill</param>
        /// <param name="max">Max value. Represents 100% fill.</param>
        public MeterBar(SimpleString text, float min, float max) : this(text,
            new Rectangle(
                text.Position.ToPoint(),
                text.spriteFont.MeasureString(text.Message).ToPoint()),
            min, max) { }

        /// <summary>
        /// Creates a new MeterBar with the specified simple string, min value, and max value. 
        /// </summary>
        /// <param name="text">The simple string to be paired with</param>
        /// <param name="drawSize">The size and location of the bar when full</param>
        /// <param name="min">Min value. Represents 0% fill</param>
        /// <param name="max">Max value. Represents 100% fill.</param>
        public MeterBar(SimpleString text, Rectangle drawSize, float min, float max) : base(text.Game) {
            resources = ((Game1) text.Game).Resources;
            this.spriteBatch = text.spriteBatch;
            initialMessage = text.Message;
            this.text = text;
            this.min = current = min;
            this.max = max;
            text.Message = initialMessage + current;
            this.drawSize = drawSize;
        }

        /// <summary>
        /// Updates the message to display the new value.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime) {
            text.Message = initialMessage + $"{current,5:##0.0}";
        }

        /// <summary>
        /// Draws the Simple string, and the MeterBar
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime) {
            spriteBatch.Begin();
            Rectangle line;
            Rectangle bb = line = drawSize;
            line.Width = (int) (line.Width * (Math.Min(current, max)) / (max - min));
            spriteBatch.Draw(resources.WhitePixel, line, ColourSchemes.pink);
            // Y
            line = bb;
            line.Height = 1;
            spriteBatch.Draw(resources.WhitePixel, line, ColourSchemes.normRed);
            line.Y += bb.Height;
            spriteBatch.Draw(resources.WhitePixel, line, ColourSchemes.normRed);
            // X
            line = bb;
            line.Width = 1;
            spriteBatch.Draw(resources.WhitePixel, line, ColourSchemes.normRed);
            line.X += bb.Width;
            spriteBatch.Draw(resources.WhitePixel, line, ColourSchemes.normRed);
            spriteBatch.End();

            text.Draw(gameTime);
        }
    }
}