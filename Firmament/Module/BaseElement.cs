using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Firmament.Module
{
   public class BaseElement : IBaseElement, INotifyPropertyChanged
    {
        private double _x;
        public double X
        {
            get { return _x; }
            set
            {
                if (_x != value)
                {
                    _x = value;
                    OnPropertyChanged(nameof(X));
                }
            }
        }

        private double _y;
        public double Y
        {
            get { return _y; }
            set
            {
                if (_y != value)
                {
                    _y = value;
                    OnPropertyChanged(nameof(Y));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        //public double X { get; set; }
        //public double Y { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double XSpeed { get; set; }
        public double YSpeed { get; set; }
        public virtual bool HitState { get; set; }
        public int Flag { get; set; }
        public int Tag { get; set; }

        public Image image { get; set; }
    }
}
