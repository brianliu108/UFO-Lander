using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Prog2370_Final.Drawable.Sprites { //TODO Make this inherit from `Sprite` instead.
    public class UFO : DrawableGameComponent {
        private SpriteBatch spriteBatch;
        private Texture2D tex;
        private Texture2D thrustTex;
        private Vector2 position;
        private Vector2 speed;
        protected readonly Resources resources;
        bool thrusting = false;

        public UFO(Game game,
            SpriteBatch spriteBatch,
            Texture2D tex,
            Vector2 position) : base(game) {
            this.spriteBatch = spriteBatch;
            this.tex = tex;
            this.position = position;
            resources = ((Game1) game).Resources;
        }

        public Texture2D Tex {
            get => tex;
            set => tex = value;
        }

        public override void Draw(GameTime gameTime) {
            spriteBatch.Begin();
            if (!thrusting) {
                spriteBatch.Draw(tex, position, Color.White);
            }
            spriteBatch.End();
        }

        protected override void LoadContent() {
            tex = resources.UFO;
            thrustTex = resources.UFO_thrust;
        }
    }
}