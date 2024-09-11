using Firmament.Utils;
using System;
using System.Threading;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Firmament.Module
{
    //暂时是
   public class Plan : BaseElement
    {
        private double maxWidth, maxHeight;
        private System.Timers.Timer timer = null;
        public Plan()
        {
             control = new Image();
            (control as Image).Width = 30;
            (control as Image).Height = 30;
            (control as Image).Source = new BitmapImage(new Uri("./Images/plan1.png", UriKind.Relative));
            this.Width = 30;
            this.Height = 30;
            this.ySpeed = 3;
            this.tag = 1;

            this.x = new Random().Next(10, 400);
            this.y = 30;
        }
        public void Show(double width, double height)
        {
            this.maxWidth = width;
            this.maxHeight = height;
            InitUpdateTimer();
        }
        private void updateBallMove()
        {
            bool isout = false;
            
            this.y += this.ySpeed;
            //边缘检测 达到边缘后速度取反
            if (this.x + this.Width / 2 > this.maxWidth)
            {
                isout = true;
            }
            else if (this.x - this.Width / 2 < 0)
            {
                isout = true;
            }
            if (this.y + this.Height / 2 > this.maxHeight)
            {
                isout = true;
            }
            else if (this.y - this.Height / 2 < 0)
            {
                isout = true;
            }
            if (isout)
            {
                timer.Stop();
                timer.Dispose();
                //在UI和四叉树上移除该对象
                Common.ballList.Remove(this);
                Common.frmae.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                {
                    Common.frmae.canvas.Children.Remove(this.control);
                });
            }
            else {
                Common.frmae.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                {
                    Canvas.SetLeft(this.control, this.x);
                    Canvas.SetTop(this.control, this.y);
                });
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
            updateBallMove();
        }

        public override bool Hit_State
        {
            set
            {
                base.Hit_State = value;
                if (base.Hit_State)
                {
                    Common.frmae.canvas.Dispatcher.BeginInvoke(DispatcherPriority.Render, (ThreadStart)delegate
                    {
                        Common.frmae.canvas.Children.Remove(this.control);
                        Common.ballList.Remove(this);
                    });
                }
            }
            get
            {
                return base.Hit_State;
            }
        }

      
    }
}
