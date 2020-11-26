using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Prog2370_Final.Scenes {
    public class CreditsScene : GameScene {
        private SpriteBatch spriteBatch;
        private SimpleString sS;


        public CreditsScene(Game game, SpriteBatch spriteBatch) : base(game) {
            this.spriteBatch = spriteBatch;
        }
    }
}