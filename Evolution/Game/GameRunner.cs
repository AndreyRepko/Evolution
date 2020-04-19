using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Accessibility;
using Evolution.Game.Model;
using Evolution.Game.Model.Positions;

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
        public ObservableCollection<IBeing> Population { get; } = new ObservableCollection<IBeing>();
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
            EnergyBoxNutrition = energyBoxNutrition;
            for (var i = 0; i < zavrs; i++)
            {
                var pos = GetNotOccupiedRandomPosition();
                Population.Add(Zavr.GetRandomZavr(pos));
            }

            for (var i = 0; i < vegetables; i++)
            {
                var pos = GetNotOccupiedRandomPosition();
                Population.Add(new Vegetable(pos));
            }

            for (var i = 0; i < energyBox; i++)
            {
                var pos = GetNotOccupiedRandomPosition();
                Population.Add(new EnergyBox(pos, EnergyBoxNutrition));
            }

            _worldInformation = new WorldInformation(this);

            Day = 0;

            NotifyPropertyChanged(nameof(Population));
        }

        private Position GetNotOccupiedRandomPosition()
        {
            var pos = PositionExtensions.GetRandomPosition(MaxX, MaxY);
            while (IsOccupied(pos))
            {
                pos = PositionExtensions.GetRandomPosition(MaxX, MaxY);
            }

            return pos;
        }

        private bool IsOccupied(Position position)
        {
            foreach (var being in Population)
            {
                if (Equals((object)position, (object)being.Position))
                    return true;
            }

            return false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void NextTurn()
        {
            foreach (var being in Population.Where(x => x is Zavr).ToList())
            {
                if ((being as Zavr).State)
                {
                    (being as Zavr).NextTurn(true, _worldInformation);
                }
            }

            foreach (var being in Population.Where(x => x is Vegetable).ToList())
            {
                (being as Vegetable).NextTurn(true, _worldInformation);
            }

            if (Day %10 == 0)
            {
                SpawnEnergyBoxes();
            }

            Day++;
        }

        private void SpawnEnergyBoxes()
        {
            for (var i = 0; i < 50; i++)
            {
                var pos = GetNotOccupiedRandomPosition();
                Population.Add(new EnergyBox(pos, EnergyBoxNutrition));
            }
        }

        internal void Remove(Position position)
        {
            var item = Population.First(x => x.Position.Equals(position));
            Population.Remove(item);
        }

        public void Remove(Vegetable food)
        {
            if (Population.Contains(food))
                Population.Remove(food);
            else
                throw new ArgumentOutOfRangeException("Item not found, sorry");
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
                Population.Add(new Zavr(newPosition, 1, true, 2500, speed, Directions.Up, sight));
            }
        }

        internal void AddNewVegetable(Position newPosition)
        {
            if (!IsOccupied(newPosition))
            {
                Population.Add(new Vegetable(newPosition));
            }
        }
    }
}
