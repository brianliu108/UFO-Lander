using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Prog2370_Final.Scenes {
    public class CreditsScene : Scene {
        private SpriteBatch spriteBatch;
        private SimpleString sS;


        public CreditsScene(Game game, SpriteBatch spriteBatch) : base(game) {
            this.spriteBatch = spriteBatch;
        }

        protected override void LoadContent()
        {
            var boldFont = Game.Content.Load<SpriteFont>("Fonts/BoldFont");

            SimpleString creditsString = new SimpleString(Game, spriteBatch, boldFont, new Vector2(220, 220),
                "Made By:\nTim Skibik\nBrian Liu", ColourSchemes.boldColour);
             this.Components.Add(creditsString);
        }
    }
}