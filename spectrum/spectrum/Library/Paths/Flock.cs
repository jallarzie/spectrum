using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Spectrum.Library.Geometry;
using System;
using Spectrum.Library.Graphics;
using System.Collections.Generic;
using Spectrum.Components;

namespace Spectrum.Library.Paths
{
    class Flock : DistantFollow
    {
        private static float NEIGHBOR_DISTANCE = 150; // in pixels
        private static float SEPARATION_DISTANCE = 75; // in pixels
        private static float WEIGHT_ALIGNMENT = 1;
        private static float WEIGHT_COHESION = 1;
        private static float WEIGHT_SEPARATION = 200;
        private static float WEIGHT_FOLLOW = 1;
        private static float WEIGHT_AVOID_CORE = 100;

        private List<Enemy> flockmates;
        private PowerCore core;

        public Flock(CoordinateSystem entity, List<Enemy> flockmates, PowerCore core, Entity2D target, int distance, int leeway)
            : base(entity, target, distance, leeway)
        {
            this.flockmates = flockmates;
            this.core = core;
        }

        public override Vector2 Move(float distance)
        {
            List<Flock> neighbors = Neighbors();

            Vector2 alignment = Alignment(neighbors) * WEIGHT_ALIGNMENT;
            Vector2 cohesion = Cohesion(neighbors) * WEIGHT_COHESION;
            Vector2 separation = Separation(neighbors) * WEIGHT_SEPARATION;
            Vector2 follow = Follow() * WEIGHT_FOLLOW;
            Vector2 avoid = AvoidCore() * WEIGHT_AVOID_CORE;

            if (moving)
            {
                Direction += alignment + cohesion + separation + follow + avoid;
                if (Direction.LengthSquared() > 1)
                    Direction.Normalize();
                Position += Direction * distance;
            }

            return base.Move(0);
        }

        // flockmates within NEIGHBOR_DISTANCE
        private List<Flock> Neighbors()
        {
            List<Flock> neighbors = new List<Flock>();
            foreach (Enemy mate in flockmates)
            {
                if (mate.Path is Flock)
                {
                    float distance = Vector2.Distance(Position, mate.Position);
                    if (distance < NEIGHBOR_DISTANCE)
                        neighbors.Add(mate.Path as Flock);
                }
            }
            return neighbors;
        }

        // points in the average direction of neighbors
        private Vector2 Alignment(List<Flock> neighbors)
        {
            Vector2 alignment = Vector2.Zero;
            foreach (Flock n in neighbors)
                alignment += n.Direction;
            if (neighbors.Count > 0)
                alignment /= neighbors.Count;
            if (alignment.LengthSquared() > 1)
                alignment.Normalize();
            return alignment;
        }

        // points toward the average position of neighbors
        private Vector2 Cohesion(List<Flock> neighbors)
        {
            Vector2 cohesion = Vector2.Zero;
            foreach (Flock n in neighbors)
                cohesion += n.Position;
            if (neighbors.Count > 0)
            {
                cohesion /= neighbors.Count;
                if (cohesion.LengthSquared() > 1)
                    cohesion.Normalize();
            }
            return cohesion;
        }

        // avoids neighbors that are closer than SEPARATION_DISTANCE
        private Vector2 Separation(List<Flock> neighbors)
        {
            Vector2 separation = Vector2.Zero;
            int count = 0;
            foreach (Flock n in neighbors)
            {
                Vector2 direction = Vector2.Subtract(Position, n.Position);
                float distance = direction.Length();
                if (distance > 0 && distance < SEPARATION_DISTANCE)
                {
                    direction.Normalize();
                    direction /= distance;
                    separation += direction;
                    count++;
                }
            }
            if (count > 0)
            {
                separation /= neighbors.Count;
                if (separation.LengthSquared() > 1)
                    separation.Normalize();
            }
            return separation;
        }

        // move away if PowerCore is closer than SEPARATION_DISTANCE
        private Vector2 AvoidCore()
        {
            Vector2 direction = Vector2.Subtract(Position, core.Position);
            float distance = direction.Length() - core.CalculateCurrentRadius() * 1.3f; // accounts for forcefield
            if (distance > 0 && distance < SEPARATION_DISTANCE)
            {
                direction.Normalize();
                direction /= distance;
                return direction;
            }
            else
                return Vector2.Zero;
        }

        // move towards the target of the Follow path
        private Vector2 Follow()
        {
            Vector2 direction = Vector2.Subtract(Target.Position, Position);
            float currentDistance = direction.Length();

            if ((moving && currentDistance > targetDistance) ||
                !moving && currentDistance > targetDistance + leeway)
            {
                direction.Normalize();
                moving = true;
                return direction;
            }
            else
            {
                moving = false;
                return Vector2.Zero;
            }
        }

    }
}
