using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Prog2370_Final.Drawable.Sprites;

namespace Prog2370_Final.Drawable {
    /// <summary>
    /// A drawable collection of terrain objects (referred to as chunks) that can extend infinitely to either direction.
    /// When the offset is moved, the portion of the terrain that is drawn is changed, and more terrain is loaded and
    /// unloaded as necessary.
    /// </summary>
    public class InfiniteTerrain : DrawableGameComponent {
        private SpriteBatch spriteBatch;
        private readonly Random r = new Random();
        private readonly ShortTerrainDeQueue data; // Main terrain data
        private readonly List<WeakReference<GasCan>> gasCans = new List<WeakReference<GasCan>>();
        private float lastGasCanTickOffset = 0;
        private float minGasCanDistance = 231.72f; // Decently random 

        /// <summary>
        /// Gets all the individual Terrain chunks as a list
        /// </summary>
        public List<Terrain> Chunks => data.AsTerrainList();

        /// <summary>
        /// The largest the offset has gotten.
        /// </summary>
        public float FurthestOffset { get; private set; } = 0;

        /// <summary>
        /// The main offset of the terrain. Changing this will change which portion of the terrain is displayed,
        /// and more chunks will be loaded or unloaded as necessary.
        /// </summary>
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

        /// <summary>
        /// Creates a new instance with a starting terrain, the number of chunks to get from the center, and the
        /// number to keep when trimming.
        /// </summary>
        /// <param name="game">The game creating this object</param>
        /// <param name="spriteBatch">The sprite batch used to draw this</param>
        /// <param name="terrain">The starting terrain chunk. Everything else will be generated relative to this</param>
        /// <param name="trimLength">How far to keep chunks when trimming</param>
        /// <param name="genLength">How far to gen chunks when generating</param>
        public InfiniteTerrain(Game game, SpriteBatch spriteBatch, Terrain terrain, int trimLength, int genLength)
            : base(game) {
            this.spriteBatch = spriteBatch;
            data = new ShortTerrainDeQueue(terrain, trimLength, genLength, GraphicsDevice.Viewport.Bounds.Width / 2f);
        }

        /// <summary>
        /// Draws all loaded chunks
        /// </summary>
        /// <param name="gameTime">Unused</param>
        public override void Draw(GameTime gameTime) {
            foreach (var terrain in data.AsTerrainList()) terrain.Draw(gameTime);
        }

        /// <summary>
        /// When called, this has a random chance to make a new GasCan.
        /// </summary>
        /// <param name="gasCan">The created gas can</param>
        /// <returns>Whether or not the reference has a gas can.</returns>
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
                    float x = GraphicsDevice.Viewport.Width + g.tex.Width, y = ExtremeHeightAt(x, 30, false);
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

        /// <summary>
        /// Gets the extreme (max or min) height of the terrain, starting at xStart and ending at xStart + dx
        /// </summary>
        /// <param name="xStart">The beginning of the sample range</param>
        /// <param name="dx">The length of the sample</param>
        /// <param name="maxNotMin">Looking for the max height, not the minimum</param>
        /// <returns>The extreme y in the x range</returns>
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

        /// <summary>
        /// Gets the average y height in a given x range 
        /// </summary>
        /// <param name="xStart">The beginning of the sample range</param>
        /// <param name="dx">The length of the sample</param>
        /// <returns>The extreme y in the x range</returns>
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

        /// <summary>
        /// A custom doubly linked list that hold onto the center node, and extends in either direction. 
        /// </summary>
        private class ShortTerrainDeQueue { //TODO rename
            private TdqNode center;
            private int genLength;
            private int trimLength;
            public float terrainOffset; // The location in the infinite terrain
            public readonly float drawOffset; // The draw position of the infinite terrain
            public int integerOffset = 0; // which `chunk` are we currently in

            /// <summary>
            /// Gets the domain of the terrain. Since all the terrains have the same domain, it does not matter which
            /// one it comes from.
            /// </summary>
            public float Domain => center.terrain.domain;

            /// <summary>
            /// Creates a new doubly linked list centered on the given terrain, then generation out on both sides.
            /// </summary>
            /// <param name="startingTerrain">The terrain to make in the center</param>
            /// <param name="trimLength">How many in one direction to keep when trimming</param>
            /// <param name="genLength">How many to generate in one direction</param>
            /// <param name="drawOffset">The relative offset of this terrain vs the original</param>
            public ShortTerrainDeQueue(Terrain startingTerrain, int trimLength, int genLength, float drawOffset) {
                this.trimLength = trimLength;
                this.genLength = genLength;
                center = new TdqNode(this, startingTerrain, null, null);
                terrainOffset = 0;
                this.drawOffset = drawOffset;
                GenLeft();
                GenRight();
            }

            /// <summary>
            /// Returns the data from all the nodes in the linked list as a <c>List</c>
            /// </summary>
            /// <returns>All the Terrains currently loaded</returns>
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

            /// <summary>
            /// Moves the center one to the left, generating more chunks to the left if required.
            /// </summary>
            public void MoveLeft() {
                center = center.left;
                GenLeft();
            }

            /// <summary>
            /// Moves the center one to the right, generating more chunks to the right if required.
            /// </summary>
            public void MoveRight() {
                center = center.right;
                GenRight();
            }

            /// <summary>
            /// Generates chunks to the left until the list reaches the length of <c>genLength</c> to the left.
            /// Trims the right side of the list.
            /// </summary>
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

            /// <summary>
            /// Generates chunks to the right until the list reaches the length of <c>genLength</c> to the right.
            /// Trims the left side of the list.
            /// </summary>
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

            /// <summary>
            /// Trims the left size to the length <c>trimLength</c>
            /// </summary>
            public void TrimLeft() {
                var current = center;
                var d = 0;
                while ((current = current.left) != null)
                    if (++d > trimLength)
                        current.CutRecursiveLeft();
            }

            /// <summary>
            /// Trims the right size to the length <c>trimLength</c>
            /// </summary>
            public void TrimRight() {
                var current = center;
                var d = 0;
                while ((current = current.right) != null)
                    if (++d > trimLength)
                        current.CutRecursiveRight();
            }

            /// <summary>
            /// The node for managing data.
            /// </summary>
            private class TdqNode {
                public TdqNode right, left;
                public Terrain terrain;
                public float offsetFromMain;

                /// <summary>
                /// Creates a mew node in the list.
                /// </summary>
                /// <param name="owner">The owner of the list</param>
                /// <param name="terrain">The terrain in this node</param>
                /// <param name="right">The node to the right</param>
                /// <param name="left">The node to the left</param>
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

                /// <summary>
                /// Cuts this node and every node to the left from the list
                /// </summary>
                public void CutRecursiveLeft() {
                    if (right != null) {
                        right.left = null;
                        right = null;
                    }
                    if (left == null) return;
                    left.CutRecursiveLeft();
                    left = null;
                }

                /// <summary>
                /// Cuts this node and every node to the right from the list
                /// </summary>
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