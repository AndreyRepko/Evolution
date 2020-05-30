using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Threading;
using Evolution.Game;
using Evolution.Presenter.ChartControl;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace Evolution.Presenter
{
    public class EvolutionGameModel : INotifyPropertyChanged
    {
        public EvolutionGameModel()
        {
            StatisticSeries = new ObservableCollection<LineSeries>();
            BoardSetup = new GameSetup();
            Setup = new ZavrSetup();

            StartNewGame();
        }

        private RelayCommand _startNewGameCommand;
        private RelayCommand<Window> _closeWindowCommand;
        private RelayCommand _nextTurnCommand;
        private int _daysCount = 1;
        private string _lastTurnTime;
        private RelayCommand _saveGameCommand;
        private RelayCommand _loadGameCommand;
        private ZavrSetup _setup;
        private RelayCommand _playCommand;
        private DispatcherTimer _playTimer;
        private bool _isPlaying = false;
        private string playButtonText = "Play";

        public GameSetup BoardSetup { get; set; }

        public ZavrSetup Setup
        {
            get => _setup;
            set
            {
                if (_setup != value)
                {
                    _setup = value;
                    NotifyPropertyChanged(nameof(Setup));
                }
            }
        }


        public int BoardSize => BoardSetup.BoardSize;

        public ObservableCollection<LineSeries> StatisticSeries { get; }

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
        public object PlayCommand
        {
            get
            {
                _playCommand ??= new RelayCommand(Play);
                return _playCommand;
            }
        }

        public string PlayButtonText
        {
            get => playButtonText;
            set
            {
                if (playButtonText != value)
                {
                    playButtonText = value;
                    NotifyPropertyChanged(nameof(PlayButtonText));
                }
            }
        }
        private void Play()
        {
            if (_isPlaying)
            {
                _playTimer?.Stop();
                PlayButtonText = "Play";
            }
            else
            {
                _playTimer = new DispatcherTimer();
                _playTimer.Tick += (_, __) => NextTurn();
                _playTimer.Interval = new TimeSpan(0, 0, 0, 0, 500);
                _playTimer.Start();
                PlayButtonText = "Stop";
            }
            _isPlaying = !_isPlaying;
        }

        public int DaysCount
        {
            get { return _daysCount; }
            set { _daysCount = value; }

        }

        public int ZavrsCount
        {
            get { return CurrentGame?.Statistic.ZavrsCount ?? -1; }
        }

        public int AverageAge
        {
            get { return CurrentGame?.Statistic.AverageAge ?? -1; }
        }

        public double AverageSpeed
        {
            get { return CurrentGame?.Statistic.AverageSpeed ?? -1; }
        }

        public double AverageSight
        {
            get { return CurrentGame?.Statistic.AverageSight ?? -1; }
        }

        public int AverageEnergy
        {
            get { return CurrentGame?.Statistic.AverageEnergy ?? -1; }
        }

        public int AverageGeneration
        {
            get { return CurrentGame?.Statistic.AverageGeneration ?? -1; }
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
            get { return BoardSetup.EnergyBoxNutrition; }
            set
            {
                BoardSetup.EnergyBoxNutrition = value;
                CurrentGame.EnergyBoxNutrition = BoardSetup.EnergyBoxNutrition;
            }

        }
        public bool IsLimitedStatistic
        {
            get;
            set;
        }

        public object SaveGameCommand
        {
            get
            {
                _saveGameCommand ??= new RelayCommand(SaveGame);
                return _saveGameCommand;
            }
        }

        public object LoadGameCommand
        {
            get
            {
                _loadGameCommand ??= new RelayCommand(LoadGame);
                return _loadGameCommand;
            }
        }

        private void SaveGame()
        {
            var saveFileDialog = new SaveFileDialog
            {
                DefaultExt = ".zavr",
                Filter = "Zavr files (*.zavr)|*.zavr"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                var json = JsonConvert.SerializeObject(Setup, Formatting.Indented);
                if (File.Exists(saveFileDialog.FileName))
                    File.Delete(saveFileDialog.FileName);

                File.WriteAllText(saveFileDialog.FileName, json);
            }
        }

        private void LoadGame()
        {
            var openFileDialog = new OpenFileDialog
            {
                DefaultExt = ".zavr",
                Filter = "Zavr files (*.zavr)|*.zavr"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                if (File.Exists(openFileDialog.FileName))
                {
                    var text = File.ReadAllText(openFileDialog.FileName);
                    Setup = JsonConvert.DeserializeObject<ZavrSetup>(text);
                }
            }
        }

        private void NextTurn()
        {

            var sw = new Stopwatch();
            sw.Start();
            if (DaysCount > 1000)
            {
                var game = GetNewGame();
                game.NextTurn(DaysCount, Setup);

                CurrentGame = game;
                NotifyPropertyChanged(nameof(CurrentGame));
            }
            else
            {
                CurrentGame.NextTurn(DaysCount, Setup);
                NotifyPropertyChanged(nameof(CurrentGame));
            }

            ReloadStatistic();

            NotifyPropertyChanged(nameof(ZavrsCount));
            NotifyPropertyChanged(nameof(AverageAge));
            NotifyPropertyChanged(nameof(AverageEnergy));
            NotifyPropertyChanged(nameof(AverageSpeed));
            NotifyPropertyChanged(nameof(AverageSight));

            sw.Stop();
            LastTurnTime = string.Format("{0:0.00}", sw.ElapsedMilliseconds / 1000.0);
        }

        private void ReloadStatistic()
        {
            StatisticSeries.Clear();
            int startFromDay;
            if (IsLimitedStatistic)
            {
                startFromDay = CurrentGame.Day - 500;
            }
            else
            {
                startFromDay = 1;
            }
            Func<int, bool> filter;
            if (CurrentGame.Day > 100)
            {
                filter = (x) => x % 10 == 0 && x > startFromDay;
            }
            else
            {
                filter = (_) => true;
            }


            var speedSeries = new LineSeries() { Name = "Average speed" };
            foreach (var pair in CurrentGame.Statistic.AverageSpeedByDays.Where(x => filter(x.Key)))
            {
                speedSeries.Data.Add(new DataPoint(pair.Key, pair.Value));
            }

            var sightSeries = new LineSeries() { Name = "Average sight" };
            foreach (var pair in CurrentGame.Statistic.AverageSightByDays.Where(x => filter(x.Key)))
            {
                sightSeries.Data.Add(new DataPoint(pair.Key, pair.Value));
            }

            var ZavrCountSeries = new LineSeries() { Name = "Zavr Count" };
            foreach (var pair in CurrentGame.Statistic.ZavrCountByDays.Where(x => filter(x.Key)))
            {
                ZavrCountSeries.Data.Add(new DataPoint(pair.Key, pair.Value));
            }

            StatisticSeries.Add(speedSeries);
            StatisticSeries.Add(sightSeries);
            StatisticSeries.Add(ZavrCountSeries);

            NotifyPropertyChanged(nameof(StatisticSeries));
        }

        private void StartNewGame()
        {
            CurrentGame = GetNewGame();
            NotifyPropertyChanged(nameof(BoardSize));
            NotifyPropertyChanged(nameof(CurrentGame));
        }

        private GameRunner GetNewGame()
        {
            return new GameRunner(BoardSetup.InitialZavrsCount, BoardSetup.InitialTreesCount,
                BoardSetup.InitialEnergyBoxCount, BoardSize, BoardSize,
                BoardSetup.EnergyBoxNutrition, Setup);
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
