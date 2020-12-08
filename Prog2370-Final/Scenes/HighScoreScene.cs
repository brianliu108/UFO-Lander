using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Xml.Schema;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Prog2370_Final.Drawable;
using Prog2370_Final.Drawable.Sprites;
using System.IO;
using System.Text;

namespace Prog2370_Final.Scenes
{
    /// <summary>
    /// The scene to show top 5 high scores
    /// </summary>
    public class HighScoreScene : Scene
    {
        private SimpleString highScores;
        private string highScoreMessage;
        public HighScoreScene(Game game, SpriteBatch spriteBatch) : base(game)
        {
            this.spriteBatch = spriteBatch;

            ReadFromFile();

        }

        public override void Draw(GameTime gameTime)
        {
            highScores.Draw(gameTime);
        }

        public void ReadFromFile()
        {
            StringBuilder sb = new StringBuilder();
            using (StreamReader reader = new StreamReader(@"C:\Users\Brian\Desktop\Prog2370-Final\Prog2370-Final\bin\Windows\x86\Debug\highscores.txt"))
            {
                sb.Append(reader.ReadLine());
                
            }
            highScoreMessage = sb.ToString();

            highScores = new SimpleString(Game, spriteBatch, resources.BoldFont, new Vector2(400, 200), highScoreMessage, ColourSchemes.boldColour);
            this.Components.Add(highScores);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
