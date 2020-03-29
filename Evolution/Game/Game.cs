using System;
using System.Collections.Generic;
using System.Text;
using Evolution.Game.Model;

namespace Evolution.Game
{
    public class Game
    {
        private const int _maxX = 100;
        private const int _maxY = 100;

        public readonly List<IBeing> _population = new List<IBeing>();

        public Game(int zavrs, int vegetables)
        {
            for (var i = 0; i < zavrs; i++)
            {
                var pos = GetNotOccupiedRandomPosition();
                _population.Add(new Zavr(pos));
            }

            for (var i = 0; i < vegetables; i++)
            {
                var pos = GetNotOccupiedRandomPosition();
                _population.Add(new Vegetable(pos));
            }
        }

        private Position GetNotOccupiedRandomPosition()
        {
            var pos = Position.GetRandomPosition(_maxX, _maxY);
            while (!IsNotOccupied(pos))
            {
                pos = Position.GetRandomPosition(_maxX, _maxY);
            }

            return pos;
        }

        private bool IsNotOccupied(PositionReadOnly position)
        {
            foreach (var being in _population)
            {
                if (Equals(position, being.Position))
                    return true;
            }

            return false;
        }
    }
}
