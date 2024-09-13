using Firmament.Utils;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Firmament.Module
{
    public class Plan : BaseElement
    {
        public Plan()
        {
            Common.mainPage.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
            {
                this.image = new System.Windows.Controls.Image
                {
                    Width = 30,
                    Height = 30,
                    Source = new BitmapImage(new Uri("./Images/plan1.png", UriKind.Relative)),
                    Stretch = System.Windows.Media.Stretch.UniformToFill,
                    DataContext = this
                };
                Binding leftBinding = new Binding("X")
                {
                    Source = this
                };
                BindingOperations.SetBinding(this.image, Canvas.LeftProperty, leftBinding);

                Binding topBinding = new Binding("Y")
                {
                    Source = this
                };
                BindingOperations.SetBinding(this.image, Canvas.TopProperty, topBinding);
            });
            this.Width = 30;
            this.Height = 30;
            this.YSpeed = 1;
            this.Tag = 1;
            this.X = new Random().Next(10, 400);
            this.Y = 35;
            Start();
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
        private async void Start()
        {
            while (!HitState)
            {
                await Task.Delay(32); // 每秒 60 帧
                UpdatePlanPositionsAsync();
            }

        }

        public void  UpdatePlanPositionsAsync()
        {
            // 异步更新子弹移动
            this.Y = this.Y + this.YSpeed;
            //边缘检测 达到边缘后速度取反
            if (this.X + this.Width / 2 > Common.mainPage.canvas.ActualWidth || this.X - this.Width / 2 < 0 ||
             this.Y + this.Height / 2 > Common.mainPage.canvas.ActualHeight || this.Y - this.Height / 2 < 0)
            {

                Common.mainPage.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                {
                    Common.ballList.Remove(this);
                    Common.mainPage.canvas.Children.Remove(this.image);
                });
            }
        }
    }
}
