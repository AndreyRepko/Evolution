using System.Windows.Navigation;

namespace Evolution.Game.Model
{
    public class Vegetable : IFood, IBeing
    {
        private int _nutrition;
        private readonly PositionReadOnly _position;
        private readonly int _increment;

        public int Nutrition => _nutrition;

        public Vegetable(PositionReadOnly position, int initialNutrition, int increment = 1)
        {
            _position = position;
            _nutrition = initialNutrition;
            _increment = increment;
        }

        public Vegetable(PositionReadOnly position) : this(position, 1)  { }

        public PositionReadOnly Position => _position;

        public BeingType Type => BeingType.Tree;

        public void NextTurn(bool isNormalTurn)
        {
            if (isNormalTurn)
                _nutrition += _increment;
            else
                _nutrition -= _increment;
        }
    }
}
