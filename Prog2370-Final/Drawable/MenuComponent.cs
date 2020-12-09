using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace Prog2370_Final.Drawable {
    /// <summary>
    /// Drawable menu system
    /// </summary>
    public class MenuComponent : DrawableGameComponent {
        private SpriteBatch spriteBatch;
        private SpriteFont regularFont;
        private SpriteFont boldFont;
        private List<string> menuItems;
        private int selectedIndex = 0; // Current selected item
        private SoundEffectInstance menuSoundIns, menuSoundIns2;
        private Resources resources;

        private Vector2 position;
        private Color regularColour = ColourSchemes.regularColour;
        private Color boldColour = ColourSchemes.boldColour;

        private KeyboardState oldState;

        /// <summary>
        /// Creates the menu component
        /// </summary>
        /// <param name="game">Current game reference</param>
        /// <param name="spriteBatch">spriteBatch to draw with</param>
        /// <param name="regularFont">font for non-selected menu items</param>
        /// <param name="boldFont">font for selected menu item</param>
        /// <param name="menus">string array of menu items</param>
        public MenuComponent(Game game,
            SpriteBatch spriteBatch,
            SpriteFont regularFont,
            SpriteFont boldFont,
            string[] menus) : base(game) {
            this.spriteBatch = spriteBatch;
            this.regularFont = regularFont;
            this.boldFont = boldFont;
            menuItems = new List<string>();

            resources = ((Game1) Game).Resources;

            menuItems = menus.ToList();
            float maxWidth = 0;
            foreach (string s in menus) maxWidth = Math.Max(maxWidth, boldFont.MeasureString(s).X);
            float maxHeight = boldFont.MeasureString("A").Y +
                              regularFont.MeasureString("A").Y * (menus.Length - 1);
            position = new Vector2((Shared.stage.X - maxWidth) / 2, (Shared.stage.Y - maxHeight) / 2);
            menuSoundIns = resources.menuSound.CreateInstance();
            menuSoundIns2 = resources.menuSound.CreateInstance();

            menuSoundIns.Volume = .4f;
            menuSoundIns2.Volume = .4f;
        }

        /// <summary>
        /// Get and Set selectedIndex
        /// </summary>
        public int SelectedIndex {
            get => selectedIndex;
            set => selectedIndex = value;
        }

        /// <summary>
        /// Changes the selected item based on keyboard input
        /// </summary>
        /// <param name="gameTime">snapshot of game timing</param>
        public override void Update(GameTime gameTime) {
            var ks = Keyboard.GetState();

            if (ks.IsKeyDown(Keys.Down) && oldState.IsKeyUp(Keys.Down)) {
                selectedIndex++;

                if (selectedIndex == menuItems.Count) selectedIndex = 0;

                menuSoundIns.Play();
            }
            if (ks.IsKeyDown(Keys.Up) && oldState.IsKeyUp(Keys.Up)) {
                //menuSoundIns.Stop();
                selectedIndex--;

                if (selectedIndex == -1) selectedIndex = menuItems.Count - 1;

                menuSoundIns2.Play();
            }
            oldState = ks;

            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the SpriteBatch to current selected index
        /// </summary>
        /// <param name="gameTime">Snapshot of game timing</param>
        public override void Draw(GameTime gameTime) {
            var temp = position;
            spriteBatch.Begin();

            for (var i = 0; i < menuItems.Count; i++)
                if (selectedIndex == i) {
                    spriteBatch.DrawString(boldFont, menuItems[i], temp, boldColour);
                    temp.Y += boldFont.LineSpacing;
                } else {
                    spriteBatch.DrawString(regularFont, menuItems[i], temp, regularColour);
                    temp.Y += regularFont.LineSpacing;
                }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}