using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Prog2370_Final {
    public class Resources {
        public readonly SpriteFont
            RegularFont,
            BoldFont,
            TitleFont;

        public readonly Texture2D
            UFO, UFO_thrust,
            GasCan;

        public Resources(Game game) {
            RegularFont = game.Content.Load<SpriteFont>("Fonts/RegularFont");
            BoldFont = game.Content.Load<SpriteFont>("Fonts/BoldFont");
            TitleFont = game.Content.Load<SpriteFont>("Fonts/TitleFont");
            UFO = game.Content.Load<Texture2D>("Images/UFO");
            UFO_thrust = game.Content.Load<Texture2D>("Images/UFOThrust");
            GasCan = game.Content.Load<Texture2D>("Images/gascan");
        }

    }
}