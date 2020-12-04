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
        public Vector2 position;
        public string message;
        public Color color;

        public SimpleString(Game game,
            SpriteBatch spriteBatch,
            SpriteFont spriteFont,
            Vector2 position,
            string message,
            Color color) : base(game) {
            this.spriteBatch = spriteBatch;
            this.spriteFont = spriteFont;
            this.position = position;
            this.message = message;
            this.color = color;
        }

        public override void Draw(GameTime gameTime) {
            spriteBatch.Begin();

            spriteBatch.DrawString(spriteFont, message, position, color);

            spriteBatch.End();
        }

        public override void Update(GameTime gameTime) 
        {
            
        }
    }
}