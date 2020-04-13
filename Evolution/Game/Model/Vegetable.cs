using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace Evolution.Game.Model
{
    public class Vegetable : IFood, IBeing
    {
        private int _nutrition;
        private readonly Position _position;
        private readonly int _increment;
        private bool _underAggression;
        private int _age;

        public int Age
        {
            get => _age;
            set
            {
                if (value != _age)
                {
                    _age = value;
                    NotifyPropertyChanged(nameof(Age));
                }
            }
        }

        public int Nutrition
        {
            get => _nutrition;
            set
            {
                if (value != _nutrition)
                {
                    _nutrition = value;
                    NotifyPropertyChanged(nameof(Nutrition));
                }
            }
        }

        public bool IsUnderAggression => _underAggression;

        public Vegetable(Position position, int initialNutrition, int increment = 1)
        {
            _position = position;
            _nutrition = initialNutrition;
            _age = 1;
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
                Nutrition += _increment;
            else
                Nutrition -= _increment;

            Age++;

            if (Age > 10)
            {
              var random  = RandomNumberGenerator.GetInt32(1, 4);
                if (random == 1)
                {
                   // SpawnNewVegetable();
                }
            }

        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
