using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Prog2370_Final.Scenes {
    internal class StartScene : Scene {
        private MenuComponent menu;
        private SpriteBatch spriteBatch;
        private string[] menuItems = {"Start", "Credits", "Exit"};

        public StartScene(Game game,
            SpriteBatch spriteBatch) : base(game) {
            this.spriteBatch = spriteBatch;
            menu = new MenuComponent(game, spriteBatch, game.Content.Load<SpriteFont>("Fonts/RegularFont"),
                game.Content.Load<SpriteFont>("Fonts/BoldFont"),
                menuItems);

            Components.Add(menu);
        }

        public MenuComponent Menu {
            get => menu;
            set => menu = value;
        }

        protected override void LoadContent()
        {
            var titleFont = Game.Content.Load<SpriteFont>("Fonts/TitleFont");

            var title = new SimpleString(Game, spriteBatch, titleFont,
                new Vector2(100, 100), "Cool Title", ColourSchemes.boldColour);
            this.Components.Add(title);


        }
    }
}