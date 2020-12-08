using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Prog2370_Final.Drawable.Sprites;

namespace Prog2370_Final.Drawable {
    public class InfiniteTerrain : DrawableGameComponent {
        private SpriteBatch spriteBatch;
        private readonly Random r = new Random();
        private readonly ShortTerrainDeQueue data; // Main terrain data
        private readonly List<WeakReference<GasCan>> gasCans = new List<WeakReference<GasCan>>();
        private float lastGasCanTickOffset = 0;
        private float minGasCanDistance = 231.72f; // Decently random 

        public List<Terrain> Chunks => data.AsTerrainList();

        public float FurthestOffset { get; private set; } = 0;

        public float MasterOffset {
            get => data.terrainOffset;
            set {
                float dif = value - data.terrainOffset;
                data.terrainOffset = value;
                if (value > FurthestOffset) FurthestOffset = value;
                if ((int) (data.terrainOffset + data.Domain / 2) / (int) data.Domain > data.integerOffset) {
                    data.integerOffset++;
                    data.MoveLeft();
                } else if ((int) (data.terrainOffset + data.Domain / 2) / (int) data.Domain < data.integerOffset) {
                    data.integerOffset--;
                    data.MoveRight();
                }
                foreach (var reference in gasCans) {
                    if (reference.TryGetTarget(out GasCan gasCan)) {
                        gasCan.pos.X += dif;
                    }
                }
            }
        }

        public InfiniteTerrain(Game game, SpriteBatch spriteBatch, Terrain terrain, int trimLength, int genLenth)
            : base(game) {
            this.spriteBatch = spriteBatch;
            data = new ShortTerrainDeQueue(terrain, trimLength, genLenth, GraphicsDevice.Viewport.Bounds.Width / 2f);
        }

        public override void Draw(GameTime gameTime) {
            foreach (var terrain in data.AsTerrainList()) terrain.Draw(gameTime);
        }

        public override void Update(GameTime gameTime) { }

        public bool HasNewGasCan(out GasCan gasCan) {
            if (-MasterOffset - lastGasCanTickOffset > minGasCanDistance) {
                gasCans.RemoveAll(reference => {
                    if (reference.TryGetTarget(out GasCan g) == false) return true;
                    if (g.pos.X < -100) g.Perished = true;
                    return g.Perished;
                });
                lastGasCanTickOffset += minGasCanDistance;
                if (r.Next(10) < 1) {
                    GasCan g = new GasCan(Game, spriteBatch, Vector2.Zero);
                    float x = GraphicsDevice.Viewport.Width + g.tex.Width, y = ExtremeHeightAt(x, 30,false);
                    if (y != float.MaxValue) {
                        y -= 35;
                        g.pos = new Vector2(x, y);
                        gasCans.Add(new WeakReference<GasCan>(g));
                        gasCan = g;
                        return true;
                    }

                }
            }
            gasCan = null;
            return false;
        }

        public float ExtremeHeightAt(float xStart, float dx, bool maxNotMin = true) {
            float y = maxNotMin ? float.MinValue : float.MaxValue;
            foreach (Terrain chunk in Chunks) {
                Rectangle chunkBounds = chunk.terrain.BoundingBox;
                if (chunkBounds.X < xStart && xStart + dx < chunkBounds.X + chunkBounds.Width)
                    foreach (Vector2 vertex in chunk.terrain.BoundingVertices)
                        if (xStart < vertex.X && vertex.X < xStart + dx)
                            y = maxNotMin ? Math.Max(y, vertex.Y) : Math.Min(y, vertex.Y);
            }
            return y;
        }
        
        public float AverageHeightAt(float xStart, float dx) {
            float y = 0;
            int count = 0;
            foreach (Terrain chunk in Chunks) {
                Rectangle chunkBounds = chunk.terrain.BoundingBox;
                if (chunkBounds.X < xStart && xStart + dx < chunkBounds.X + chunkBounds.Width)
                    foreach (Vector2 vertex in chunk.terrain.BoundingVertices)
                        if (xStart < vertex.X && vertex.X < xStart + dx) {
                            y += vertex.Y;
                            count++;
                        }
            }
            if (count == 0) return 0;
            else return y / count;
        }

        private class ShortTerrainDeQueue { //TODO rename
            private TdqNode center;
            private int genLength;
            private int trimLength;
            public float terrainOffset; // The location in the infinite terrain
            public readonly float drawOffset; // The draw position of the infinite terrain
            public int integerOffset = 0; // which `chunk` are we currently in

            public float Domain => center.terrain.domain;

            public ShortTerrainDeQueue(Terrain startingTerrain, int trimLength, int genLength, float drawOffset) {
                this.trimLength = trimLength;
                this.genLength = genLength;
                center = new TdqNode(this, startingTerrain, null, null);
                terrainOffset = 0;
                this.drawOffset = drawOffset;
                GenLeft();
                GenRight();
            }

            public List<Terrain> AsTerrainList() { //Todo: Make this only return terrain pieces worth drawing.
                var terrains = new List<Terrain>();
                var current = center;
                while (current.right != null) current = current.right;
                do {
                    var tempTerrain = current.terrain;
                    tempTerrain.Offset = new Vector2(drawOffset + terrainOffset + current.offsetFromMain,
                        tempTerrain.Offset.Y);
                    terrains.Add(tempTerrain);
                    current = current.left;
                } while (current != null);
                return terrains;
            }

            public void MoveLeft() {
                center = center.left;
                GenLeft();
            }

            public void MoveRight() {
                center = center.right;
                GenRight();
            }

            public void GenLeft() {
                var current = center;
                var d = 0;
                while (++d < genLength) {
                    if (current.left == null)
                        current.left = new TdqNode(this,
                            current.terrain.NewAdjacentLeft(),
                            current, null);
                    current = current.left;
                }

                TrimRight();
            }

            public void GenRight() {
                var current = center;
                var d = 0;
                while (++d < genLength) {
                    if (current.right == null)
                        current.right = new TdqNode(this,
                            current.terrain.NewAdjacentRight(),
                            null, current);
                    current = current.right;
                }
                TrimLeft();
            }

            public void TrimLeft() {
                var current = center;
                var d = 0;
                while ((current = current.left) != null)
                    if (++d > trimLength)
                        current.CutRecursiveLeft();
            }

            public void TrimRight() {
                var current = center;
                var d = 0;
                while ((current = current.right) != null)
                    if (++d > trimLength)
                        current.CutRecursiveRight();
            }

            private class TdqNode {
                public TdqNode right, left;
                public Terrain terrain;
                public float offsetFromMain;

                public TdqNode(ShortTerrainDeQueue owner, Terrain terrain, TdqNode right, TdqNode left) {
                    this.terrain = terrain;
                    if (right == null && left == null) {
                        offsetFromMain = 0;
                    } else if (right != null) {
                        this.right = right;
                        offsetFromMain = right.offsetFromMain - terrain.domain;
                    } else {
                        this.left = left;
                        offsetFromMain = left.offsetFromMain + terrain.domain;
                    }
                }


                public void CutRecursiveLeft() {
                    if (right != null) {
                        right.left = null;
                        right = null;
                    }
                    if (left == null) return;
                    left.CutRecursiveLeft();
                    left = null;
                }

                public void CutRecursiveRight() {
                    if (left != null) {
                        left.right = null;
                        left = null;
                    }
                    if (right == null) return;
                    right.CutRecursiveRight();
                    right = null;
                }
            }
        }
    }
}