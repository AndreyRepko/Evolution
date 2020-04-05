using System;
using System.IO;
using System.Security.Cryptography;

namespace Evolution.Game.Model
{
    public class PositionReadOnly
    {
        protected int _x;
        protected int _y;

        public int X => _x;

        public int Y => _y;

        public override bool Equals(object? obj)
        {
            //Check for null and compare run-time types.
            if (!(obj is PositionReadOnly))
            {
                return false;
            }
            else
            {
                var pos = (PositionReadOnly) obj;
                return this.X == pos.X && this.Y == pos.Y;
            }
        }

        protected bool Equals(PositionReadOnly other)
        {
            return _x == other._x && _y == other._y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_x, _y);
        }
    }

    public class Position : PositionReadOnly
    {
        public new int X
        {
            get => _x;
            set => _x = value;
        }

        public new int Y
        {
            get => _y;
            set => _y = value;
        }

        public Position(int x, int y)
        {
            _x = x;
            _y = y;
        }

        public static Position GetRandomPosition(int maxX, int maxY)
        {
            return new Position(RandomNumberGenerator.GetInt32(0, maxX), RandomNumberGenerator.GetInt32(0, maxY));
        }

        public static Directions GetRelativePosition(PositionReadOnly p, in int x, in int y)
        {
            if (p.X == x && p.Y > y) return Directions.Up;
            if (p.X < x && p.Y < y) return Directions.UpRight;
            if (p.X < x && p.Y == y) return Directions.Right;
            if (p.X < x && p.Y > y) return Directions.DownRight;
            if (p.X == x && p.Y < y) return Directions.Down;
            if (p.X > x && p.Y < y) return Directions.DownLeft;
            if (p.X > x && p.Y == y) return Directions.Left;
            if (p.X > x && p.Y > y) return Directions.UpLeft;
            if (p.X == x && p.Y == y) return Directions.TheSame;
            throw new InvalidDataException($"Direction could not be determined ({p.X}, {p.Y}) and ({x}, {y})");
        }
    }

    /// <summary>
    /// Position of the object
    /// </summary>
    public interface IHavePosition
    {
        PositionReadOnly Position { get; }
    }
}
