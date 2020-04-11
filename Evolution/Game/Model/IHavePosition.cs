using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace Evolution.Game.Model
{
    [DebuggerDisplay("Position = {X}, {Y}")]
    public class Position : INotifyPropertyChanged
    {
        private int _x;
        private int _y;

        public int X
        {
            get => _x;
            set
            {
                if (value != _x)
                {
                    _x = value;
                    NotifyPropertyChanged(nameof(X));
                }
            }
        }

        public int Y
        {
            get => _y;
            set
            {
                if (value != _y)
                {
                    _y = value;
                    NotifyPropertyChanged(nameof(Y));
                }
            }
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

        public static Directions GetRelativePosition(Position p, in int x, in int y)
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;

            //Check for null and compare run-time types.
            if (!(obj is Position))
            {
                return false;
            }
            else
            {
                var pos = (Position)obj;
                return this.X == pos.X && this.Y == pos.Y;
            }
        }

        protected bool Equals(Position other)
        {
            return _x == other._x && _y == other._y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_x, _y);
        }

    }

    /// <summary>
    /// Position of the object
    /// </summary>
    public interface IHavePosition
    {
        Position Position { get; }
    }
}
