using Firmament.Utils;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
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
                this.image = new System.Windows.Controls.Image() {
                    Width = 6,
                    Height = 10,
                    Source = new BitmapImage(new Uri("./Images/bullet.png", UriKind.Relative)),
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
            this.Width = 6;
            this.Height = 10;
            this.YSpeed = 2;
            this.Tag = 2;
            this.X = role.X + 13;
            this.Y = role.Y;
            Start();
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

        private async void Start() {
            while (!HitState) {
                await Task.Delay(32); // 每秒 60 帧
                UpdateBulletPositionsAsync();
            }
        }

        public void UpdateBulletPositionsAsync()
        {
            this.Y -= this.YSpeed;

            if (this.X + this.Width / 2 > Common.mainPage.canvas.ActualWidth ||
                this.X - this.Width / 2 < 0 ||
                this.Y + this.Height / 2 > Common.mainPage.canvas.ActualHeight ||
                this.Y - this.Height / 2 < 0)
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
