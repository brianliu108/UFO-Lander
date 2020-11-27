using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Prog2370_Final.Drawable;

namespace Prog2370_Final.Scenes {
    public class CreditsScene : Scene {
        private SpriteBatch spriteBatch;
        private SimpleString sS;

        private int starCount = 3;
        private VectorImage[] stars;

        public CreditsScene(Game game, SpriteBatch spriteBatch) : base(game) {
            this.spriteBatch = spriteBatch;
            LoadContent();
        }

        protected override void LoadContent() {
            // Credit words
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

        public override void Show(bool enable) {
            base.Show(enable);
            if (enable == true) {
                Random r = new Random();
                Vector2 center = new Vector2(
                    GraphicsDevice.Viewport.Bounds.Width / 2f,
                    GraphicsDevice.Viewport.Bounds.Height / 2f);
                for (int i = 0; i < starCount; i++) {
                    float x = (float) ((i + r.NextDouble() * 0.75) / (starCount - 1f) * 2f * Math.PI);
                    stars[(int) i].offset = center + new Vector2(
                        (float) (200 * Math.Cos(x)),
                        (float) (100 * Math.Sin(x)));
                }
            }
        }
    }
}