using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using TO_Projekt.Adapter;
using TO_Projekt.Memento;
using TO_Projekt.Models;
using TO_Projekt.Observer;

namespace TO_Projekt.Simulation
{
    public class MainViewModel : BaseViewModel, ISimulationObserver
    {
        private DispatcherTimer _timer;
        private World _world;
        private ColonyMemento _savedState;
        private IConfigAdapter _configAdapter;

        public ObservableCollection<object> WorldEntities { get; set; } = new ObservableCollection<object>();

        private string _statusMessage = "Ready";
        public string StatusMessage { get => _statusMessage; set { _statusMessage = value; OnPropertyChanged(); } }

        private int _initialAntCount = 50;
        public int InitialAntCount { get => _initialAntCount; set { _initialAntCount = value; OnPropertyChanged(); } }

        public string AntCountText => _world != null ? $"Ants: {_world.Ants.Count}" : "-";
        public string FoodCountText => _world != null ? $"Food: {_world.FoodStore}" : "-";

        private bool _canLoadState = false;
        public bool CanLoadState { get => _canLoadState; set { _canLoadState = value; OnPropertyChanged(); } }

        public ICommand StartCommand { get; }
        public ICommand PauseCommand { get; }
        public ICommand RestartCommand { get; }
        public ICommand SaveStateCommand { get; }
        public ICommand LoadStateCommand { get; }

        public MainViewModel()
        {
            _configAdapter = new UIConfigAdapter();

            StartCommand = new RelayCommand(Start);
            PauseCommand = new RelayCommand(Pause);
            RestartCommand = new RelayCommand(Restart);
            SaveStateCommand = new RelayCommand(SaveState);
            LoadStateCommand = new RelayCommand(LoadState);

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(30);
            _timer.Tick += (s, e) => _world?.Tick();

            Restart();
            Pause();
        }

        public void OnSimulationTick()
        {
            UpdateVisuals(false);
            OnPropertyChanged(nameof(AntCountText));
            OnPropertyChanged(nameof(FoodCountText));
        }

        public void OnSimulationStateChanged(string status)
        {
            StatusMessage = status;
        }

        private void Start()
        {
            _timer.Start();
            StatusMessage = "Started";
        }

        private void Pause()
        {
            _timer.Stop();
            StatusMessage = "Paused";
        }

        private void Restart()
        {
            _timer.Stop();
            var config = _configAdapter.GetConfig(InitialAntCount, 700, 580);
            _world = new World(config);
            _world.AddObserver(this);
            UpdateVisuals(fullRefresh: true);
            StatusMessage = "Restarted";
            OnPropertyChanged(nameof(AntCountText));
            OnPropertyChanged(nameof(FoodCountText));
        }

        private void SaveState()
        {
            if (_world == null) return;
            _savedState = _world.CreateMemento();
            CanLoadState = true;
            StatusMessage = "State Saved";
        }

        private void LoadState()
        {
            if (_savedState == null || _world == null) return;
            _world.RestoreMemento(_savedState);
            UpdateVisuals(fullRefresh: true);
            StatusMessage = "State Loaded";
            OnPropertyChanged(nameof(AntCountText));
            OnPropertyChanged(nameof(FoodCountText));
        }

        private void OnTick(object sender, EventArgs e)
        {
            if (_world == null) return;

            _world.Tick();

            UpdateVisuals(fullRefresh: false);

            OnPropertyChanged(nameof(AntCountText));
            OnPropertyChanged(nameof(FoodCountText));
        }

        private void UpdateVisuals(bool fullRefresh)
        {
            if (fullRefresh)
            {
                WorldEntities.Clear();
                foreach (var obs in _world.Obstacles) WorldEntities.Add(obs);
            }
            foreach (var ant in _world.Ants)
                if (!WorldEntities.Contains(ant.ViewModel)) WorldEntities.Add(ant.ViewModel);

            var staticObjects = WorldEntities.Where(x => x is ObstacleViewModel).ToList();
            WorldEntities.Clear();
            foreach (var s in staticObjects) WorldEntities.Add(s);

            foreach (var p in _world.Pheromones) WorldEntities.Add(p);
            foreach (var f in _world.Food) WorldEntities.Add(f);
            foreach (var t in _world.Threats) WorldEntities.Add(t);
            foreach (var a in _world.Ants) WorldEntities.Add(a.ViewModel);
        }
    }
}
