using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Prog2370_Final.Drawable;

namespace Prog2370_Final.Scenes {
    public class CreditsScene : Scene {
        private SpriteBatch spriteBatch;
        private SimpleString sS;


        public CreditsScene(Game game, SpriteBatch spriteBatch) : base(game) {
            this.spriteBatch = spriteBatch;
            LoadContent();
        }

        protected override void LoadContent() {
            SimpleString creditsString = new SimpleString(Game, spriteBatch, ((Game1) Game).Resources.BoldFont,
                new Vector2(220, 220),
                "Made By:\nTim Skibik\nBrian Liu", ColourSchemes.boldColour);
            this.Components.Add(creditsString);
        }
    }
}