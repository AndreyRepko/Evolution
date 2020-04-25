using System.Collections.Generic;
using System.Linq;

namespace Evolution.Game
{
    public class GameStatistic
    {
        private readonly GameRunner _game;
        private readonly Dictionary<int, double> _averageSpeed;

        public IReadOnlyDictionary<int, double> AverageSpeedByDays => _averageSpeed;

        public GameStatistic(GameRunner game)
        {
            _game = game;
            _averageSpeed = new Dictionary<int, double>();
        }

        public int ZavrsCount
        {
            get
            {
                return _game.Zavrs.Count;
            }
        }

        public int AverageAge
        {
            get
            {
                if (_game.Zavrs.Count == 0)
                    return 0;
                return (int)_game.Zavrs.Average(x => x.Age);
            }
        }

        public double AverageSpeed
        {
            get
            {
                if (_game.Zavrs.Count == 0)
                    return 0;
                return 1.0 * _game.Zavrs.Sum(x => x.Speed) / _game.Zavrs.Count;
            }
        }

        public double AverageSight
        {
            get
            {
                if (_game.Zavrs.Count == 0)
                    return 0;

                return 1.0 * _game.Zavrs.Sum(x => x.Sight) / _game.Zavrs.Count;
            }
        }

        public int AverageEnergy
        {
            get
            {
                if (_game.Zavrs.Count == 0)
                    return 0;
                return (int)_game.Zavrs.Average(x => x.Energy);
            }
        }

        public void Gather()
        {
            _averageSpeed[_game.Day] = AverageSpeed;
        }
    }
}
