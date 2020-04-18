using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Evolution.Game;

namespace Evolution.Presenter
{
    public class EvolutionGameModel : INotifyPropertyChanged
    {
        private const int _boardSize = 50;
        private const int _initialZavrsCount = 20;
        private const int _initialTreesCount = 0;
        private const int _initialEnergyBoxCount = 100;

        public  EvolutionGameModel()
        {
            StartNewGame();
        }

        private RelayCommand _startNewGameCommand;
        private RelayCommand<Window> _closeWindowCommand;
        private RelayCommand _nextTurnCommand;
        private int _daysCount = 1;
        private int _energyBoxNutrition = 50;

        public int BoardSize => _boardSize;

        public GameRunner CurrentGame { get; set; }

        public object StartNewGameCommand
        {
            get
            {
                _startNewGameCommand ??= new RelayCommand(StartNewGame);
                return _startNewGameCommand;
            }
        }

        public RelayCommand<Window> CloseWindowCommand
        {
            get
            {
                _closeWindowCommand ??= new RelayCommand<Window>(CloseWindow);
                return _closeWindowCommand;
            }
        }

        public object NextTurnCommand
        {
            get
            {
                _nextTurnCommand ??= new RelayCommand(NextTurn);
                return _nextTurnCommand;
            }
        }

        public int DaysCount
        {
            get { return _daysCount; }
            set { _daysCount = value; }

        }
        public int EnergyBoxNutrition
        {
            get { return _energyBoxNutrition; }
            set 
            {
                _energyBoxNutrition = value;
                CurrentGame.EnergyBoxNutrition = _energyBoxNutrition;
            }

        }

        private void NextTurn()
        {
            for (var i = 0; i < DaysCount; i++)
            {
                CurrentGame.NextTurn();
                NotifyPropertyChanged(nameof(CurrentGame));
            }
        }

        private void StartNewGame()
        {
            CurrentGame = new GameRunner(_initialZavrsCount, _initialTreesCount, _initialEnergyBoxCount, BoardSize, BoardSize, EnergyBoxNutrition);
            NotifyPropertyChanged(nameof(CurrentGame));
        }

        private void CloseWindow(Window window)
        {
            if (window != null)
            {
                window.Close();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
