using System.ComponentModel;
using System.Runtime.CompilerServices;
using Evolution.Game;

namespace Evolution.Presenter
{
    public class EvolutionGameModel : INotifyPropertyChanged
    {
        private RelayCommand _startNewGameCommand;
        public int BoardSize => 50;

        public GameRunner CurrentGame { get; set; }

        public object StartNewGameCommand
        {
            get
            {
                _startNewGameCommand ??= new RelayCommand(StartNewGame);
                return _startNewGameCommand;
            }
        }

        private void StartNewGame()
        {
            CurrentGame = new GameRunner(10, 10, BoardSize, BoardSize);
            NotifyPropertyChanged(nameof(CurrentGame));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
