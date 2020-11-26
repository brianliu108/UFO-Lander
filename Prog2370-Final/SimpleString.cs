using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Prog2370_Final
{
    class SimpleString : DrawableGameComponent
    {
        private SpriteBatch spriteBatch;

        private SpriteFont spriteFont;
        private Vector2 position;
        private String message;
        private Color color;
        public SimpleString(Game game, 
            SpriteBatch spriteBatch,
            SpriteFont spriteFont,
            Vector2 position,
            String message,
            Color color) : base(game)
        {
            this.spriteBatch = spriteBatch;
            this.spriteFont = spriteFont;
            this.position = position;
            this.message = message;
            this.color = color;
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            spriteBatch.DrawString(spriteFont, message, position, color);

            spriteBatch.End();
        }

        public override void Update(GameTime gameTime) { }
    }
}
