using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Prog2370_Final
{
    public abstract class GameScene : DrawableGameComponent
    {
        private List<GameComponent> components;
        public GameScene(Game game) : base(game)
        {
        }

        public List<GameComponent> Components { get => components; set => components = value; }        

        public virtual void Show(bool enable)
        {
            this.Enabled = enable;
            this.Visible = enable;
        }

        public override void Update(GameTime gameTime)
        {
            foreach(GameComponent item in components)
            {
                if (item.Enabled)
                {
                    item.Update(gameTime);
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            DrawableGameComponent component = null;

            foreach(GameComponent item in components)
            {
                if(item is DrawableGameComponent)
                {
                    component = (DrawableGameComponent)item;
                    if (component.Visible)
                    {
                        component.Draw(gameTime);
                    }
                }
            }

            base.Draw(gameTime);
        }
    }


}
