using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace QuardTreeProject.Module
{
    public class Ball : IBaseElement
    {
        public Rectangle control;
        public int col;
        public int row;
        public int Speed = 10;
        private bool _hitState = false;

        public Ball()
        {
            control = new Rectangle();
            ((Rectangle)control).Stroke = new SolidColorBrush(Colors.White);
            this.Width = ((Rectangle)control).Width = 10;
            this.Height = ((Rectangle)control).Height = 10;
        }
        public double Width { get; set; }
        public double Height { get; set ; }
        public double X { get ; set; }
        public double Y { get; set; }
        public double XSpeed { get; set; }
        public double YSpeed { get; set; }
        public int Flag { get; set; }

        public bool HitState
        {
            set
            {
                _hitState = value;
                if (HitState)
                {
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Render, (ThreadStart)delegate
                    {
                        (this.control).Fill = new SolidColorBrush(Colors.Red);
                    });
                }
                else
                {
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Render, (ThreadStart)delegate
                    {
                        (this.control).Fill = new SolidColorBrush(Colors.Blue);
                    });
                }
            }
            get 
            {
                return _hitState;
            }
        }
    }
}
