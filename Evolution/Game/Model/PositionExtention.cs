using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Game.Model
{
    public static class PositionExtention
    {
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
    }
}
