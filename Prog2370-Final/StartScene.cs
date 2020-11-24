using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Prog2370_Final
{
    class StartScene : GameScene
    {
        private MenuComponent menu;
        private SpriteBatch spriteBatch;
        private string[] menuItems = { "Start", "Credits", "Exit" };

        public StartScene(Game game,
            SpriteBatch spriteBatch) : base(game)
        {
            this.spriteBatch = spriteBatch;
            menu = new MenuComponent(game, spriteBatch, game.Content.Load<SpriteFont>("Fonts/RegularFont"),
                game.Content.Load<SpriteFont>("Fonts/BoldFont"),
                menuItems);

           this.Components.Add(menu);
        }

        public MenuComponent Menu { get => menu; set => menu = value; }
    }
}
