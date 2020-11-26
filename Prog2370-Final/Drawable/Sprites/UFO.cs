using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Prog2370_Final.Drawable.Sprites {
    public class UFO : DrawableGameComponent {
        private SpriteBatch spriteBatch;
        private Texture2D tex;
        private Vector2 position;
        private Vector2 speed;

        public UFO(Game game,
            SpriteBatch spriteBatch,
            Texture2D tex,
            Vector2 position) : base(game) {
            this.spriteBatch = spriteBatch;
            this.tex = tex;
            this.position = position;
        }

        public Texture2D Tex {
            get => tex;
            set => tex = value;
        }

        protected override void LoadContent() {
            tex = ((Game1) Game).Resources.UFO_thrust;
        }
    }
}