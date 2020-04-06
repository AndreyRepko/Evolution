using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Navigation;

namespace Evolution.Game.Model
{
    public class Vegetable : IFood, IBeing
    {
        private int _nutrition;
        private readonly Position _position;
        private readonly int _increment;

        public int Nutrition => _nutrition;

        public Vegetable(Position position, int initialNutrition, int increment = 1)
        {
            _position = position;
            _nutrition = initialNutrition;
            _increment = increment;
        }

        public Vegetable(Position position) : this(position, 1)  { }

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
