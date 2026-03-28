using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TO_Projekt.Models
{
    public class PheromoneViewModel : EntityViewModel
    {
        private double _intensity;
        public double Intensity { get => _intensity; set { _intensity = value; OnPropertyChanged(); } }
    }
}
