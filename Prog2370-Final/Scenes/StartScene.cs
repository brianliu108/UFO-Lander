using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Prog2370_Final.Drawable;

namespace Prog2370_Final.Scenes {
    internal class StartScene : Scene {
        private MenuComponent menu;
        
        private string[] menuItems = {"Play", "Restart", "Help","Credits", "Exit"};
        private Song music;

        public StartScene(Game game,
            SpriteBatch spriteBatch) : base(game) {
            this.spriteBatch = spriteBatch;
            menu = new MenuComponent(game, spriteBatch, resources.RegularFont,
                game.Content.Load<SpriteFont>("Fonts/BoldFont"),
                menuItems);

            Components.Add(menu);

            LoadContent();

            music = resources.menuMusic;

            PlayMusic(true);
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

        public void PlayMusic(bool enable)
        {
            if (enable)
            {
                MediaPlayer.Play(music);
            }
            else
            {
                MediaPlayer.Stop();
            }
        }
    }
}