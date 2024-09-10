using Firmament.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Firmament.Module
{
    /// <summary>
    ///子弹类
    /// </summary>
    public class Bullet: BaseElement
    {
        private System.Timers.Timer timer = null;
        private double maxWidth, maxHeight;
        public Bullet(Role role)
        {
            control = new Rectangle();
            ((Rectangle)control).Fill = new SolidColorBrush(Colors.Red);
            (control as Rectangle).Width = 2;
            control.Height = 5;
            this.x = role.x + 13;
            this.y = role.y;
            Canvas.SetLeft((control as Rectangle), this.x);
            Canvas.SetTop((control as Rectangle), this.y);
            this.maxWidth = Common.frmae.canvas.ActualWidth;
            this.maxHeight = Common.frmae.canvas.ActualHeight;
            this.ySpeed = 5;
            InitUpdateTimer();
        }

        private void updateBallMove()
        {
            bool isout = false;

            this.y -= this.ySpeed;
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
            else
            {
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
    }
}
