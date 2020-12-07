using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Prog2370_Final.Drawable {
    public class SimpleString : DrawableGameComponent {
        public readonly SpriteBatch spriteBatch;

        public SpriteFont spriteFont;
        private Vector2 fakePos;
        private Vector2 position;
        private string message;
        public Color color;
        private TextAlignH horizontalAlignment;
        private TextAlignV verticalAlignment;

        public Vector2 Position {
            get => position;
            set {
                fakePos = position = value;
                if (horizontalAlignment == TextAlignH.Left && verticalAlignment == TextAlignV.Top) 
                    return;
                Vector2 size = spriteFont.MeasureString(Message);
                switch (horizontalAlignment) {
                    case TextAlignH.Middle: position.X -= size.X / 2f;
                        break;
                    case TextAlignH.Right: position.X -= size.X ;
                        break;
                }
                switch (verticalAlignment) {
                    case TextAlignV.Middle: position.Y -= size.Y / 2f;
                        break;
                    case TextAlignV.Bottom: position.Y -= size.Y;
                        break;
                }
            }
        }

        public string Message {
            get => message;
            set {
                message = value;
                Position =  fakePos;
            }
        }

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


        public override void Draw(GameTime gameTime) {
            spriteBatch.Begin();
            spriteBatch.DrawString(spriteFont, Message, Position, color);
            spriteBatch.End();
        }

        public override void Update(GameTime gameTime) { }

        public enum TextAlignH { Left, Middle, Right }

        public enum TextAlignV { Top, Middle, Bottom }
    }
}