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
    /// <summary>
    /// Manages collisions. There is an internal array of Collidables that can be modified from the outside, and on
    /// and update, this will check for all potential collisions that occur, and send logs of those collisions to the
    /// objects involved.
    /// </summary>
    public class CollisionManager : DrawableGameComponent {
        private bool debug = false; // When set to true, all ICollidables will have their bounding boxes displayed.

        /// <summary>
        /// A list of weak references of all objects to test collisions for. This is a self cleaning list, since it
        /// uses weak references, any item cleaned up by the garbage collector can and will be removed, as will
        /// any perishable that perishes.
        /// </summary>
        private readonly List<WeakReference<ICollidable>> collidables = new List<WeakReference<ICollidable>>();

        public CollisionManager(Game game) : base(game) { }

        /// <summary>
        /// Updating the Collision manager will test all combinations (specifically not permutations) of possible
        /// collisions of objects in the <c>collidables</c> list. For each potential collision, it is first checked
        /// if the axis aligned bounding boxes collide as that is very cheap and allows us to skip expensive checking
        /// on things that will never collide. Then,  
        /// </summary>
        /// <param name="gameTime">Unused.</param>
        public override void Update(GameTime gameTime) {
            collidables.RemoveAll(weakReference =>
                weakReference.TryGetTarget(out ICollidable c) == false || c is IPerishable p && p.Perished);
            List<CollisionLog>[] collisionLogs = new List<CollisionLog>[collidables.Count];
            for (int i = 0; i < collisionLogs.Length; i++) collisionLogs[i] = new List<CollisionLog>();
            for (int i = 0; i < collidables.Count - 1; i++)
            for (int j = i + 1; j < collidables.Count; j++)
                if (collidables[i].TryGetTarget(out ICollidable left) &&
                    collidables[j].TryGetTarget(out ICollidable right) &&
                    CheckAabbCollision(left, right)
                ) {
                    if (left.CollisionNotificationLevel == None && right.CollisionNotificationLevel == None
                        || left.CanCollide == false
                        || right.CanCollide == false)
                        continue;
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
                        // We need to be able to reference both the left and right as Complex Collidables
                        ICollidableComplex
                            leftC = left is ICollidableComplex tl ? tl : new DefaultComplexCollidable(left),
                            rightC = right is ICollidableComplex tr ? tr : new DefaultComplexCollidable(right);

                        // A place to store all collisions (assuming we care about them)
                        List<Vector2> collisionLocations =
                            left.CollisionNotificationLevel >= Partner ||
                            right.CollisionNotificationLevel >= Partner
                                ? new List<Vector2>() // At least one requires specific locations
                                : null; // Neither require specific locations

                        // If we don't care about specific locations, this lets us short circuit our collision finds.
                        // IF `collisionLocations == null` THEN `lookForCollisions == false →implies→ collision found`
                        bool lookForCollisions = true;

                        if (leftC.BoundingLinesLoop && leftC.BoundingLinesFormConvexPolygon &&
                            rightC.BoundingLinesLoop && rightC.BoundingLinesFormConvexPolygon) {
                            //TODO logic for convex polygons
                            // } else if (leftC.BoundingLinesLoop && rightC.BoundingLinesLoop) {
                            //     //TODO logic for concave polygons
                            //     //We never had to worry about this one because
                            //     //everything fits the other two categories
                        } else {
                            int leftItCount = leftC.BoundingVertices.Length - (leftC.BoundingLinesLoop ? 0 : 1);
                            int rightItCount = rightC.BoundingVertices.Length - (rightC.BoundingLinesLoop ? 0 : 1);
                            for (int p = 0; lookForCollisions && p < leftItCount; p++)
                            for (int q = 0; lookForCollisions && q < rightItCount; q++) {
                                Vector2 // The raw points (allowing for rollover to make loops)
                                    p0 = leftC.BoundingVertices[p],
                                    p1 = leftC.BoundingVertices[(p + 1) % leftC.BoundingVertices.Length],
                                    q0 = rightC.BoundingVertices[q], //FLAG possible performance issue here and 1 below
                                    q1 = rightC.BoundingVertices[(q + 1) % rightC.BoundingVertices.Length];
                                Vector2 // End points for `p + s` or `q + s` form
                                    s1 = new Vector2(p1.X - p0.X, p1.Y - p0.Y),
                                    s2 = new Vector2(q1.X - q0.X, q1.Y - q0.Y);
                                float // Solve for s & t
                                    s = (-s1.Y * (p0.X - q0.X) + s1.X * (p0.Y - q0.Y)) / (-s2.X * s1.Y + s1.X * s2.Y),
                                    t = (s2.X * (p0.Y - q0.Y) - s2.Y * (p0.X - q0.X)) / (-s2.X * s1.Y + s1.X * s2.Y);

                                if (0 <= s && s <= 1 && 0 <= t && t <= 1) { // If this is true then COLLISION!
                                    if (collisionLocations != null)
                                        collisionLocations.Add(new Vector2(
                                            p0.X + (t * s1.X),
                                            p0.Y + (t * s1.Y)));
                                    else lookForCollisions = false;
                                }
                            }
                        }
                        // Now time to log this
                        if (collisionLocations == null) { // Care about no more than partner (NONE falls here too)
                            if (lookForCollisions == false) { // At least one collision found
                                if (left.CollisionNotificationLevel == Partner) // Only add for partner / NONE gets none
                                    collisionLogs[i].Add(new CollisionLog(right));
                                if (right.CollisionNotificationLevel == Partner) // same ↑
                                    collisionLogs[j].Add(new CollisionLog(left));
                            }
                        } else { // Care about collision locations
                            if (collisionLocations.Count > 0) { // Collisions were found
                                if (left.CollisionNotificationLevel == Location)
                                    collisionLogs[i].Add(new CollisionLog(right, collisionLocations.ToArray()));
                                else if (left.CollisionNotificationLevel == Partner)
                                    collisionLogs[i].Add(new CollisionLog(right));
                                // ↑ Left | Right ↓
                                if (right.CollisionNotificationLevel == Location)
                                    collisionLogs[j].Add(new CollisionLog(left, collisionLocations.ToArray()));
                                else if (right.CollisionNotificationLevel == Partner)
                                    collisionLogs[j].Add(new CollisionLog(right));
                            }
                        }
                    }
                }
            // Giving the Collidables their collision logs
            for (int i = 0; i < collidables.Count; i++)
                if (collidables[i].TryGetTarget(out ICollidable item))
                    item.CollisionLogs = collisionLogs[i];
        }


        /// <summary>
        /// Debug only; Draws the bounding boxes of all collidables, highlighting them if something collides.
        /// </summary>
        /// <param name="gameTime">Unused</param>
        public override void Draw(GameTime gameTime) {
            if (debug)
                foreach (var reference in collidables)
                    if (reference.TryGetTarget(out ICollidable item)) {
                        Sprite.DrawBoundingBox(item.AABB, (Game1) Game,
                            item.CollisionLogs.Count == 0 ? ColourSchemes.normRed : Color.Wheat);
                        foreach (CollisionLog log in item.CollisionLogs) {
                            foreach (Vector2 loc in log.collisionLocations)
                                Sprite.DrawBoundingBox(
                                    new Rectangle(loc.ToPoint() - new Point(5), new Point(10)),
                                    (Game1) Game, Color.Wheat
                                );
                        }
                    }
        }

        /// <summary>
        /// Adds a Collidable to the list of collidables.
        /// </summary>
        /// <param name="item">Unused</param>
        public void Add(ICollidable item) => collidables.Add(new WeakReference<ICollidable>(item));

        /// <summary>
        /// Tells if a specific item is in the list of collidables.w
        /// </summary>
        /// <param name="item">Item to check</param>
        /// <returns>True if it exists in the list, otherwise false.</returns>
        public bool Contains(ICollidable item) {
            foreach (var reference in collidables)
                if (reference.TryGetTarget(out ICollidable target) && target == item)
                    return true;
            return false;
        }

        /// <summary>
        /// Removes a Collidable to the list of collidables.
        /// </summary>
        /// <param name="item">Unused</param>
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

    /// <summary>
    /// Represents an object that can collide with another of the same type. These objects can be added to an instance
    /// of the <c>CollisionManager</c> class to automatically check for collisions. This interface is for objects
    /// defined in a very basic sense, for more complex objects use <see cref="ICollidableComplex"/>
    /// </summary>
    public interface ICollidable {
        /// <summary>
        /// Can the object currently collide with anything?
        /// </summary>
        bool CanCollide { get; }

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

    /// <summary>
    /// <p><inheritdoc cref="ICollidable"/></p>
    /// <p>This specific interface represents more complex collidables, those that are described by strips of
    /// connected vertices instead of by AABB's</p>
    /// </summary>
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
        /// <summary>
        /// The actual Collidable object that this represents. This is where all the data comes from.
        /// </summary>
        private readonly ICollidable collidable;

        /// <summary>
        /// <inheritdoc cref="ICollidable.CanCollide"/>
        /// </summary>
        public bool CanCollide => collidable.CanCollide;

        /// <summary>
        /// <inheritdoc cref="ICollidable.AABB"/>
        /// </summary>
        public Rectangle AABB => collidable.AABB;

        /// <summary>
        /// <inheritdoc cref="ICollidable.CollisionNotificationLevel"/>
        /// </summary>
        public CollisionNotificationLevel CollisionNotificationLevel => collidable.CollisionNotificationLevel;

        /// <summary>
        /// <inheritdoc cref="ICollidable.CollisionLogs"/>
        /// </summary>
        public List<CollisionLog> CollisionLogs {
            get => collidable.CollisionLogs;
            set => collidable.CollisionLogs = value;
        }

        private Rectangle lastAABB;
        private Vector2[] lastBoundingVertices;

        /// <summary>
        /// <p><inheritdoc cref="ICollidableComplex.BoundingVertices"/></p>
        /// Note: This also caches the bounding vertices for performance reasons.
        /// This has adverse effect on the reliability of the returned value.
        /// </summary>
        public Vector2[] BoundingVertices {
            get {
                if (AABB == lastAABB) return lastBoundingVertices;
                AABB.Deconstruct(out int x, out int y, out int dx, out int dy);
                return lastBoundingVertices = new[] {
                    new Vector2(x + 10, y + 10),
                    new Vector2(x, y + dy),
                    new Vector2(x + dx - 10, y + dy - 10),
                    new Vector2(x + dx, y)
                };
                lastAABB = AABB;
            }
        }

        /// <summary>
        /// <inheritdoc cref="ICollidableComplex.BoundingLinesLoop"/>
        /// </summary>
        public bool BoundingLinesLoop => true;

        /// <summary>
        /// <inheritdoc cref="ICollidableComplex.BoundingLinesFormConvexPolygon"/>
        /// </summary>
        public bool BoundingLinesFormConvexPolygon => true;

        /// <summary>
        /// Creates a Complex Collidable that represents the given Collidable
        /// </summary>
        /// <param name="collidable">The Collidable to represent</param>
        public DefaultComplexCollidable(ICollidable collidable) => this.collidable = collidable;
    }

    /// <summary>
    /// Used by <see cref="ICollidable"/> to register how much information about the collision to receive.
    /// </summary>
    public enum CollisionNotificationLevel {
        None, // For when you don't care at all
        Partner, // Only care about who you collided with
        Location // Who did you collide with, and where did it happen
    }

    /// <summary>
    /// Represents a collision with a specific collidable. This always contains who the given Collidable collided with,
    /// and may contain the exact location of the collision.
    /// </summary>
    public class CollisionLog {
        /// <summary>The Collidable that this collided with. This will always have a value. </summary>
        public readonly ICollidable collisionPartner;

        /// <summary>
        /// The point(s) where the bounding lines intersected. Length will be 0 if this information is not requested
        /// </summary>
        public readonly Vector2[] collisionLocations;

        /// <summary>
        /// Creates a collision log with only the collision partner.
        /// </summary>
        /// <param name="collisionPartner">The Collidable this collided with</param>
        public CollisionLog(ICollidable collisionPartner) : this(collisionPartner, new Vector2[0]) { }

        /// <summary>
        /// Creates a collision log with both a partner and collision points
        /// </summary>
        /// <param name="collisionPartner">The Collidable this collided with</param>
        /// <param name="collisionLocations">The points where collisions occured.</param>
        public CollisionLog(ICollidable collisionPartner, Vector2[] collisionLocations) {
            this.collisionPartner = collisionPartner;
            this.collisionLocations = collisionLocations;
        }
    }
}