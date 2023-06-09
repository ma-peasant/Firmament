using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Firmament.Module
{
    public class Ball : BaseElement
    {
        public int col;
        public int row;
        public Ball()
        {
            control = new Rectangle();
            ((Rectangle)control).Stroke = new SolidColorBrush(Colors.White);
            this.Width = ((Rectangle)control).Width = 10;
            this.Height = ((Rectangle)control).Height = 10;
        }

        public override bool Hit_State {
            set {
                base.Hit_State = value;
                if (base.Hit_State)
                {
                    ((Rectangle)this.control).Fill = new SolidColorBrush(Colors.Red);
                }
                else {
                    ((Rectangle)this.control).Fill = new SolidColorBrush(Colors.Blue);
                }
            }
            get {
                return base.Hit_State;
            }
        }


        //public Rect getRec()
        //{
        //    return new Rect(Canvas.GetLeft(rectangle), Canvas.GetTop(rectangle), 2, 5);
        //}
    }
}
