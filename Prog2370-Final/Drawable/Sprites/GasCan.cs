using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace Prog2370_Final.Drawable.Sprites {
    public class GasCan : Sprite , ICollidable, IPerishable
    { //TODO Make this inherit from `Sprite` instead.        
        public Vector2 pos;
        private Point drawSize;
        // private Rectangle position;
        private bool perished;

        public bool CanCollide { get; private set; }
        public Rectangle AABB => new Rectangle(pos.ToPoint(),drawSize);

        public CollisionNotificationLevel CollisionNotificationLevel => CollisionNotificationLevel.Partner;

        public List<CollisionLog> CollisionLogs { get; set; } = new List<CollisionLog>();

        public GasCan(Game game,
            SpriteBatch spriteBatch,
            Texture2D tex,
            Vector2 position) : base(game, spriteBatch, tex, position) {
            this.spriteBatch = spriteBatch;
            this.tex = tex;
            // this.position = new Rectangle((int) position.X, (int) position.Y, tex.Width / 2, tex.Height / 2);
            this.pos = position;
            drawSize = new Point(tex.Width / 2, tex.Height / 2);
            CanCollide = true;
            Perished = false;
        }
        

        // public void Move(Vector2 position) {
        //     // this.position = new Rectangle((int) (position.X), (int) (position.Y), tex.Width / 2, tex.Height / 2);
        //     positionNew = position;
        // }

        // public Rectangle GetBound() {
        //     return new Rectangle((int) (position.X), (int) (position.Y), tex.Width, tex.Height);
        // }

        public override void Draw(GameTime gameTime) {
            spriteBatch.Begin();
            spriteBatch.Draw(tex, new Rectangle(pos.ToPoint(),drawSize), Color.White);
            spriteBatch.End();


            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime) {
            if (CollisionLogs.Count(log => log.collisionPartner is UFO) > 0)
                Perished = true;
        }

        protected override void LoadContent() {
            tex = ((Game1) Game).Resources.GasCan;
        }

        public bool Perished {
            get => perished;
            set {
                perished = value;
                if (value == true) CanCollide = false;
            }
        }
    }
}