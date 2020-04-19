using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Evolution.Game.Model.Positions
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
}
