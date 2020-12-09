using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Prog2370_Final.Drawable {
    public class MeterBar : DrawableGameComponent {
        private Resources resources;


        private SpriteBatch spriteBatch;
        private SimpleString text;
        private readonly string initialMessage;
        private Rectangle drawSize;

        private readonly float min, max;
        public float current;

        public MeterBar(SimpleString text, float min, float max) : this(text,
            new Rectangle(
                text.Position.ToPoint(),
                text.spriteFont.MeasureString(text.Message).ToPoint()),
            min, max) { }

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

        public override void Update(GameTime gameTime) {
            text.Message = initialMessage + $"{current,5:##0.0}";
        }

        public override void Draw(GameTime gameTime) {
            spriteBatch.Begin();
            Rectangle line;
            Rectangle bb = line = drawSize;
            line.Width = (int) (line.Width * (Math.Min(current,max)) / (max - min));
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