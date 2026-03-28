using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media;
using TO_Projekt.Decorator;
using TO_Projekt.Memento;
using TO_Projekt.Models;
using TO_Projekt.State;

namespace TO_Projekt.Simulation
{
    public class AntContext
    {
        public double X { get; private set; }
        public double Y { get; private set; }
        public Vector2D Direction { get; set; }
        public EntityViewModel ViewModel { get; private set; }
        public bool IsDead { get; set; } = false;

        private AntState _currentState;

        private IAntAppearance _appearance;
        private double _baseSpeed = 2.0;
        private static Random _rng = new Random();
        public bool IsCarrying => _appearance is CarrierAppearanceDecorator;

        public AntContext(double x, double y)
        {
            X = x;
            Y = y;
            double angle = _rng.NextDouble() * 2 * Math.PI;
            Direction = new Vector2D(Math.Cos(angle), Math.Sin(angle));
            ViewModel = new EntityViewModel { X = x, Y = y, Size = 5 };

            _appearance = new BasicAntAppearance();
            UpdateAppearance();

            TransitionTo(new ForagingState());
        }

        public void TransitionTo(AntState state)
        {
            _currentState = state;
            _currentState.SetContext(this);
        }

        public void Update(World world)
        {
            _currentState.Update(world);
            ViewModel.X = X;
            ViewModel.Y = Y;
        }

        public void Move(Vector2D vector, double mapWidth, double mapHeight)
        {
            double speed = _baseSpeed * _appearance.GetSpeedModifier();

            double nextX = X + vector.X * speed;
            double nextY = Y + vector.Y * speed;

            if (nextX <= 0 || nextX >= mapWidth)
            {
                Direction = new Vector2D(-Direction.X, Direction.Y);
                nextX = nextX <= 0 ? 0 : mapWidth;
            }

            if (nextY <= 0 || nextY >= mapHeight)
            {
                Direction = new Vector2D(Direction.X, -Direction.Y);
                nextY = nextY <= 0 ? 0 : mapHeight;
            }

            X = nextX;
            Y = nextY;
        }

        public void ApplyCarrierDecorator()
        {
            if (!(_appearance is CarrierAppearanceDecorator))
            {
                _appearance = new CarrierAppearanceDecorator(_appearance);
                UpdateAppearance();
            }
        }

        public void RemoveCarrierDecorator()
        {
            _appearance = new BasicAntAppearance();
            UpdateAppearance();
        }

        private void UpdateAppearance()
        {
            ViewModel.ColorBrush = _appearance.GetColor();
        }

        public AntSnapshot GetSnapshot()
        {
            return new AntSnapshot { X = X, Y = Y, IsCarrying = this.IsCarrying };
        }

        public void Restore(AntSnapshot snap)
        {
            X = snap.X;
            Y = snap.Y;
            if (snap.IsCarrying) ApplyCarrierDecorator();
            else RemoveCarrierDecorator();
            ViewModel.X = X;
            ViewModel.Y = Y;
        }
    }
}
