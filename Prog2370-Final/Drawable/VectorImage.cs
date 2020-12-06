using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Prog2370_Final.Drawable {
    public class VectorImage : DrawableGameComponent, ICollidableComplex {
        private static bool debug = false;

        private static readonly Vector2 originRight = new Vector2(0, .5f);
        private static readonly Vector2 originLeft = new Vector2(1, .5f);
        private static Texture2D whitePixel = null;

        private SpriteBatch spriteBatch;
        private Texture2D whiteCircle;
        private Vector2 circleOffset;

        private readonly Vector2[] vertices;
        private readonly Rectangle boundingBox;
        private Rectangle[] rectangles;
        private float[] rotations;
        private bool[] drawDir;
        private float width;
        private Color color;

        public Vector2[] Vertices => vertices;
        public Vector2 offset = Vector2.Zero;
        public Vector2 scale = Vector2.One;

        public Rectangle BoundingBox {
            get {
                Rectangle r = boundingBox;
                r.X += (int) offset.X;
                r.Y += (int) offset.Y;
                r.Width *= (int) scale.X;
                r.Height *= (int) scale.Y;
                return r;
            }
        }


        /// <summary>
        /// Creates a vector based image by connecting all the vertices given together in a string.
        /// </summary>
        /// <param name="game">A reference to the main game</param>
        /// <param name="spriteBatch">Spritebatch for drawing.</param>
        /// <param name="vertices">The vertices to connect</param>
        /// <param name="width">The width of the line</param>
        /// <param name="color">The color of the line</param>
        /// <exception cref="Exception">If less than 2 vertices were given.</exception>
        public VectorImage(Game game, SpriteBatch spriteBatch, Vector2[] vertices, int width, Color color) :
            base(game) {
            if (vertices.Length < 2) throw new Exception("There must be at least 2 vertices");
            this.spriteBatch = spriteBatch;
            this.vertices = vertices;
            this.width = width;
            this.color = color;
            if (whitePixel == null) whitePixel = ((Game1) game).Resources.WhitePixel;
            rectangles = new Rectangle[vertices.Length - 1];
            rotations = new float[vertices.Length - 1];
            drawDir = new bool[vertices.Length - 1];
            boundingBox = new Rectangle((int) vertices[0].X, (int) vertices[0].Y, 0, 0);
            for (var i = 0; i < vertices.Length - 1; i++) {
                var a = vertices[i];
                var b = vertices[i + 1];
                rectangles[i] = new Rectangle((int) a.X, (int) a.Y,
                    (int) Math.Sqrt(Math.Pow(b.X - a.X, 2) + Math.Pow(b.Y - a.Y, 2)),
                    width);
                rotations[i] = (float) Math.Atan((b.Y - a.Y) / (b.X - a.X));
                drawDir[i] = a.X <= b.X;
                boundingBox.X = (int) Math.Min(boundingBox.X, b.X);
                boundingBox.Y = (int) Math.Min(boundingBox.Y, b.Y);
                boundingBox.Width = (int) Math.Max(boundingBox.Width, b.X);
                boundingBox.Height = (int) Math.Max(boundingBox.Height, b.Y);
            }
            boundingBox.Width -= boundingBox.X;
            boundingBox.Height -= boundingBox.Y;

            var r = width / 2;
            var d2 = width * width * 0.25f;
            whiteCircle = new Texture2D(GraphicsDevice, width, width);
            var circleData = new Color[width * width];
            for (var i = 0; i < width; i++)
            for (var j = 0; j < width; j++)
                circleData[i * width + j] =
                    Math.Pow(i - r, 2) + Math.Pow(j - r, 2) <= d2
                        ? Color.White
                        : Color.Transparent;
            whiteCircle.SetData(circleData);
            circleOffset = new Vector2((float) width / 2, (float) width / 2);
        }


        /// <summary>
        /// Draws the vector image.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime) {
            // Setting up the offset array
            Rectangle[] rAdjusted;
            if (offset == Vector2.Zero && scale == Vector2.Zero) {
                rAdjusted = rectangles;
            } else {
                rAdjusted = new Rectangle[rectangles.Length];
                for (var i = 0; i < rectangles.Length; i++)
                    rAdjusted[i] =
                        new Rectangle(
                            (rectangles[i].Location.ToVector2() * scale + offset).ToPoint(),
                            (rectangles[i].Size.ToVector2() * new Vector2(scale.X, 1)).ToPoint());
            }

            // Doing the actual drawing
            spriteBatch.Begin();
            if (debug) DebugDraw();
            for (var i = 0; i < rectangles.Length; i++) // Draw the lines
                spriteBatch.Draw(
                    whitePixel, rAdjusted[i], null, color, rotations[i],
                    drawDir[i] ? originRight : originLeft, SpriteEffects.None, 0);
            foreach (var r in rAdjusted) // Draw the dots
                spriteBatch.Draw(whiteCircle, r.Location.ToVector2() - circleOffset, color);
            spriteBatch.End();
        }

        private void DebugDraw() {
            Rectangle line, bb = BoundingBox;
            // Y
            line = bb;
            line.Height = 1;
            spriteBatch.Draw(whitePixel, line, Color.Wheat);
            line.Y += bb.Height;
            spriteBatch.Draw(whitePixel, line, Color.Wheat);
            // X
            line = bb;
            line.Width = 1;
            spriteBatch.Draw(whitePixel, line, Color.Wheat);
            line.X += bb.Width;
            spriteBatch.Draw(whitePixel, line, Color.Wheat);
        }

        #region ICollidableComplex Members

        public Rectangle AABB => BoundingBox;
        public CollisionNotificationLevel CollisionNotificationLevel => CollisionNotificationLevel.None;
        public List<CollisionLog> CollisionLogs { get; set; }
        public Vector2[] BoundingVertices {
            get {
                Vector2[] boundingVertices = new Vector2[vertices.Length];
                for (int i = 0; i < vertices.Length; i++)
                    boundingVertices[i] = vertices[i] + offset;
                return boundingVertices;
            }
        }

        public bool BoundingLinesLoop => false;
        public bool BoundingLinesFormConvexPolygon => false;

        #endregion
    }
}