using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Prog2370_Final.Scenes {
    /// <summary>
    /// Generic Scene parent class object
    /// </summary>
    public abstract class Scene : DrawableGameComponent {
        private List<GameComponent> components;
        protected readonly Resources resources;
        protected SpriteBatch spriteBatch;

        /// <summary>
        /// Creates the scene
        /// </summary>
        /// <param name="game">Reference to current game</param>
        public Scene(Game game) : base(game) {
            components = new List<GameComponent>();
            this.resources = ((Game1) game).Resources;
            Show(false);
        }

        /// <summary>
        /// get and set components
        /// </summary>
        public List<GameComponent> Components {
            get => components;
            set => components = value;
        }

        /// <summary>
        /// Show/disable the scene
        /// </summary>
        /// <param name="enable">whether to show/disable or not</param>
        public virtual void Show(bool enable) {
            Enabled = enable;
            Visible = enable;
        }

        /// <summary>
        /// Updates the scene if enabled
        /// </summary>
        /// <param name="gameTime">snapshot of timing</param>
        public override void Update(GameTime gameTime) {
            foreach (var item in components)
                if (item.Enabled)
                    item.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// Drawing the Scene
        /// </summary>
        /// <param name="gameTime">Snapshot of timing</param>
        public override void Draw(GameTime gameTime) {
            DrawableGameComponent component = null;

            foreach (var item in components)
                if (item is DrawableGameComponent) {
                    component = (DrawableGameComponent) item;
                    if (component.Visible) component.Draw(gameTime);
                }

            base.Draw(gameTime);
        }
    }
}