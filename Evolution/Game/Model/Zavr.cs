using System.IO;
using System.Security.Cryptography;

namespace Evolution.Game.Model
{
    public class Zavr : IBeing
    {
        private int _sight;
        private readonly Position _position;
        private int _age;
        private int _enerenergy;
        private bool _state;
        private int _speed;
        private Directions _direction;
        private BeingType _type;

        public Zavr(Position position)
        {
            _position = position;
            _age = 1;
            _state = true;
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

        public int Age
        {
            get => _age;
        }

        public int Energy
        {
            get => _enerenergy;
            set
            {
                if (value < 0 || value > 10000)
                    throw new InvalidDataException("enerenergy must be in 1..1 range");
                _enerenergy = value;
            }
        }

        public bool State
        {
            get => _state;
        }

        public int Speed
        {
            get => _speed;
            set
            {
                if (value < 1 || value > 5)
                    throw new InvalidDataException("speed must be in 5..5 range");
                _speed = value;
            }
        }

        public Directions Direction
        {
            get => _direction;
        }

        /// <summary>
        /// Position of the Zavr
        /// </summary>
        public PositionReadOnly Position => _position;

        public BeingType Type => BeingType.Zavr;

        public void ChangePosition(int x, int y)
        {
            //ToDo: implement check on speed, if we can move to the new position form the Zavr limitations
            // point of view
            _position.X = x;
            _position.Y = y;
        }

        public static Zavr GetRandomZavr(Position position)
        {
            var zavr = new Zavr(position);
            zavr._sight = RandomNumberGenerator.GetInt32(1, 10);
            zavr._enerenergy = 3000;
            zavr._speed = RandomNumberGenerator.GetInt32(1, 5);
            return zavr;
        }
        

        public void NextTurn(bool isNormalTurn)
        {
            throw new System.NotImplementedException();
        }
    }
}
