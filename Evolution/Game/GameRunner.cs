using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Accessibility;
using Evolution.Game.Model;

namespace Evolution.Game
{
    public class GameRunner : INotifyPropertyChanged
    {
        private WorldInformation _worldInformation;
        private Dictionary<object, object> _aggressionList = new Dictionary<object, object>();

        public int MaxX { get; }
        public int MaxY { get; }

        public ObservableCollection<IBeing> Population { get; } = new ObservableCollection<IBeing>();

        public GameRunner(int zavrs, int vegetables, int maxX, int maxY)
        {
            MaxX = maxX;
            MaxY = maxY;
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

            _worldInformation = new WorldInformation(this);

            NotifyPropertyChanged(nameof(Population));
        }

        private Position GetNotOccupiedRandomPosition()
        {
            var pos = Position.GetRandomPosition(MaxX, MaxY);
            while (IsOccupied(pos))
            {
                pos = Position.GetRandomPosition(MaxX, MaxY);
            }

            return pos;
        }

        private bool IsOccupied(Position position)
        {
            foreach (var being in Population)
            {
                if (Equals(position, being.Position))
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
                (being as Zavr).NextTurn(true, _worldInformation);
            }

            foreach (var being in Population.Where(x => x is Vegetable))
            {
                (being as Vegetable).NextTurn(true);
            }

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
    }
}
