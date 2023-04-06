using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace Firmament.Module
{
    //暂时是
   public class Plan :Image
    {
        public Plan()
        {
            this.Source = new BitmapImage(new Uri("./Images/plan1.png", UriKind.Relative));
            this.Width = 30;
            this.Height = 30;
        }
        public void Show(double x , double y) {
            DoubleAnimation daX = new DoubleAnimation();
            DoubleAnimation daY = new DoubleAnimation();
            //指定起点
            daX.From = 0;
            daY.From = 0;
            //指定终点
            //Random r = new Random();
            daX.To = x;
            daY.To = y;
            //指定时长
            Duration duration = new Duration(TimeSpan.FromSeconds(10));
            daX.Duration = duration;
            daY.Duration = duration;
            //动画的主题是TranslateTransform变形，而非Button
            this.BeginAnimation(Canvas.LeftProperty, daX);
            this.BeginAnimation(Canvas.TopProperty, daY);
            //image.Source = null;
            //image = null;
        }
        public Rect getRec() {
            return new Rect(Canvas.GetLeft(this),Canvas.GetTop(this),30,30);
        
        }


    }
}
