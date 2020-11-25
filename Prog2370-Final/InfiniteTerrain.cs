using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Prog2370_Final {
    public class InfiniteTerrain : DrawableGameComponent {
        private ShortTerrainDeQueue data;
        private SpriteBatch spriteBatch;

        public InfiniteTerrain(Game game, SpriteBatch spriteBatch, Terrain terrain, int trimLength, int genLenth)
            : base(game) {
            this.spriteBatch = spriteBatch;
            data = new ShortTerrainDeQueue(terrain, trimLength, genLenth);
        }

        public override void Draw(GameTime gameTime) {
            foreach (var terrain in data.AsTerrainList())
                terrain.Draw(gameTime);
        }

        public override void Update(GameTime gameTime) {
            data.mainOffset -= 2;
            if ((int) data.mainOffset / (int) data.Domain > data.integerOffset) {
                data.integerOffset++;
                data.MoveRight();
            } else if ((int) data.mainOffset / (int) data.Domain < data.integerOffset) {
                data.integerOffset--;
                data.MoveLeft();
            }
        }

        private class ShortTerrainDeQueue { //TODO rename
            private TdqNode center;
            private int genLength;
            private int trimLength;
            public float mainOffset = 0;
            public int integerOffset = 0;

            public float Domain => center.terrain.domain;

            public ShortTerrainDeQueue(Terrain startingTerrain, int trimLength, int genLength) {
                this.trimLength = trimLength;
                this.genLength = genLength;
                this.center = new TdqNode(this, startingTerrain, null, null);
                mainOffset = 0;
                this.GenRight();
                this.GenLeft();
            }

            public List<Terrain> AsTerrainList() {
                List<Terrain> terrains = new List<Terrain>();
                var current = center;
                while (current.left != null) current = current.left;
                do {
                    Terrain tempTerrain = current.terrain;
                    tempTerrain.Offset = new Vector2(mainOffset + current.offsetFromMain, tempTerrain.Offset.Y);
                    terrains.Add(tempTerrain);
                    current = current.right;
                } while (current != null);
                return terrains;
            }

            public void MoveRight() {
                center = center.right;
                GenRight();
            }

            public void MoveLeft() {
                center = center.left;
                GenLeft();
            }

            public void GenRight() {
                var current = center;
                var d = 0;
                while (++d < genLength) {
                    if (current.right == null) {
                        current.right = new TdqNode(this,
                            current.terrain.NewAdjacentRight(),
                            current, null);
                    }
                    current = current.right;
                }

                TrimLeft();
            }

            public void GenLeft() {
                var current = center;
                var d = 0;
                while (++d < genLength) {
                    if (current.left == null) {
                        current.left = new TdqNode(this,
                            current.terrain.NewAdjacentLeft(),
                            null, current);
                    }
                    current = current.left;
                }
                TrimRight();
            }

            public void TrimRight() {
                var current = center;
                var d = 0;
                while ((current = current.right) != null)
                    if (++d > trimLength)
                        current.CutRecursiveRight();
            }

            public void TrimLeft() {
                var current = center;
                var d = 0;
                while ((current = current.left) != null)
                    if (++d > trimLength)
                        current.CutRecursiveLeft();
            }

            private class TdqNode {
                private readonly ShortTerrainDeQueue owner;
                public TdqNode left, right;
                public Terrain terrain;
                public float offsetFromMain;

                public TdqNode(ShortTerrainDeQueue owner, Terrain terrain, TdqNode left, TdqNode right) {
                    this.owner = owner;
                    this.terrain = terrain;
                    if (left == null && right == null) {
                        offsetFromMain = 0;
                    } else if (left != null) {
                        this.left = left;
                        offsetFromMain = left.offsetFromMain - terrain.domain;
                    } else {
                        this.right = right;
                        offsetFromMain = right.offsetFromMain + terrain.domain;
                    }
                }

                public void Cut() {
                    CutLeft();
                    CutRight();
                }

                public void CutLeft() {
                    if (left == null) return;
                    left.right = null;
                    left = null;
                }

                public void CutRight() {
                    if (right == null) return;
                    right.left = null;
                    right = null;
                }

                public void CutRecursiveRight() {
                    CutLeft();
                    if (right == null) return;
                    right.CutRecursiveRight();
                    right = null;
                }

                public void CutRecursiveLeft() {
                    CutRight();
                    if (left == null) return;
                    left.CutRecursiveLeft();
                    left = null;
                }
            }
        }
    }
}