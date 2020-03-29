using System.IO;
using System.Security.Cryptography;

namespace Evolution.Game.Model
{
    public class Zavr : IBeing
    {
        private int _sight;
        private readonly Position _position;

        public Zavr(Position position)
        {
            _position = position;
        }

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

        public static Zavr GetRandomZavr(int x, int y)
        {
            var zavr = new Zavr(new Position(x,y));
            zavr._sight = RandomNumberGenerator.GetInt32(1, 10);
            return zavr;
        }

        public void NextTurn(bool isNormalTurn)
        {
            throw new System.NotImplementedException();
        }
    }
}
