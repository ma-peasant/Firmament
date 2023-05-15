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
    public class Bullet
    {
        private int speed = 10;

        public Rectangle rectangle;

        public Bullet(Role role)
        {
            rectangle = new Rectangle();
            rectangle.Fill = new SolidColorBrush(Colors.Red);
            rectangle.Width = 2;
            rectangle.Height = 5;
            Canvas.SetLeft(rectangle, Canvas.GetLeft(role) + 13);
            Canvas.SetTop(rectangle, Canvas.GetTop(role));
            Move(role);
        }

        public void Move(Role role)
        {
            DoubleAnimation daY = new DoubleAnimation();
            daY.From = Canvas.GetTop(role);
            daY.To = 0;
            double second = 1;

            daY.Duration = new System.Windows.Duration(TimeSpan.FromSeconds(second));
            rectangle.BeginAnimation(Canvas.TopProperty, daY);
        }
        public Rect getRec()
        {
            return new Rect(Canvas.GetLeft(rectangle), Canvas.GetTop(rectangle), 2, 5);

        }
    }
}
