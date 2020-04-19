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
