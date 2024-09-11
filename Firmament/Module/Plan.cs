using Firmament.Utils;
using System;
using System.Threading;
using System.Timers;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Firmament.Module
{
    public class Plan : BaseElement
    {
        private System.Timers.Timer timer = null;
        double interval = 500;
        double maxWidth, maxHeight;
        public Plan()
        {
            this.Width = 30;
            this.Height = 30;
            this.Source = new BitmapImage(new Uri("./Images/plan1.png", UriKind.Relative));
            this.YSpeed = 5;
            this.Tag = "1";
            this.X = new Random().Next(10, 400);
            this.Y = 35;
            this.maxWidth = Common.mainPage.canvas.ActualWidth;
            this.maxHeight = Common.mainPage.canvas.ActualHeight;
        }
        private void updateBallMove()
        {
            bool isOut = false;
            this.Y = this.Y + this.YSpeed;

            //边缘检测 达到边缘后速度取反
            if (this.X + this.Width / 2 > maxWidth || this.X - this.Width / 2 < 0 ||
             this.Y + this.Height / 2 > maxHeight || this.Y - this.Height / 2 < 0)
            {
                isOut = true;
            }

            if (isOut)
            {
                timer.Stop();
                timer.Dispose();
                //在UI和四叉树上移除该对象
                Common.ballList.Remove(this);
                Common.mainPage.canvas.Children.Remove(this);
            }
            else {
                Canvas.SetLeft(this, this.X);
                Canvas.SetTop(this, this.Y);
            }
           
        }
        public void InitUpdateTimer()
        {
            timer = new System.Timers.Timer();
            timer.Interval = interval; // 更新间隔，可以根据需要调整   大概就是30帧
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

        public override bool HitState
        {
            set
            {
                base.HitState = value;
                if (value)
                {
                    Common.mainPage.canvas.Dispatcher.BeginInvoke(DispatcherPriority.Render, (ThreadStart)delegate
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
