using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Threading;
using Evolution.Game.Model;
using Evolution.Game.Model.Items;
using Evolution.Game.Model.Positions;
using Evolution.Game.Model.WorldInteraction;

namespace Evolution.Game
{
    public class GameRunner : INotifyPropertyChanged
    {
        private WorldInformation _worldInformation;
        private Dictionary<object, object> _aggressionList = new Dictionary<object, object>();
        private int _day;

        public int MaxX { get; }
        public int MaxY { get; }
        public int EnergyBoxNutrition { get; set; }
        public ObservableCollection<IBeing> Population2 { get; private set; } = new ObservableCollection<IBeing>();

        private List<Zavr> _zavrs = new List<Zavr>();
        private List<EnergyBox> _energyBoxes = new List<EnergyBox>();
        private List<Vegetable> _vegetables = new List<Vegetable>();

        private IBeing[][] _beings;
        private Dictionary<IBeing, Position> _beingPositions = new Dictionary<IBeing, Position>();

        public IReadOnlyList<Zavr> Zavrs => _zavrs;

        public int Day
        {
            get { return _day; }
            set
            {
                if (_day != value)
                {
                    _day = value;
                    NotifyPropertyChanged(nameof(Day));
                }
            }
        }

        public GameRunner(int zavrs, int vegetables, int energyBox, int maxX, int maxY, int energyBoxNutrition)
        {
            MaxX = maxX;
            MaxY = maxY;
            _beings = new IBeing[MaxX][];
            for (var i = 0; i < MaxX; i++)
            {
                _beings[i] = new IBeing[MaxY];
            }

            EnergyBoxNutrition = energyBoxNutrition;
            _worldInformation = new WorldInformation(this);

            for (var i = 0; i < zavrs; i++)
            {
                AddZavrAtRandomPosition(Zavr.GetRandomZavr(_worldInformation));
            }

            for (var i = 0; i < vegetables; i++)
            {
                AddVegetableAtRandomPosition();
            }

            for (var i = 0; i < energyBox; i++)
            {
                AddEnergyBoxAtRandomPosition();
            }

            Day = 1;

            Repopulate();

            Statistic = new GameStatistic(this);
        }

        public GameStatistic Statistic { get; set; }

        private void Repopulate()
        {
            Population2 = new ObservableCollection<IBeing>();
            foreach (var zavr in _zavrs)
            {
                Population2.Add(zavr);
            }

            foreach (var vegetable in _vegetables)
            {
                Population2.Add(vegetable);
            }

            foreach (var energyBox in _energyBoxes)
            {
                Population2.Add(energyBox);
            }
        }

        private void AddEnergyBoxAtRandomPosition()
        {
            var pos = GetNotOccupiedRandomPosition();
            AddEnergyBox(pos, new EnergyBox(pos, EnergyBoxNutrition));
        }

        private void AddEnergyBox(Position pos, EnergyBox energyBox)
        {
            _energyBoxes.Add(energyBox);
            _beings[pos.X][pos.Y] = energyBox;
            _beingPositions[energyBox] = pos;
        }

        private void AddVegetableAtRandomPosition()
        {
            var pos = GetNotOccupiedRandomPosition();
            AddVegetable(pos, new Vegetable(_worldInformation));
        }

        private void AddVegetable(Position pos, Vegetable vegetable)
        {
            _vegetables.Add(vegetable);
            _beings[pos.X][pos.Y] = vegetable;
            _beingPositions[vegetable] = pos;
        }

        private void AddZavrAtRandomPosition(Zavr zavr)
        {
            var pos = GetNotOccupiedRandomPosition();
            AddZavr(pos, zavr);
        }

        private void AddZavr(Position pos, Zavr zavr)
        {
            _zavrs.Add(zavr);
            _beings[pos.X][pos.Y] = zavr;
            _beingPositions[zavr] = pos;
        }

        private Position GetNotOccupiedRandomPosition()
        {
            if (_beingPositions.Count == MaxX * MaxY)
            {
                throw new Exception("Field is full");
            }

            if (_beingPositions.Count < (MaxX * MaxY) / 2) //If there is almost empty field then looking random will work well
            {
                var pos = PositionExtensions.GetRandomPosition(MaxX, MaxY);
                while (IsOccupied(pos))
                {
                    pos = PositionExtensions.GetRandomPosition(MaxX, MaxY);
                }

                return pos;
            }
            else //if field is almost full - it does not make sense to try to find empty slot randomly...
            {
                var movement = RandomNumberGenerator.GetInt32(0, MaxX * MaxY - _beingPositions.Count);

                for (var x = 0; x < MaxX; x++)
                for (var y = 0; y < MaxY; y++)
                {
                    if (_beings[x][y] == null)
                    {
                        if (movement ==0)
                            return new Position(x,y);
                        movement--;
                    }
                }
            }
            throw new Exception("Can't find position");
        }

        private bool IsOccupied(Position position)
        {
            return _beings[position.X][position.Y] != null;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void NextTurn(int count)
        {
            for (int i = 0; i < count; i++)
            {
                if (IsFieldIsAlmostFull())
                    break;
                DoNextTurn();
                Statistic.Gather();
                if (IsFieldIsAlmostFull())
                    break;
            }

            SanityCheck();

            Repopulate();
        }

        private bool IsFieldIsAlmostFull()
        {
            if (_beingPositions.Count > MaxX * MaxY -50)
                return true;
            return false;
        }

        private void SanityCheck()
        {
            foreach (var zavr in _zavrs)
            {
                var pos = _beingPositions[zavr];
                if (_beings[pos.X][pos.Y] != zavr)
                    throw new AbandonedMutexException("zavr :(");
            }

            foreach (var veg in _vegetables)
            {
                var pos = _beingPositions[veg];
                if (_beings[pos.X][pos.Y] != veg)
                    throw new AbandonedMutexException("veg :(");
            }

            foreach (var energyBox in _energyBoxes)
            {
                var pos = _beingPositions[energyBox];
                if (_beings[pos.X][pos.Y] != energyBox)
                    throw new AbandonedMutexException("energyBox :(");
            }

            for (int x =0 ; x < MaxX;x++)
            for (int y = 0; y < MaxY; y++)
            {
                if (_beings[x][y] != null)
                    if (_beings[x][y] is Zavr zavr)
                        if (!zavr.State)
                            throw new AbandonedMutexException("dead zavr found");
            }

            for (int x = 0; x < MaxX; x++)
            for (int y = 0; y < MaxY; y++)
            {
                if (_beings[x][y] != null)
                {
                    for (int x1 = 0; x1 < MaxX; x1++)
                    for (int y1 = 0; y1 < MaxY; y1++)
                    {
                        if (_beings[x][y] == _beings[x1][y1] && (x != x1) && (y != y1))
                            throw new AbandonedMutexException("cloning is forbidden");
                    }
                }
            }
        }

        private void DoNextTurn()
        {
            foreach (var zavr in _zavrs.Where(z => z.State).ToList())
            {
                zavr.NextTurn(true);
            }

            foreach (var vegetable in _vegetables.ToList())
            {
                vegetable.NextTurn(true);
            }

            if (Day % 10 == 0)
            {
                SpawnEnergyBoxes();
            }

            Day++;
        }

        private void SpawnEnergyBoxes()
        {
            for (var i = 0; i < 50; i++)
            {
                AddEnergyBoxAtRandomPosition();
            }
        }

        public void Remove(Zavr zavr)
        {
            var position = _beingPositions[zavr];
            _beings[position.X][position.Y] = null;
            _zavrs.Remove(zavr);
            _beingPositions.Remove(zavr);
        }

        public void AddAggression(object aggressor, object victim)
        {
            if (_aggressionList.ContainsKey(aggressor) && _aggressionList[aggressor] != victim)
            {
                if (_aggressionList[aggressor] is Vegetable vegetable)
                    vegetable.NotifyAboutAggressionChange(false);
            }

            _aggressionList[aggressor] = victim;
            if (victim is Vegetable vegetable2)
                vegetable2.NotifyAboutAggressionChange(true);
        }

        internal void AddNewZavr(Position newPosition, int speed, int sight)
        {
            if (!IsOccupied(newPosition))
            {
                AddZavr(newPosition, new Zavr(1, true, 2500, speed, Directions.Up, 0, sight, _worldInformation));
            }
        }

        internal void AddNewVegetable(Position newPosition)
        {
            if (!IsOccupied(newPosition))
            {
                AddVegetable(newPosition, new Vegetable(_worldInformation));
            }
        }

        public Position GetBeingPosition(IBeing being)
        {
            return _beingPositions[being];
        }

        public IEnumerable<(IBeing Being, Position Position)> GetPopulation(int minX, int maxX, int minY, int maxY)
        {
            for (var x = minX; x <= maxX; x++)
            for (var y = minY; y <= maxY; y++)
            {
                var being = _beings[x][y];
                if (being != null)
                {
                    yield return (being, _beingPositions[being]);
                }
            }
        }

        public void EatFoodByZavr(IBeing food, IBeing zavr)
        {
            var position = _beingPositions[food];
            var zavrPosition = _beingPositions[zavr];
            if (food is EnergyBox box)
                _energyBoxes.Remove(box);
            if (food is Vegetable veg)
                _vegetables.Remove(veg);
            _beings[position.X][position.Y] = zavr;
            _beings[zavrPosition.X][zavrPosition.Y] = null;
            _beingPositions.Remove(food);
            _beingPositions[zavr] = position;
        }

        public void TryToMoveZavr(Zavr zavr, Position newPosition)
        {
            if (!IsOccupied(newPosition))
            {
                var oldPosition = _beingPositions[zavr];
                _beings[oldPosition.X][oldPosition.Y] = null;
                _beingPositions[zavr] = newPosition;
                _beings[newPosition.X][newPosition.Y] = zavr;
            }
            else
            {
                //Debug.WriteLine("Occupied");
            }
        }
    }
}
