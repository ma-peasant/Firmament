using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Firmament.Module
{
    /// <summary>
    ///子弹类
    /// </summary>
   public  class Bullet
    {
        private int speed = 10;


        private Canvas _canvas;
        private Rectangle _rectangle;
        public Bullet(Canvas canvas,Role role)
        {
            //< !--< Rectangle x: Name = "bullet1" Fill = "Red" Width = "10" Height = "10" Canvas.Left = "50" Canvas.Top = "50" />
            _rectangle = new Rectangle() { Fill = new SolidColorBrush(Colors.Red), Width = 2, Height = 5 };
            Canvas.SetLeft(_rectangle, Canvas.GetLeft(role) + 13);
            Canvas.SetTop(_rectangle, Canvas.GetTop(role));
            canvas.Children.Add(_rectangle);
            _canvas = canvas;
            Move(role);
        }

        public void Move(Role role) {
            DoubleAnimation daY = new DoubleAnimation();
            daY.From = Canvas.GetTop(role);
            daY.To = 0;

            double second  = Canvas.GetTop(role) / 100;

            daY.Duration = new System.Windows.Duration(TimeSpan.FromSeconds(second));
            _rectangle.BeginAnimation(Canvas.TopProperty,daY);
        }
        public Rect getRec()
        {
            return new Rect(Canvas.GetLeft(_rectangle), Canvas.GetTop(_rectangle), 2, 5);

        }
        public void Clear() {
            _canvas.Children.Remove(_rectangle);
        }
    }
}
