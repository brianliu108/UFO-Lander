using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Prog2370_Final.Drawable;

namespace Prog2370_Final.Scenes {
    /// <summary>
    /// Scene to show our names
    /// </summary>
    public class CreditsScene : Scene {
        private int starCount = 4;
        private VectorImage[] stars;

        /// <summary>
        /// Creates the credits scene
        /// </summary>
        /// <param name="game">reference to current game</param>
        /// <param name="spriteBatch">spriteBatch to draw with</param>
        public CreditsScene(Game game, SpriteBatch spriteBatch) : base(game) {
            this.spriteBatch = spriteBatch;
            LoadContent();
        }

        /// <summary>
        /// Loading the content for the scene
        /// </summary>
        protected override void LoadContent() {
            // Add Credit words
            string header = "Made by:";
            string credit = "Brian Liu\nTim Skibik";
            SpriteFont fontHeader = resources.RegularFont;
            SpriteFont fontCredit = resources.BoldFont;
            Vector2 center = new Vector2(
                GraphicsDevice.Viewport.Bounds.Width / 2f - fontCredit.MeasureString(credit).X / 2,
                GraphicsDevice.Viewport.Bounds.Height / 2f);
            Vector2 posHeader = center + new Vector2(0, -fontHeader.LineSpacing - fontCredit.LineSpacing / 2f);
            Vector2 posCredit = center + new Vector2(0, -fontCredit.LineSpacing / 2f);

            Components.Add(new SimpleString(Game, spriteBatch,
                fontHeader, posHeader, header, ColourSchemes.normRed));

            Components.Add(new SimpleString(Game, spriteBatch,
                fontCredit, posCredit, credit, ColourSchemes.boldColour));


            // Credit stars
            Vector2[] starVertices = new[] {
                new Vector2(0, -100),
                new Vector2(58.7785f, 80.9017f),
                new Vector2(-95.1057f, -30.9017f),
                new Vector2(95.1057f, -30.9017f),
                new Vector2(-58.7785f, 80.9017f),
                new Vector2(0, -100)
            };
            stars = new VectorImage[starCount];
            for (int i = 0; i < starCount; i++) {
                var star = new VectorImage(
                    Game, spriteBatch,
                    starVertices, 2, ColourSchemes.pink) {
                    scale = new Vector2(0.15f, 0.15f)
                };
                stars[i] = star;
                Components.Add(star);
            }
        }

        /// <summary>
        /// Whether to show the scene or not. Hides the stars as well
        /// </summary>
        /// <param name="enable"></param>
        public override void Show(bool enable) {
            base.Show(enable);
            if (enable == true) {
                Random r = new Random();
                Vector2 center = new Vector2(
                    GraphicsDevice.Viewport.Bounds.Width / 2f,
                    GraphicsDevice.Viewport.Bounds.Height / 2f);
                for (int i = 0; i < starCount; i++) {
                    float x = (float) ((i + r.NextDouble() * 0.75) / (starCount - 1f) * 2f * Math.PI);
                    stars[(int) i].Offset = center + new Vector2(
                        (float) (200 * Math.Cos(x)),
                        (float) (100 * Math.Sin(x)));
                }
            }
        }
    }
}