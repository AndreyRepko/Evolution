using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using Evolution.Game;

namespace Evolution.Presenter
{
    public class EvolutionGameModel : INotifyPropertyChanged
    {
        private const int _boardSize = 50;
        private const int _initialZavrsCount = 40;
        private const int _initialTreesCount = 0;
        private const int _initialEnergyBoxCount = 50;

        public  EvolutionGameModel()
        {
            StartNewGame();
        }

        private RelayCommand _startNewGameCommand;
        private RelayCommand<Window> _closeWindowCommand;
        private RelayCommand _nextTurnCommand;
        private int _daysCount = 1;
        private int _energyBoxNutrition = 50;
        private string _lastTurnTime;

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
        public int ZavrsCount
        {
            get { return CurrentGame?.ZavrsCount ?? -1; }
        }

        public int AverageAge
        {
            get { return CurrentGame?.AverageAge ?? -1; }
        }

        public double AverageSpeed
        {
            get { return CurrentGame?.AverageSpeed ?? -1; }
        }

        public double AverageSight
        {
            get { return CurrentGame?.AverageSight ?? -1; }
        }

        public int AverageEnergy
        {
            get { return CurrentGame?.AverageEnergy ?? -1; }
        }
        
        public int AverageGeneration
        {
            get { return CurrentGame?.AverageGeneration ?? -1; }
        }

        public string LastTurnTime
        {
            get { return _lastTurnTime; }
            set
            {
                if (_lastTurnTime != value)
                {
                    _lastTurnTime = value;
                    NotifyPropertyChanged(nameof(LastTurnTime));
                }
            }
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
            var sw = new Stopwatch();
            sw.Start();
            if (DaysCount > 1000)
            {
                var game = GetNewGame();
                game.NextTurn(DaysCount);

                CurrentGame = game;
                NotifyPropertyChanged(nameof(CurrentGame));
            }
            else
            {
                CurrentGame.NextTurn(DaysCount);
                NotifyPropertyChanged(nameof(CurrentGame));
            }

            NotifyPropertyChanged(nameof(ZavrsCount));
            NotifyPropertyChanged(nameof(AverageAge));
            NotifyPropertyChanged(nameof(AverageEnergy));
            NotifyPropertyChanged(nameof(AverageSpeed));
            NotifyPropertyChanged(nameof(AverageSight));

            sw.Stop();
            LastTurnTime = string.Format("{0:0.00}", sw.ElapsedMilliseconds / 1000.0);
        }

        private void StartNewGame()
        {
            CurrentGame = GetNewGame();
            NotifyPropertyChanged(nameof(CurrentGame));
        }

        private GameRunner GetNewGame()
        {
            return new GameRunner(_initialZavrsCount, _initialTreesCount, _initialEnergyBoxCount, BoardSize, BoardSize, EnergyBoxNutrition);
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
