using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

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

        public readonly SoundEffect softExplosion,
            hugeExplosion, thrust, land,
            deathSound, menuSound, enterSound;

        public Song menuMusic;

        public readonly SpriteFont
            RegularFont,
            BoldFont,
            TitleFont,
            DeathFont;

        public readonly Texture2D
            WhitePixel,
            UFO, UFO_thrust,
            GasCan, Explosion,
            UFOSprite;
        

        public Resources(Game game) {
            WhitePixel = new Texture2D(game.GraphicsDevice, 1, 1);
            WhitePixel.SetData(new[] {new Color(255, 255, 255)});
            
            // Load Fonts
            RegularFont = game.Content.Load<SpriteFont>("Fonts/RegularFont");
            BoldFont = game.Content.Load<SpriteFont>("Fonts/BoldFont");
            TitleFont = game.Content.Load<SpriteFont>("Fonts/TitleFont");
            DeathFont = game.Content.Load<SpriteFont>("Fonts/DeathFont");

            // Load Textures
            UFO = game.Content.Load<Texture2D>("Images/UFO");
            UFO_thrust = game.Content.Load<Texture2D>("Images/UFOThrust");
            GasCan = game.Content.Load<Texture2D>("Images/gascan");
            Explosion = game.Content.Load<Texture2D>("Images/explosion");
            UFOSprite = game.Content.Load<Texture2D>("Images/ufoSprite");


            // Load Sounds
            softExplosion = game.Content.Load<SoundEffect>("Sounds/softExplosion");
            hugeExplosion = game.Content.Load<SoundEffect>("Sounds/hugeExplosion");
            thrust = game.Content.Load<SoundEffect>("Sounds/thrust");
            land = game.Content.Load<SoundEffect>("Sounds/land");
            deathSound = game.Content.Load<SoundEffect>("Sounds/deathSound");
            menuSound = game.Content.Load<SoundEffect>("Sounds/menuSound");
            enterSound = game.Content.Load<SoundEffect>("Sounds/enterSound");
            menuMusic = game.Content.Load<Song>("Sounds/pauseMenu");
        }

    }
}