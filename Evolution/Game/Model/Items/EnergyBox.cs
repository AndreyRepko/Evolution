using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using Evolution.Game.Model.Positions;

namespace Evolution.Game.Model.Items
{
    public class EnergyBox : IFood, IBeing, IVictim
    {
        private int _nutrition;
        private bool _underAggression;
        private readonly Position _weakPosition;

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

        public EnergyBox(Position position, int initialNutrition)
        {
            _weakPosition = position;
            _nutrition = initialNutrition + RandomNumberGenerator.GetInt32(1, 21) - 10;
        }

        public void NotifyAboutAggressionChange(bool aggression)
        {
            _underAggression = aggression;
            NotifyPropertyChanged(nameof(IsUnderAggression));
        }

        public Position WeakPosition => _weakPosition;

        public BeingType Type => BeingType.EnergyBox;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
