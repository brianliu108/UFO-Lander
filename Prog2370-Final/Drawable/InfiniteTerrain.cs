using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Prog2370_Final.Drawable {
    public class InfiniteTerrain : DrawableGameComponent {
        private ShortTerrainDeQueue data;
        private SpriteBatch spriteBatch;
        public List<Terrain> Chunks => data.AsTerrainList();

        public float MasterOffset {
            get => data.terrainOffset;
            set {
                data.terrainOffset = value;
                if ((int) (data.terrainOffset + data.Domain / 2) / (int) data.Domain > data.integerOffset) {
                    data.integerOffset++;
                    data.MoveLeft();
                } else if ((int) (data.terrainOffset + data.Domain / 2) / (int) data.Domain < data.integerOffset) {
                    data.integerOffset--;
                    data.MoveRight();
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

        public override void Update(GameTime gameTime) {
            // data.terrainOffset -= 5; // Terrain moves left
            // data.terrainOffset += 5; // Terrain moves right
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