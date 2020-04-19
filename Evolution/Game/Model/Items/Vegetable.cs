using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using Evolution.Game.Model.Positions;
using Evolution.Game.Model.WorldInteraction;

namespace Evolution.Game.Model.Items
{
    public class Vegetable : IFood, IBeing
    {
        private readonly IVegetableWorldInteraction _world;
        private int _nutrition;
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

        public Vegetable(IVegetableWorldInteraction world, int initialNutrition, int increment = 1)
        {
            _world = world;
            _nutrition = initialNutrition;
            _age = 1;
            _increment = increment;
        }

        public Vegetable(IVegetableWorldInteraction world) : this(world, 1)  { }

        public void NotifyAboutAggressionChange(bool aggression)
        {
            _underAggression = aggression;
            NotifyPropertyChanged(nameof(IsUnderAggression));
        }

        public Position WeakPosition => _world.GetPosition(this);

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
                    _world.SpawnNewVegetable(this, (Directions)RandomNumberGenerator.GetInt32(1, 9));
                    Nutrition -= 1;
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
