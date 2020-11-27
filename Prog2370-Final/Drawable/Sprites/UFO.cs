using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Prog2370_Final.Drawable.Sprites { //TODO Make this inherit from `Sprite` instead.
    public class UFO : Sprite {        
        private Texture2D thrustTex;
        private Rectangle position;
        private Vector2 speed = Vector2.Zero;        
        bool thrusting = false;

        public UFO(Game game,
            SpriteBatch spriteBatch,
            Texture2D tex,
            Vector2 position) : base(game, spriteBatch, tex, position) {
            this.spriteBatch = spriteBatch;
            this.tex = tex;
            this.position = new Rectangle((int)(position.X),(int)(position.Y),tex.Width /2, tex.Height/2);
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