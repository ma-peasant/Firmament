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
        double interval = 500;
        double maxWidth, maxHeight;
        public Plan()
        {
            Common.mainPage.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
            {
                this.image = new System.Windows.Controls.Image();
                this.Width = this.image.Width = 30;
                this.Height = this.image.Height = 30;
                this.image.Source = new BitmapImage(new Uri("./Images/plan1.png", UriKind.Relative));
            });


            this.YSpeed = 5;
            this.Tag = 1;
            this.X = new Random().Next(10, 400);
            this.Y = 35;
            this.maxWidth = Common.mainPage.canvas.ActualWidth;
            this.maxHeight = Common.mainPage.canvas.ActualHeight;
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
