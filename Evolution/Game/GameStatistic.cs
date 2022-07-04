using System.Collections.Generic;
using System.Linq;

namespace Evolution.Game
{
    public class GameStatistic
    {
        private readonly GameRunner _game;
        private readonly Dictionary<int, double> _averageSpeed;
        private readonly Dictionary<int, double> _averageSight;
        private readonly Dictionary<int, double> _ZavrsCount;

        public IReadOnlyDictionary<int, double> AverageSpeedByDays => _averageSpeed;
        public IReadOnlyDictionary<int, double> AverageSightByDays => _averageSight;
        public IReadOnlyDictionary<int, double> ZavrCountByDays => _ZavrsCount;

        public GameStatistic(GameRunner game)
        {
            _game = game;
            _averageSpeed = new Dictionary<int, double>();
            _averageSight = new Dictionary<int, double>();
            _ZavrsCount = new Dictionary<int, double>();
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

        public int AverageGeneration
        {
            get
            {
                if (_game.Zavrs.Count == 0)
                    return 0;
                return (int)_game.Zavrs.Average(x => x.MyChilds);
            }
        }


        public void Gather()
        {
            _averageSpeed[_game.Day] = AverageSpeed;
            _averageSight[_game.Day] = AverageSight;
            _ZavrsCount[_game.Day] = ZavrsCount;
        }
    }
}
