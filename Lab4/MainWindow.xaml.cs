using Lab4.Objects;
using Lab4.State;
using Lab4.Strategy;
using Lab4.Vector;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Lab4
{
    public partial class MainWindow : Window
    {
        private SKKM _skkm;
        private DispatcherTimer _timer;
        private Random _rng = new Random();

        private double _minLat = 49.95;
        private double _maxLat = 50.16;
        private double _minLon = 19.68;
        private double _maxLon = 20.05;

        private double _nextIncidentTimer = 0;

        public MainWindow()
        {
            InitializeComponent();
            _skkm = new SKKM();

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(0.04);
            _timer.Tick += Loop;
            _timer.Start();
        }

        private void Loop(object sender, EventArgs e)
        {
            double dt = 0.04;

            _nextIncidentTimer -= dt;
            if (_nextIncidentTimer <= 0)
            {
                _skkm.GenerateRandomIncident();
                _nextIncidentTimer = 2.0 + _rng.NextDouble() * 6.0;
            }

            _skkm.Update(dt);
            Draw();
        }

        private void Draw()
        {
            MapCanvas.Children.Clear();

            double width = MapCanvas.ActualWidth;
            double height = MapCanvas.ActualHeight;

            foreach (var inc in _skkm.GetIncidents())
            {
                var pt = GeoToCanvas(inc.Position, width, height);

                Ellipse el = new Ellipse();
                el.Width = 12;
                el.Height = 12;

                if (inc.Strategy is PZStrategy) el.Fill = Brushes.OrangeRed;
                else el.Fill = Brushes.Goldenrod;

                if (inc.IsResolved)
                {
                    el.Stroke = Brushes.Black;
                    el.StrokeThickness = 2;
                }

                el.ToolTip = inc.Strategy.GetName();
                Canvas.SetLeft(el, pt.X - 6);
                Canvas.SetTop(el, pt.Y - 6);
                MapCanvas.Children.Add(el);
            }

            foreach (var unit in _skkm.GetUnits())
            {
                var pt = GeoToCanvas(unit.Position, width, height);

                Rectangle rect = new Rectangle();
                rect.Width = 14;
                rect.Height = 14;
                rect.Fill = Brushes.DarkBlue;
                rect.ToolTip = unit.Name;

                Canvas.SetLeft(rect, pt.X - 7);
                Canvas.SetTop(rect, pt.Y - 7);
                MapCanvas.Children.Add(rect);

                int freeCars = unit.GetFreeCars().Count;
                int totalCars = unit.Cars.Count;

                TextBlock tb = new TextBlock();
                tb.Text = $"{unit.Name} [{freeCars}/{totalCars}]";
                tb.FontSize = 10;
                tb.Foreground = Brushes.Black;
                tb.FontWeight = FontWeights.SemiBold;
                Canvas.SetLeft(tb, pt.X + 8);
                Canvas.SetTop(tb, pt.Y - 7);
                MapCanvas.Children.Add(tb);

                foreach (var car in unit.Cars)
                {
                    if (car.CurrentState is FreeState) continue;

                    var tPt = GeoToCanvas(car.Position, width, height);
                    Ellipse tEl = new Ellipse();
                    tEl.Width = 8;
                    tEl.Height = 8;

                    if (car.CurrentState is MovingState) tEl.Fill = Brushes.Blue;
                    else if (car.CurrentState is WaitingState) tEl.Fill = Brushes.Yellow;
                    else if (car.CurrentState is ActionState) tEl.Fill = Brushes.LightBlue;
                    else if (car.CurrentState is ReturningState) tEl.Fill = Brushes.Green;

                    tEl.ToolTip = $"Car {car.Id} ({unit.Name})";
                    Canvas.SetLeft(tEl, tPt.X - 4);
                    Canvas.SetTop(tEl, tPt.Y - 4);
                    MapCanvas.Children.Add(tEl);
                }
            }
        }

        private Point GeoToCanvas(Vector2D geoPos, double canvasW, double canvasH)
        {
            double xPct = (geoPos.X - _minLon) / (_maxLon - _minLon);
            double yPct = (geoPos.Y - _minLat) / (_maxLat - _minLat);
            yPct = 1.0 - yPct;
            return new Point(xPct * canvasW, yPct * canvasH);
        }
    }
}