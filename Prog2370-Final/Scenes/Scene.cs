using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Prog2370_Final.Scenes {
    public abstract class Scene : DrawableGameComponent {
        private List<GameComponent> components;
        protected readonly Resources resources;

        public Scene(Game game) : base(game) {
            components = new List<GameComponent>();
            this.resources = ((Game1) game).Resources;
            Show(false);
        }

        public List<GameComponent> Components {
            get => components;
            set => components = value;
        }

        public virtual void Show(bool enable) {
            Enabled = enable;
            Visible = enable;
        }

        public override void Update(GameTime gameTime) {
            foreach (var item in components)
                if (item.Enabled)
                    item.Update(gameTime);

            base.Update(gameTime);
        }

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