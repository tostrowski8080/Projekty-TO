using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace TO_Projekt.Models
{
    public class EntityViewModel : BaseViewModel, IVisualEntity
    {
        private double _x;
        private double _y;
        private Brush _colorBrush;

        public double X { get => _x; set { _x = value; OnPropertyChanged(); } }
        public double Y { get => _y; set { _y = value; OnPropertyChanged(); } }
        public double Size { get; set; } = 5;
        public Brush ColorBrush { get => _colorBrush; set { _colorBrush = value; OnPropertyChanged(); } }
    }
}
