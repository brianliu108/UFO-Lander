using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Prog2370_Final.Drawable.Sprites {
    public class UFO : DrawableGameComponent {
        private SpriteBatch spriteBatch;
        private Texture2D tex;
        private Texture2D thrustTex;
        private Vector2 position;
        private Vector2 speed;
        bool thrusting = false;

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

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            if (!thrusting)
            {
                spriteBatch.Draw(tex, position, Color.White);
            }
            spriteBatch.End();
        }

        protected override void LoadContent() {
            tex = ((Game1)Game).Resources.UFO;
            thrustTex = ((Game1) Game).Resources.UFO_thrust;

        }
    }
}