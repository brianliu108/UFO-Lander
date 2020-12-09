using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace Prog2370_Final.Drawable.Sprites {
    /// <summary>
    /// GasCan collectable object. Collision with UFO fills up gas
    /// </summary>
    public class GasCan : Sprite, ICollidable, IPerishable { //TODO Make this inherit from `Sprite` instead.        
        public Vector2 pos;

        private Point drawSize;

        // private Rectangle position;
        private bool perished;

        /// <summary>
        /// <inheritdoc cref="ICollidable.CanCollide"/>
        /// </summary>
        public bool CanCollide { get; private set; }

        /// <summary>
        /// <inheritdoc cref="ICollidable.AABB"/>
        /// </summary>
        public Rectangle AABB => new Rectangle(pos.ToPoint(), drawSize);

        /// <summary>
        /// <inheritdoc cref="ICollidable.CollisionNotificationLevel"/>
        /// </summary>
        public CollisionNotificationLevel CollisionNotificationLevel => CollisionNotificationLevel.Partner;

        /// <summary>
        /// <inheritdoc cref="ICollidable.CollisionLogs"/>
        /// </summary>
        public List<CollisionLog> CollisionLogs { get; set; } = new List<CollisionLog>();

        /// <summary>
        /// Creates a new GasCan at the specified position.
        /// </summary>
        /// <param name="game">The game creating the sprite</param>
        /// <param name="spriteBatch">The spritebatch used to draw the sprite</param>
        /// <param name="position">The position to draw the sprite</param>
        public GasCan(Game game,
            SpriteBatch spriteBatch,
            Vector2 position)
            : base(game, spriteBatch, ((Game1) game).Resources.GasCan, position) {
            this.spriteBatch = spriteBatch;
            this.pos = position;
            drawSize = new Point(tex.Width / 2, tex.Height / 2);
            CanCollide = true;
            Perished = false;
        }

        /// <summary>
        /// Draws the gas can
        /// </summary>
        /// <param name="gameTime">Unused</param>
        public override void Draw(GameTime gameTime) {
            spriteBatch.Begin();
            spriteBatch.Draw(tex, new Rectangle(pos.ToPoint(), drawSize), Color.White);
            spriteBatch.End();
        }

        /// <summary>
        /// Loads the texture
        /// </summary>
        protected override void LoadContent() {
            tex = ((Game1) Game).Resources.GasCan;
        }

        /// <summary>
        /// <inheritdoc cref="IPerishable.Perished"/>
        /// </summary>
        public bool Perished {
            get => perished;
            set {
                perished = value;
                if (value == true) CanCollide = false;
            }
        }
    }
}