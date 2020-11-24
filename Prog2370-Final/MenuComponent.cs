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
    public class MenuComponent : DrawableGameComponent
    {
        SpriteBatch spriteBatch;
        SpriteFont regularFont;
        SpriteFont boldFont;
        private List<String> menuItems;
        private int selectedIndex = 0;

        private Vector2 position;
        private Color regularColour = ColourSchemes.regularColour;
        private Color boldColour = ColourSchemes.boldColour;

        private KeyboardState oldState;

        public MenuComponent(Game game,
            SpriteBatch spriteBatch,
            SpriteFont regularFont,
            SpriteFont boldFont,
            string[] menuItems) : base(game) {

        }

        public int SelectedIndex { get => selectedIndex; set => selectedIndex = value; }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Vector2 temp = position;
            spriteBatch.Begin();

            for (int i = 0; i < menuItems.Count; i++)
            {
                if(selectedIndex == i)
                {
                    spriteBatch.DrawString(boldFont, menuItems[i], temp, boldColour);
                    temp.Y += boldFont.LineSpacing;
                }
                else
                {
                    spriteBatch.DrawString(regularFont, menuItems[i], temp, regularColour);
                    temp.Y += regularFont.LineSpacing;
                }
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }        
    }
}
