using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Prog2370_Final {
    public class MenuComponent : DrawableGameComponent {
        private SpriteBatch spriteBatch;
        private SpriteFont regularFont;
        private SpriteFont boldFont;
        private List<string> menuItems;
        private int selectedIndex = 0;

        private Vector2 position;
        private Color regularColour = ColourSchemes.regularColour;
        private Color boldColour = ColourSchemes.boldColour;

        private KeyboardState oldState;

        public MenuComponent(Game game,
            SpriteBatch spriteBatch,
            SpriteFont regularFont,
            SpriteFont boldFont,
            string[] menus) : base(game) {
            this.spriteBatch = spriteBatch;
            this.regularFont = regularFont;
            this.boldFont = boldFont;
            menuItems = new List<string>();

            menuItems = menus.ToList();

            position = new Vector2(Shared.stage.X / 2, Shared.stage.Y / 2);
        }

        public int SelectedIndex {
            get => selectedIndex;
            set => selectedIndex = value;
        }


        public override void Update(GameTime gameTime) {
            var ks = Keyboard.GetState();

            if (ks.IsKeyDown(Keys.Down) && oldState.IsKeyUp(Keys.Down)) {
                selectedIndex++;

                if (selectedIndex == menuItems.Count) selectedIndex = 0;
            }
            if (ks.IsKeyDown(Keys.Up) && oldState.IsKeyUp(Keys.Up)) {
                selectedIndex--;

                if (selectedIndex == -1) selectedIndex = menuItems.Count - 1;
            }
            oldState = ks;

            base.Update(gameTime);
        }

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