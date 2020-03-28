using System.IO;

namespace Evolution.Game.Model
{
    public class Zavr : IHavePosition
    {
        private int _sight;
        private Position _position;

        /// <summary>
        /// Gets or sets the range of sight.
        /// It defines the range object can see at most.
        /// Must be in 1..10 range.
        /// </summary>
        public int Sight
        {
            get => _sight;
            set
            {
                if (value < 1 || value > 10 )
                    throw new InvalidDataException("Sight must be in 1..10 range");
                _sight = value;
            }
        }

        /// <summary>
        /// Position of the Zavr
        /// </summary>
        public PositionReadOnly Position => _position;

        public void ChangePosition(int x, int y)
        {
            //ToDo: implement check on speed, if we can move to the new position form the Zavr limitations
            // point of view
            _position.X = x;
            _position.Y = y;
        }
    }
}
