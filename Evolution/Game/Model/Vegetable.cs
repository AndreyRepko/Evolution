using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Evolution.Game.Model
{
    public class Vegetable : IFood, IBeing
    {
        private int _nutrition;
        private readonly Position _position;
        private readonly int _increment;
        private bool _underAggression;

        public int Nutrition => _nutrition;

        public bool IsUnderAggression => _underAggression;

        public Vegetable(Position position, int initialNutrition, int increment = 1)
        {
            _position = position;
            _nutrition = initialNutrition;
            _increment = increment;
        }

        public Vegetable(Position position) : this(position, 1)  { }

        public void NotifyAboutAggressionChange(bool aggression)
        {
            _underAggression = aggression;
            NotifyPropertyChanged(nameof(IsUnderAggression));
        }

        public Position Position => _position;

        public BeingType Type => BeingType.Tree;

        public void NextTurn(bool isNormalTurn)
        {
            if (isNormalTurn)
                _nutrition += _increment;
            else
                _nutrition -= _increment;
            NotifyPropertyChanged(nameof(Nutrition));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
