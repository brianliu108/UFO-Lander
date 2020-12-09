using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Prog2370_Final {
    /// <summary>
    /// Object to commonly reference game content
    /// </summary>
    public class Resources {
        public const string SaveFileLocation = @".\highscores.txt";
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
            hugeExplosion,
            thrust,
            land,
            deathSound,
            menuSound,
            enterSound;

        public Song menuMusic;

        public readonly SpriteFont
            RegularFont,
            BoldFont,
            TitleFont,
            DeathFont,
            MonoFont;

        public readonly Texture2D
            WhitePixel,
            UFO,
            UFO_thrust,
            GasCan,
            Explosion,
            UFOSprite;

        /// <summary>
        /// Creates the Resource object
        /// </summary>
        /// <param name="game">Current game reference</param>
        public Resources(Game game) {
            WhitePixel = new Texture2D(game.GraphicsDevice, 1, 1);
            WhitePixel.SetData(new[] {new Color(255, 255, 255)});

            // Load Fonts
            RegularFont = game.Content.Load<SpriteFont>("Fonts/RegularFont");
            BoldFont = game.Content.Load<SpriteFont>("Fonts/BoldFont");
            TitleFont = game.Content.Load<SpriteFont>("Fonts/TitleFont");
            DeathFont = game.Content.Load<SpriteFont>("Fonts/DeathFont");
            MonoFont = game.Content.Load<SpriteFont>("Fonts/Monospaced");

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

        public const int maxRecordsInScore = 10;
        /// <summary>
        /// Creates list of high scores
        /// </summary>
        /// <returns>a list of highscores. Tupled with a name and a score</returns>
        public static List<Tuple<string, int>> ParseHighScores() {
            List<Tuple<string, int>> highScores = new List<Tuple<string, int>>();
            using (StreamReader reader = new StreamReader(Resources.SaveFileLocation)) {
                string line;
                while ((line = reader.ReadLine()) != null && line.Length > 1) {
                    string[] brokenLine = line.Split(' ');
                    highScores.Add(new Tuple<string, int>(brokenLine[0], int.Parse(brokenLine[1])));
                }
            }
            return highScores;
        }

        /// <summary>
        /// Creates the formatted high scores string
        /// </summary>
        /// <returns>formatted high scores</returns>
        public static string FormattedHighScores() {
            List<Tuple<string, int>> records = ParseHighScores();
            if (records.Count == 0) return "";
            int maxNameLength = records.Max(tuple => tuple.Item1.Length);
            int maxScoreLength = records.Max(tuple => tuple.Item2.ToString().Length);
            records.Sort((l, r) => r.Item2 - l.Item2);
            StringBuilder strb = new StringBuilder("High scores:\n");
            int num = 1;
            foreach (Tuple<string, int> record in records)
                strb.Append($"{num++,2}")
                    .Append(": ")
                    .Append(string.Format("{0," + -(maxNameLength + 1) + "}", record.Item1))
                    .Append(" - ")
                    .Append(string.Format("{0," + (maxScoreLength + 1) + "}", record.Item2))
                    .Append("\n");
            return strb.ToString();
        }

        /// <summary>
        /// Adds a new high score
        /// </summary>
        /// <param name="name">name of palyer</param>
        /// <param name="score">player's score</param>
        public static void AddToHighScoreFile(string name, int score) {
            List<Tuple<string, int>> records = ParseHighScores();
            records.Add(new Tuple<string, int>(name,score));
            if (records.Count > maxRecordsInScore) {
                int minScore = records.Min(tuple => tuple.Item2);
                records.RemoveAll(tuple => tuple.Item2 == minScore);
            }
            records.Sort((l, r) => r.Item2 - l.Item2);
            using (StreamWriter writer = new StreamWriter(Resources.SaveFileLocation))
                foreach (var record in records)
                    writer.WriteLine(record.Item1 + " " + record.Item2);
        }
    }
}