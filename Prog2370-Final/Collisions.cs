using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Prog2370_Final.Drawable;
using Prog2370_Final.Drawable.Sprites;
using static Prog2370_Final.CollisionNotificationLevel;

namespace Prog2370_Final {
    public class CollisionManager : DrawableGameComponent {
        private bool debug = true;
        private readonly List<WeakReference<ICollidable>> collidables = new List<WeakReference<ICollidable>>();

        public CollisionManager(Game game) : base(game) { }

        public override void Update(GameTime gameTime) {
            collidables.RemoveAll(weakReference => weakReference.TryGetTarget(out ICollidable ignored) == false);
            List<CollisionLog>[] collisionLogs = new List<CollisionLog>[collidables.Count];
            for (int i = 0; i < collisionLogs.Length; i++) collisionLogs[i] = new List<CollisionLog>();
            for (int i = 0; i < collidables.Count - 1; i++)
            for (int j = i + 1; j < collidables.Count; j++)
                if (collidables[i].TryGetTarget(out ICollidable left) &&
                    collidables[j].TryGetTarget(out ICollidable right) &&
                    CheckAabbCollision(left, right)
                ) {
                    if (left.CollisionNotificationLevel == None && right.CollisionNotificationLevel == None) continue;
                    // First case: both objects are simple
                    if (!(left is ICollidableComplex) && !(right is ICollidableComplex)) {
                        switch (left.CollisionNotificationLevel) {
                            case Location: // TODO add detailed...
                            case Partner:
                                collisionLogs[i].Add(new CollisionLog(right));
                                break;
                        }
                        switch (right.CollisionNotificationLevel) {
                            case Location: // TODO add detailed...
                            case Partner:
                                collisionLogs[j].Add(new CollisionLog(left));
                                break;
                        }
                    }
                    // Second case: At least one is a complex collidable
                    else {
                        ICollidableComplex
                            leftC = left is ICollidableComplex tl ? tl : new DefaultComplexCollidable(left),
                            rightC = right is ICollidableComplex tr ? tr : new DefaultComplexCollidable(right);
                        if (leftC.BoundingLinesLoop && leftC.BoundingLinesFormConvexPolygon &&
                            rightC.BoundingLinesLoop && rightC.BoundingLinesFormConvexPolygon) {
                            //TODO logic for convex polygons
                            // } else if (leftC.BoundingLinesLoop && rightC.BoundingLinesLoop) {
                            //     //TODO logic for concave polygons
                        } else {
                            const double error = 10;
                            int leftItCount = leftC.BoundingVertices.Length - (leftC.BoundingLinesLoop ? 0 : 1);
                            int rightItCount = rightC.BoundingVertices.Length - (rightC.BoundingLinesLoop ? 0 : 1);
                            for (int p = 0; p < leftItCount; p++)
                            for (int q = 0; q < rightItCount; q++) {
                                Vector2
                                    p0 = leftC.BoundingVertices[p],
                                    p1 = leftC.BoundingVertices[(p + 1) % leftC.BoundingVertices.Length],
                                    q0 = rightC.BoundingVertices[q],
                                    q1 = rightC.BoundingVertices[(q + 1) % rightC.BoundingVertices.Length];
                                Vector2
                                    s1 = new Vector2(p1.X - p0.X, p1.Y - p0.Y),
                                    s2 = new Vector2(q1.X - q0.X, q1.Y - q0.Y);
                                float s, t;
                                s = (-s1.Y * (p0.X - q0.X) + s1.X * (p0.Y - q0.Y)) / (-s2.X * s1.Y + s1.X * s2.Y);
                                t = (s2.X * (p0.Y - q0.Y) - s2.Y * (p0.X - q0.X)) / (-s2.X * s1.Y + s1.X * s2.Y);

                                if (s >= 0 && s <= 1 && t >= 0 && t <= 1) {
                                    // Collision!
                                    Vector2 location = new Vector2(p0.X + (t * s1.X), p0.Y + (t * s1.Y));
                                    collisionLogs[i].Add(new CollisionLog(right, location));
                                    collisionLogs[j].Add(new CollisionLog(left, location));
                                }
                            }
                        }
                    }
                }
            for (int i = 0; i < collidables.Count; i++)
                if (collidables[i].TryGetTarget(out ICollidable item))
                    item.CollisionLogs = collisionLogs[i];
        }


        public override void Draw(GameTime gameTime) {
            if (debug)
                foreach (var reference in collidables)
                    if (reference.TryGetTarget(out ICollidable item)) {
                        Sprite.DrawBoundingBox(item.AABB, (Game1) Game,
                            item.CollisionLogs.Count == 0 ? ColourSchemes.normRed : Color.Wheat);
                        foreach (CollisionLog log in item.CollisionLogs) {
                            if (log.collisionLocation != null)
                                Sprite.DrawBoundingBox(
                                    new Rectangle(log.collisionLocation.Value.ToPoint() - new Point(5), new Point(10)),
                                    (Game1) Game, Color.Wheat
                                );
                        }
                    }
        }

        public void Add(ICollidable item) => collidables.Add(new WeakReference<ICollidable>(item));

        public bool Contains(ICollidable item) {
            foreach (var reference in collidables)
                if (reference.TryGetTarget(out ICollidable target) && target == item)
                    return true;
            return false;
        }

        public bool Remove(ICollidable item) {
            foreach (var reference in collidables)
                if (reference.TryGetTarget(out ICollidable target) && target == item)
                    return collidables.Remove(reference);
            return false;
        }

        /// <summary>
        /// Tests if two <c>ICollidable</c> objects' AABBs' overlap. True if they do, false otherwise.
        /// </summary>
        /// <param name="left">One of the two objects to check</param>
        /// <param name="right">The second of the two objects to check</param>
        /// <returns>True if the objects AABBs' intersect</returns>
        private static bool CheckAabbCollision(ICollidable left, ICollidable right)
            => Rectangle.Intersect(left.AABB, right.AABB) != Rectangle.Empty;
    }

    public interface ICollidable {
        /// <summary>
        /// An axis-aligned bounding box is a rectangle whose sides are all parallel to either the X or Y axis.
        /// This specific bounding box should completely surround the true bounds of the object, as these bounds
        /// will be used to determine if further collision checking should be done. 
        /// </summary>
        Rectangle AABB { get; }

        /// <summary>
        /// What level of collision information would you like to receive?
        /// </summary>
        CollisionNotificationLevel CollisionNotificationLevel { get; }

        /// <summary>
        /// When all the collision testing is done, this ICollidable object will be given a list of all collisions
        /// logged to the level that the <c>CollisionNotificationLevel</c> property specifies.
        /// </summary>
        List<CollisionLog> CollisionLogs { get; set; }
    }

    public interface ICollidableComplex : ICollidable {
        /// <summary>
        /// A series of vectors which represent the outer-most vertices of an object, defining the shape the object
        /// takes. The shapes drawn by these vertices are the shapes tested for collision. If zero vertices are given,
        /// this object's <c>AABB</c> will be considered the primary bounding box.
        /// </summary>
        Vector2[] BoundingVertices { get; }

        /// <summary>
        /// Whether or not the first and last vertices are meant to be connected. If this is true, the vertices will
        /// form a closed polygon with <c>n</c> sides, and if it is set to false the vertices will instead define
        /// a (potentially) jagged line made of <c>n-1</c> connected line segments.
        /// </summary>
        bool BoundingLinesLoop { get; }

        /// <summary>
        /// Note: if <c>BoundingLinesLoop</c> is set to false, this property will be assumed to be false as well.
        /// Whether or not the polygon drawn. Convex polygons are easier to check for intersections,
        /// especially for the case that one polygon is inside the other. For the sake of simplicity, I will only
        /// check if one polygon is inside the other if they are both convex. 
        /// </summary>
        bool BoundingLinesFormConvexPolygon { get; }
    }

    /// <summary>
    /// This can be used to represent an <c>ICollidable</c> object as an <c>ICollidableComplex</c> object without
    /// needing to manually set the other properties.
    /// </summary>
    public class DefaultComplexCollidable : ICollidableComplex {
        private readonly ICollidable collidable;

        public Rectangle AABB => collidable.AABB;
        public CollisionNotificationLevel CollisionNotificationLevel => collidable.CollisionNotificationLevel;

        public List<CollisionLog> CollisionLogs {
            get => collidable.CollisionLogs;
            set => collidable.CollisionLogs = value;
        }

        public Vector2[] BoundingVertices {
            get {
                AABB.Deconstruct(out int x, out int y, out int dx, out int dy);
                return new[] {
                    new Vector2(x + 10, y + 10),
                    new Vector2(x, y + dy),
                    new Vector2(x + dx - 10, y + dy - 10),
                    new Vector2(x + dx, y)
                };
            }
        }

        public bool BoundingLinesLoop => true;
        public bool BoundingLinesFormConvexPolygon => true;
        public DefaultComplexCollidable(ICollidable collidable) => this.collidable = collidable;
    }

    public enum CollisionNotificationLevel {
        None, // For when you don't care at all
        Partner, // Only care about who you collided with
        Location // Who did you collide with, and where did it happen
    }

    public class CollisionLog {
        public readonly ICollidable collisionPartner;
        public readonly Vector2? collisionLocation;

        public CollisionLog(
            ICollidable collisionPartner,
            Vector2? collisionLocation = null,
            Vector2? theirCollisionLocation = null
        ) {
            this.collisionPartner = collisionPartner;
            this.collisionLocation = collisionLocation;
        }
    }
}