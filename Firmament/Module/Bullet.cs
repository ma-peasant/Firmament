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
        private System.Timers.Timer timer = null;
        public Bullet(Role role) { 
            this.Source = new BitmapImage(new Uri("./Images/bullet.png", UriKind.Relative));
            this.Width = 6;
            this.Height = 10;
            this.YSpeed = 5;
            this.Tag = 2;
            this.X = role.X + 13;
            this.Y = role.Y;
            this.Stretch = System.Windows.Media.Stretch.UniformToFill;
            Canvas.SetLeft(this, this.X);
            Canvas.SetTop(this, this.Y);
           
            InitUpdateTimer();
        }

        private void updateBallMove()
        {
            bool isout = false;

            this.Y -= this.YSpeed;
            //边缘检测 达到边缘后速度取反
            if (this.X + this.Width / 2 > Common.mainPage.canvas.ActualWidth)
            {
                isout = true;
            }
            else if (this.X - this.Width / 2 < 0)
            {
                isout = true;
            }
            if (this.Y + this.Height / 2 > Common.mainPage.canvas.ActualHeight)
            {
                isout = true;
            }
            else if (this.Y - this.Height / 2 < 0)
            {
                isout = true;
            }
            if (isout)
            {
                timer.Stop();
                timer.Dispose();
                //在UI和四叉树上移除该对象
                Common.ballList.Remove(this);
                Common.mainPage.canvas.Children.Remove(this);
            }
            else
            {
                Canvas.SetLeft(this, this.X);
                Canvas.SetTop(this, this.Y);
            }

        }
        private void InitUpdateTimer()
        {
            timer = new System.Timers.Timer();
            timer.Interval = 100; // 更新间隔，可以根据需要调整   大概就是30帧
            timer.Elapsed += Timer_Tick; ;
            timer.Start();
        }

        private void Timer_Tick(object sender, ElapsedEventArgs e)
        {
            Common.mainPage.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
            {
                updateBallMove();
            });
           
        }

        public override bool HitState {
            set
            {
                base.HitState = value;
                if (value)
                {
                    Common.mainPage.canvas.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate
                    {
                        Common.mainPage.canvas.Children.Remove(this);
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
