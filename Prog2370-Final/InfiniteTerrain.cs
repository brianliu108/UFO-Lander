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
            data.MoveRight();
            data.MoveRight();
            data.MoveRight();
        }

        public override void Draw(GameTime gameTime) {
            foreach (var terrain in data.AsTerrainList()) terrain.Draw(gameTime);
        }

        public override void Update(GameTime gameTime) {
            data.mainOffset -= 2; //TODO remove this debug line eventually
            if ((int) data.mainOffset / (int) data.Domain > data.integerOffset) {
                data.integerOffset++;
                data.MoveLeft();
            } else if ((int) data.mainOffset / (int) data.Domain < data.integerOffset) {
                data.integerOffset--;
                data.MoveRight();
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
                center = new TdqNode(this, startingTerrain, null, null);
                mainOffset = 0;
                GenLeft();
                GenRight();
            }

            public List<Terrain> AsTerrainList() { //Todo: Make this only return terrain pieces worth drawing.
                var terrains = new List<Terrain>();
                var current = center;
                while (current.right != null) current = current.right;
                do {
                    var tempTerrain = current.terrain;
                    tempTerrain.Offset = new Vector2(mainOffset + current.offsetFromMain, tempTerrain.Offset.Y);
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
                private readonly ShortTerrainDeQueue owner;
                public TdqNode right, left;
                public Terrain terrain;
                public float offsetFromMain;

                public TdqNode(ShortTerrainDeQueue owner, Terrain terrain, TdqNode right, TdqNode left) {
                    this.owner = owner;
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

                public void Cut() {
                    CutRight();
                    CutLeft();
                }

                public void CutRight() {
                    if (right == null) return;
                    right.left = null;
                    right = null;
                }

                public void CutLeft() {
                    if (left == null) return;
                    left.right = null;
                    left = null;
                }

                public void CutRecursiveLeft() {
                    CutRight();
                    if (left == null) return;
                    left.CutRecursiveLeft();
                    left = null;
                }

                public void CutRecursiveRight() {
                    CutLeft();
                    if (right == null) return;
                    right.CutRecursiveRight();
                    right = null;
                }
            }
        }
    }
}