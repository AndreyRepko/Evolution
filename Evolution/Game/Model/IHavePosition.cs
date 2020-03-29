using System;
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
    }

    /// <summary>
    /// Position of the object
    /// </summary>
    public interface IHavePosition
    {
        PositionReadOnly Position { get; }
    }
}
