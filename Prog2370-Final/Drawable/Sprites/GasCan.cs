using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Prog2370_Final.Drawable.Sprites {
    public class GasCan : Sprite , ICollidable
    { //TODO Make this inherit from `Sprite` instead.        
        private Rectangle position;
        List<CollisionLog> collisionList;

        public Rectangle AABB => position;

        public CollisionNotificationLevel CollisionNotificationLevel => CollisionNotificationLevel.Partner;

        public List<CollisionLog> CollisionLogs { set => collisionList = value; }

        public GasCan(Game game,
            SpriteBatch spriteBatch,
            Texture2D tex,
            Vector2 position) : base(game, spriteBatch, tex, position)
        {
            this.spriteBatch = spriteBatch;
            this.tex = tex;
            this.position = new Rectangle((int)position.X, (int)position.Y, tex.Width / 2, tex.Height / 2);
        }

        public void Show(bool enable)
        {
            Enabled = enable;
            Visible = enable;
        }

        public void Move(Vector2 position)
        {
            this.position = new Rectangle((int)(position.X), (int)(position.Y), tex.Width/2, tex.Height/2);
        }
        public Rectangle GetBound()
        {
            return new Rectangle((int)(position.X), (int)(position.Y), tex.Width, tex.Height);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(tex, position, Color.White);
            spriteBatch.End();
            

            base.Draw(gameTime);
            if (collisionList != null)
                DrawBoundingBox(AABB, (Game1)Game, collisionList.Count == 0 ? ColourSchemes.normRed : Color.Wheat);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void LoadContent()
        {
            tex = ((Game1)Game).Resources.GasCan;
        }

    }
}