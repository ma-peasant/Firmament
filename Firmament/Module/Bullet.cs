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
    public class Bullet: BaseElement
    {

        public Bullet(Role role)
        {
            control = new Rectangle();
            ((Rectangle)control).Fill = new SolidColorBrush(Colors.Red);
            (control as Rectangle).Width = 2;
            control.Height = 5;
            Canvas.SetLeft((control as Rectangle), Canvas.GetLeft(role.control) + 13);
            Canvas.SetTop((control as Rectangle), Canvas.GetTop(role.control));
            Move(role);
        }

        public void Move(Role role)
        {
            DoubleAnimation daY = new DoubleAnimation();
            daY.From = Canvas.GetTop(role.control);
            daY.To = 0;
            double second = 1;
            daY.Duration = new System.Windows.Duration(TimeSpan.FromSeconds(second));
            (control as Rectangle).BeginAnimation(Canvas.TopProperty, daY);
        }
    }
}
