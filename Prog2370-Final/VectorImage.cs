using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Prog2370_Final {
    public class VectorImage : DrawableGameComponent {
        private static readonly Vector2 originRight = new Vector2(0, .5f);
        private static readonly Vector2 originLeft = new Vector2(1, .5f);
        private static Texture2D whitePixel = null;

        private SpriteBatch spriteBatch;
        private Texture2D whiteCircle;
        private Vector2 circleOffset;

        private Rectangle[] rectangles;
        private float[] rotations;
        private bool[] drawDir;
        private float width;
        private Color color;
        public Vector2 offset = Vector2.Zero;
        public Vector2 scale = Vector2.One;

        public VectorImage(Game game, SpriteBatch spriteBatch, Vector2[] vertices, int width, Color color) :
            base(game) {
            SetWhitePixel();
            if (vertices.Length < 2) throw new Exception("There must be at least 2 vertices");
            this.spriteBatch = spriteBatch;
            this.width = width;
            this.color = color;

            rectangles = new Rectangle[vertices.Length - 1];
            rotations = new float[vertices.Length - 1];
            drawDir = new bool[vertices.Length - 1];
            for (int i = 0; i < vertices.Length - 1; i++) {
                Vector2 a = vertices[i];
                Vector2 b = vertices[i + 1];
                rectangles[i] = new Rectangle((int) a.X, (int) a.Y,
                    (int) Math.Sqrt(Math.Pow(b.X - a.X, 2) + Math.Pow(b.Y - a.Y, 2)),
                    width);
                rotations[i] = (float) Math.Atan((b.Y - a.Y) / (b.X - a.X));
                drawDir[i] = a.X <= b.X;
            }

            int r = width / 2;
            float d2 = width * width * 0.25f;
            whiteCircle = new Texture2D(GraphicsDevice, width, width);
            Color[] circleData = new Color[width * width];
            for (int i = 0; i < width; i++)
            for (int j = 0; j < width; j++)
                circleData[i * width + j] =
                    Math.Pow(i - r, 2) + Math.Pow(j - r, 2) <= d2
                        ? Color.White
                        : Color.Transparent;
            whiteCircle.SetData(circleData);
            circleOffset = new Vector2((float) width / 2, (float) width / 2);
        }

        private void SetWhitePixel() {
            if (whitePixel != null) return;
            whitePixel = new Texture2D(GraphicsDevice, 1, 1);
            whitePixel.SetData(new[] {new Color(255, 255, 255)});
        }

        public override void Draw(GameTime gameTime) {
            // Setting up the offset array
            Rectangle[] rAdjusted;
            if (offset == Vector2.Zero && scale == Vector2.Zero) rAdjusted = rectangles;
            else {
                rAdjusted = new Rectangle[rectangles.Length];
                for (int i = 0; i < rectangles.Length; i++)
                    rAdjusted[i] =
                        new Rectangle(
                            (rectangles[i].Location.ToVector2() * scale + offset).ToPoint(),
                            (rectangles[i].Size.ToVector2() * new Vector2(scale.X, 1)).ToPoint());
            }

            // Doing the actual drawing
            spriteBatch.Begin();
            for (var i = 0; i < rectangles.Length; i++) // Draw the lines
                spriteBatch.Draw(
                    whitePixel, rAdjusted[i], null, color, rotations[i],
                    drawDir[i] ? originRight : originLeft, SpriteEffects.None, 0);
            foreach (var r in rAdjusted) // Draw the dots
                spriteBatch.Draw(whiteCircle, r.Location.ToVector2() - circleOffset, color);
            spriteBatch.End();
        }
    }
}