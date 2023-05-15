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
            daX.From = new Random().Next(0,400);
            daY.From = 0;
            //指定终点
            //Random r = new Random();
            daX.To = new Random().Next(0,400);
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

        public bool IsOver(List<Bullet> bullets)
        {
            for (int i = 0; i < bullets.Count; i++)
            {
                bool isCollision = bullets[i].getRec().IntersectsWith(this.getRec());
                if (isCollision)
                {
                    return true;
                }
            }
            return false;
        }


    }
}
