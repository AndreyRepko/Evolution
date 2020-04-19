using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using Evolution.Game.Model.Positions;


namespace Evolution.Game.Model
{
    public class EnergyBox : IFood, IBeing
    {
        private int _nutrition;
        private readonly Position _position;
        private bool _underAggression;

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
            _position = position;
            _nutrition = initialNutrition;
        }

        public void NotifyAboutAggressionChange(bool aggression)
        {
            _underAggression = aggression;
            NotifyPropertyChanged(nameof(IsUnderAggression));
        }

        public Position Position => _position;

        public BeingType Type => BeingType.EnergyBox;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
