using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace Prog2370_Final {
    public class Resources {
        public static readonly Color regularColour = new Color(130, 52, 65);
        public static readonly Color boldColour = new Color(158, 70, 76);
        public static readonly Color darkBlue = new Color(46, 49, 55);
        public static readonly Color normBlue = new Color(30, 84, 97);
        public static readonly Color darkRed = new Color(60, 44, 49);
        public static readonly Color normRed = new Color(130, 52, 65);
        public static readonly Color darkBrown = new Color(69, 49, 51);
        public static readonly Color brown = new Color(102, 59, 58);
        public static readonly Color pink = new Color(129, 34, 85);
        public static readonly SoundEffect thrust;


        public readonly SpriteFont
            RegularFont,
            BoldFont,
            TitleFont;

        public readonly Texture2D
            WhitePixel,
            UFO, UFO_thrust,
            GasCan;
        

        public Resources(Game game) {
            WhitePixel = new Texture2D(game.GraphicsDevice, 1, 1);
            WhitePixel.SetData(new[] {new Color(255, 255, 255)});
            
            RegularFont = game.Content.Load<SpriteFont>("Fonts/RegularFont");
            BoldFont = game.Content.Load<SpriteFont>("Fonts/BoldFont");
            TitleFont = game.Content.Load<SpriteFont>("Fonts/TitleFont");
            UFO = game.Content.Load<Texture2D>("Images/UFO");
            UFO_thrust = game.Content.Load<Texture2D>("Images/UFOThrust");
            GasCan = game.Content.Load<Texture2D>("Images/gascan");

            thrust = Content.Load<SoundEffect>("Sounds/");
        }

    }
}