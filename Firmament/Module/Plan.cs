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
        private Canvas _canvas;
        public Plan(Canvas canvas)
        {
            this.Source = new BitmapImage(new Uri("./Images/plan1.png", UriKind.Relative));
            this.Width = 30;
            this.Height = 30;
            canvas.Children.Add(this);
            this._canvas = canvas;
        }
        public void Show() {
            DoubleAnimation daX = new DoubleAnimation();
            DoubleAnimation daY = new DoubleAnimation();
            //指定起点
            daX.From = 0;
            daY.From = 0;
            //指定终点
            //Random r = new Random();
            daX.To = this._canvas.ActualWidth;
            daY.To = this._canvas.ActualHeight;
            //指定时长
            Duration duration = new Duration(TimeSpan.FromSeconds(10));
            daX.Duration = duration;
            daY.Duration = duration;
            //动画的主题是TranslateTransform变形，而非Button
            this.BeginAnimation(Canvas.LeftProperty, daX);
            this.BeginAnimation(Canvas.TopProperty, daY);
            //image.Source = null;
            //image = null;

            //Storyboard myStoryboard = new Storyboard();
            //myStoryboard.Children.Add(daX);
            //myStoryboard.Children.Add(daY);
            //Storyboard.SetTargetName(daX, image.Name);
            //Storyboard.SetTargetProperty(daX, new PropertyPath(Image.WidthProperty));

            //myStoryboard.Begin();
        }
        public Rect getRec() {
            return new Rect(Canvas.GetLeft(this),Canvas.GetTop(this),30,30);
        
        }


    }
}
