using Lab3.Simulation;
using Lab3.States;
using System.Text;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Lab3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SimulationEngine _engine;
        private DispatcherTimer _timer;
        private const double FPS = 25.0;
        private double _scaleX = 1.0;
        private double _scaleY = 1.0;

        public MainWindow()
        {
            InitializeComponent();
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1.0 / FPS);
            _timer.Tick += Timer_Tick;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitSim();
        }

        private void InitSim()
        {
            if (!double.TryParse(txtWidth.Text, out double w)) w = 50;
            if (!double.TryParse(txtHeight.Text, out double h)) h = 50;
            if (!int.TryParse(txtCount.Text, out int count)) count = 500;
            if (!double.TryParse(txtImmunity.Text, out double imm)) imm = 0.05;
            if (!double.TryParse(txtInfectedPercent.Text, out double infPerc)) infPerc = 0.00;

            _engine = new SimulationEngine(w, h);
            _engine.Init(count, imm, infPerc);
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            InitSim();
            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (_engine == null) return;

            _engine.Update(1.0 / FPS);

            Draw();
            UpdateStats();
        }

        private void UpdateStats()
        {
            if (_engine.Population.Count == 0) return;

            var stats = _engine.GetStats();
            double total = _engine.Population.Count;

            double infectedPct = (stats.Infected / total) * 100.0;
            double immunePct = (stats.Immune / total) * 100.0;

            lblStats.Text = $"Time: {_engine.CurrentTime:F2} s\n" +
                            $"Pop: {total}\n\n" +
                            $"Infected: {stats.Infected} ({infectedPct:F1}%)\n" +
                            $"Immune: {stats.Immune} ({immunePct:F1}%)\n";
        }

        private void Draw()
        {
            SimCanvas.Children.Clear();

            double canvasW = SimCanvas.ActualWidth;
            double canvasH = SimCanvas.ActualHeight;

            if (canvasW == 0 || canvasH == 0) return;

            _scaleX = canvasW / _engine.Width;
            _scaleY = canvasH / _engine.Height;

            foreach (var p in _engine.Population)
            {
                Ellipse el = new Ellipse();
                el.Width = 10;
                el.Height = 10;

                switch (p.CurrentState.GetStatus())
                {
                    case HealthStatus.Susceptible: el.Fill = Brushes.Green; break;
                    case HealthStatus.NoSymptoms: el.Fill = Brushes.Orange; break;
                    case HealthStatus.Symptoms: el.Fill = Brushes.Red; break;
                    case HealthStatus.Immune: el.Fill = Brushes.Blue; break;
                }

                double x = p.Position.X * _scaleX - (el.Width / 2);
                double y = p.Position.Y * _scaleY - (el.Height / 2);

                Canvas.SetLeft(el, x);
                Canvas.SetTop(el, y);
                SimCanvas.Children.Add(el);
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            _timer.Stop();
            var dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.Filter = "JSON Files|*.json";
            if (dialog.ShowDialog() == true)
            {
                string json = _engine.SaveToJson();
                File.WriteAllText(dialog.FileName, json);
                MessageBox.Show("Saved simulation state");
            }
            _timer.Start();
        }

        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            _timer.Stop();
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "JSON Files|*.json";
            if (dialog.ShowDialog() == true)
            {
                string json = File.ReadAllText(dialog.FileName);
                _engine.LoadFromJson(json);
                Draw();
                MessageBox.Show("Loaded simulation state");
            }
            _timer.Start();
        }
    }
}