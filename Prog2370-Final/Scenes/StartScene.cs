using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Prog2370_Final.Drawable;

namespace Prog2370_Final.Scenes {
    internal class StartScene : Scene {
        private MenuComponent menu;
        
        private string[] menuItems = {"Start", "Help","Credits", "Exit"};

        public StartScene(Game game,
            SpriteBatch spriteBatch) : base(game) {
            this.spriteBatch = spriteBatch;
            menu = new MenuComponent(game, spriteBatch, resources.RegularFont,
                game.Content.Load<SpriteFont>("Fonts/BoldFont"),
                menuItems);

            Components.Add(menu);

            LoadContent();
        }

        public MenuComponent Menu {
            get => menu;
            set => menu = value;
        }

        protected override void LoadContent()
        {
            var title = new SimpleString(Game, spriteBatch, ((Game1)Game).Resources.TitleFont,
                new Vector2(100, 100), "Cool Title", ColourSchemes.boldColour);
            this.Components.Add(title);


        }
    }
}