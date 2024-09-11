using Firmament.Utils;
using System;
using System.Threading;
using System.Timers;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Firmament.Module
{
    /// <summary>
    ///子弹类
    /// </summary>
    public class Bullet: BaseElement
    {
        public Bullet(Role role) {

            Common.mainPage.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
            {
                this.image = new System.Windows.Controls.Image();
                this.image.Source = new BitmapImage(new Uri("./Images/bullet.png", UriKind.Relative));
                this.Width = this.image.Width = 6;
                this.Height = this.image.Height = 10;
            });
            this.YSpeed = 10;
            this.Tag = 2;
            this.X = role.X + 13;
            this.Y = role.Y;
            this.image.Stretch = System.Windows.Media.Stretch.UniformToFill;
            Canvas.SetLeft(this.image, this.X);
            Canvas.SetTop(this.image, this.Y);
           
            //InitUpdateTimer();
        }


        public override bool HitState {
            set
            {
                base.HitState = value;
                if (value)
                {
                    Common.mainPage.canvas.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate
                    {
                        Common.mainPage.canvas.Children.Remove(this.image);
                        Common.ballList.Remove(this);
                    });
                }
            }
            get
            {
                return base.HitState;
            }
        }
    }
}
