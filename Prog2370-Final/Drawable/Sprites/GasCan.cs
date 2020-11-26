using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Prog2370_Final.Drawable.Sprites {
    public class GasCan : DrawableGameComponent {
        private SpriteBatch spriteBatch;
        private Texture2D tex;
        private Rectangle position;


        public GasCan(Game game,
            SpriteBatch spriteBatch,
            Texture2D tex,
            Vector2 position) : base(game) {
            this.spriteBatch = spriteBatch;
            this.tex = tex;
            this.position = new Rectangle((int) position.X, (int) position.Y, tex.Width / 2, tex.Height / 2);
        }

        public void Show(bool enable) {
            Enabled = enable;
            Visible = enable;
        }

        //public void Move(Vector2 position)
        //{
        //    this.position = position;
        //}
        //public Rectangle GetBound()
        //{
        //    return new Rectangle((int)(position.X), (int)(position.Y), tex.Width, tex.Height);
        //}

        public override void Draw(GameTime gameTime) {
            spriteBatch.Begin();
            spriteBatch.Draw(tex, position, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
        }

        protected override void LoadContent()
        {
            var gasCanTex = Game.Content.Load<Texture2D>("Images/gascan");
        }
    }
}