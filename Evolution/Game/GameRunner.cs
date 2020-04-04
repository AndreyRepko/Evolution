using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Evolution.Game.Model;

namespace Evolution.Game
{
    public class GameRunner : INotifyPropertyChanged
    {
        private int _maxX;
        private int _maxY;

        public ObservableCollection<IBeing> Population { get; } = new ObservableCollection<IBeing>();

        public GameRunner(int zavrs, int vegetables, int maxX, int maxY)
        {
            _maxX = maxX;
            _maxY = maxY;
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

            NotifyPropertyChanged(nameof(Population));
        }

        private Position GetNotOccupiedRandomPosition()
        {
            var pos = Position.GetRandomPosition(_maxX, _maxY);
            while (IsOccupied(pos))
            {
                pos = Position.GetRandomPosition(_maxX, _maxY);
            }

            return pos;
        }

        private bool IsOccupied(PositionReadOnly position)
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
    }
}
