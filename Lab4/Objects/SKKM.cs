using Lab4.Iterator;
using Lab4.Observer;
using Lab4.State;
using Lab4.Strategy;
using Lab4.Vector;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab4.Objects
{
    public class SKKM : Observer.IObserver<Incident>
    {
        private List<JRG> _units;
        private List<Incident> _incidents;
        private List<Incident> _incidentsToRemove;

        private Random _rng;

        private readonly double _minLat = 49.95855025648944;
        private readonly double _maxLat = 50.154564013341734;
        private readonly double _minLon = 19.688292482742394;
        private readonly double _maxLon = 20.02470275868903;

        public SKKM()
        {
            _rng = new Random();
            _incidents = new List<Incident>();
            _incidentsToRemove = new List<Incident>();
            _units = new List<JRG>();
            InitializeUnits();
        }

        private void InitializeUnits()
        {
            _units.Add(new JRG("JRG-1", 50.060, 19.943));
            _units.Add(new JRG("JRG-2", 50.033, 19.936));
            _units.Add(new JRG("JRG-3", 50.076, 19.887));
            _units.Add(new JRG("JRG-4", 50.038, 20.006));
            _units.Add(new JRG("JRG-5", 50.092, 19.920));
            _units.Add(new JRG("JRG-6", 50.016, 20.016));
            _units.Add(new JRG("JRG-7", 50.094, 19.977));
            _units.Add(new JRG("SA PSP", 50.067, 20.019));
            _units.Add(new JRG("JRG Skawina", 49.968, 19.800));
            _units.Add(new JRG("LSP Balice", 50.077, 19.784));
        }

        public void GenerateRandomIncident()
        {
            double lat = _minLat + (_rng.NextDouble() * (_maxLat - _minLat));
            double lon = _minLon + (_rng.NextDouble() * (_maxLon - _minLon));
            Vector2D pos = new Vector2D(lon, lat);

            IIncidentStrategy strategy;
            if (_rng.NextDouble() < 0.70) strategy = new MZStrategy();
            else strategy = new PZStrategy();

            var incident = new Incident(pos, strategy);

            incident.Attach(this);

            _incidents.Add(incident);
            Dispatch(incident);
        }

        private void Dispatch(Incident incident)
        {
            int needed = incident.Strategy.RequiredTrucks;
            int assigned = 0;

            var unitCollection = new UnitCollection(_units, incident.Position);

            foreach (var unit in unitCollection)
            {
                if (assigned >= needed) break;

                var freeTrucks = unit.GetFreeCars();
                foreach (var truck in freeTrucks)
                {
                    if (assigned >= needed) break;

                    double travelTime = _rng.NextDouble() * 3.0;
                    truck.SetState(new MovingState(unit.Position, incident, travelTime));
                    incident.AddAssignedCar(truck);
                    assigned++;
                }
            }
        }

        public void Update(double deltaTime)
        {
            foreach (var unit in _units)
            {
                foreach (var car in unit.Cars)
                {
                    car.Update(deltaTime);
                }
            }

            if (_incidentsToRemove.Count > 0)
            {
                foreach (var inc in _incidentsToRemove)
                {
                    _incidents.Remove(inc);
                }
                _incidentsToRemove.Clear();
            }
        }

        public void OnUpdate(Incident incident)
        {
            if (!_incidentsToRemove.Contains(incident))
            {
                _incidentsToRemove.Add(incident);
            }
        }

        public List<JRG> GetUnits() => _units;
        public List<Incident> GetIncidents() => _incidents;
    }
}