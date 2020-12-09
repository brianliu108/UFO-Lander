using System.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Prog2370_Final.Drawable;

namespace Prog2370_Final.Scenes {
    /// <summary>
    /// Scene with menu items. Serves as pause menu as well
    /// </summary>
    public class StartScene : Scene {
        private MenuComponent menu;
        private Terrain terrainPic;

        private string[] menuItems = {"Play", "Restart", "Help", "High Scores", "Credits", "Exit"};
        private Song music;

        /// <summary>
        /// Creates the StartScene
        /// </summary>
        /// <param name="game">Current game reference</param>
        /// <param name="spriteBatch">Spritebatch to draw with</param>
        public StartScene(Game game,
            SpriteBatch spriteBatch) : base(game) {
            this.spriteBatch = spriteBatch;
            menu = new MenuComponent(game, spriteBatch, resources.RegularFont,
                game.Content.Load<SpriteFont>("Fonts/BoldFont"),
                menuItems);

            Components.Add(terrainPic = new Terrain(Game, spriteBatch,
                (float) GraphicsDevice.Viewport.Width,
                (float) GraphicsDevice.Viewport.Height / 6,
                80,
                2,
                0,
                8,
                Resources.darkBrown,
                new Vector2(0, GraphicsDevice.Viewport.Height * 3f / 4f)));

            Components.Add(menu);

            LoadContent();

            music = resources.menuMusic;

            PlayMusic(true);
        }

        /// <summary>
        /// Get and set menu
        /// </summary>
        public MenuComponent Menu {
            get => menu;
            set => menu = value;
        }

        /// <summary>
        /// Load compoenents
        /// </summary>
        protected override void LoadContent() {
            var title = new SimpleString(Game, spriteBatch, ((UfoLander) Game).Resources.TitleFont,
                new Vector2(Shared.stage.X / 2 - 45, 100), "Cool Title", ColourSchemes.boldColour,
                SimpleString.TextAlignH.Middle, SimpleString.TextAlignV.Middle);

            this.Components.Add(title);
        }

        /// <summary>
        /// Start/Stop music
        /// </summary>
        /// <param name="enable">whether to start or stop the music</param>
        public void PlayMusic(bool enable) {
            if (enable) {
                MediaPlayer.Play(music);
            } else {
                MediaPlayer.Stop();
            }
        }
    }
}