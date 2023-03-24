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
   public  class Bullet : Shape
    {
        private int speed = 10;
        //private Canvas _canvas;

        protected override Geometry DefiningGeometry { get; }

        public Bullet(Canvas canvas,Role role)
        {
            this.Fill = new SolidColorBrush(Colors.Red);
            this.Width = 2;
            this.Height = 5;
            Canvas.SetLeft(this, Canvas.GetLeft(role) + 13);
            Canvas.SetTop(this, Canvas.GetTop(role));
            canvas.Children.Add(this);
            //_canvas = canvas;
            Move(role);
        }

        public void Move(Role role) {
            DoubleAnimation daY = new DoubleAnimation();
            daY.From = Canvas.GetTop(role);
            daY.To = 0;
            double second  = Canvas.GetTop(role) / 100;

            daY.Duration = new System.Windows.Duration(TimeSpan.FromSeconds(second));
            this.BeginAnimation(Canvas.TopProperty,daY);
        }
        public Rect getRec()
        {
            return new Rect(Canvas.GetLeft(this), Canvas.GetTop(this), 2, 5);

        }
    }
}
