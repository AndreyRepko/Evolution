using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Evolution.Game.Model.Positions
{
    public static class PositionExtensions
    {
        public static Directions GetRelativePosition(this Position p1, in Position p2)
        {
            if (p1.X == p2.X && p1.Y > p2.Y) return Directions.Up;
            if (p1.X < p2.X && p1.Y < p2.Y) return Directions.UpRight;
            if (p1.X < p2.X && p1.Y == p2.Y) return Directions.Right;
            if (p1.X < p2.X && p1.Y > p2.Y) return Directions.DownRight;
            if (p1.X == p2.X && p1.Y < p2.Y) return Directions.Down;
            if (p1.X > p2.X && p1.Y < p2.Y) return Directions.DownLeft;
            if (p1.X > p2.X && p1.Y == p2.Y) return Directions.Left;
            if (p1.X > p2.X && p1.Y > p2.Y) return Directions.UpLeft;
            if (p1.X == p2.X && p1.Y == p2.Y) return Directions.TheSame;
            throw new InvalidDataException($"Direction could not be determined ({p1.X}, {p1.Y}) and ({p2.X}, {p2.Y}");
        }
        public static Position MoveInDirection(this Position position, Directions direction, int distance)
        {
            Position newPosition;
            switch (direction)
            {
                case Directions.Up:
                    newPosition = new Position(position.X, position.Y - distance);
                    break;
                case Directions.UpRight:
                    newPosition = new Position(position.X + distance, position.Y - distance);
                    break;
                case Directions.Right:
                    newPosition = new Position(position.X + distance, position.Y);
                    break;
                case Directions.DownRight:
                    newPosition = new Position(position.X + distance, position.Y + distance);
                    break;
                case Directions.Down:
                    newPosition = new Position(position.X, position.Y + distance);
                    break;
                case Directions.DownLeft:
                    newPosition = new Position(position.X - distance, position.Y + distance);
                    break;
                case Directions.Left:
                    newPosition = new Position(position.X - distance, position.Y);
                    break;
                case Directions.UpLeft:
                    newPosition = new Position(position.X - distance, position.Y - distance);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
            return newPosition;
        }

        public static bool IsPositionInSight(this Position position, Position queriedPosition, int sight)
        {
            if (queriedPosition.X >= position.X - sight && queriedPosition.X <= position.X + sight &&
                queriedPosition.Y >= position.Y - sight && queriedPosition.Y <= position.Y + sight)
                return true;
            return false;
        }

        public static Position GetRandomPosition(int maxX, int maxY)
        {
            return new Position(RandomNumberGenerator.GetInt32(0, maxX), RandomNumberGenerator.GetInt32(0, maxY));
        }
    }
}
