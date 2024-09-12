using Firmament.Utils;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Firmament.Module
{
    //游戏角色
    public class Role :BaseElement
    {
        public delegate void  GameOverHandler();
        public event GameOverHandler GameOverEvent;
      
        public List<Bullet> bullets = new List<Bullet>();

        private CancellationTokenSource cancellation = new CancellationTokenSource();
        public Role() {
          this.image = new System.Windows.Controls.Image();
          this.Width =  this.image.Width = 30;
          this.Height =  this.image.Height = 30;
           this.image.Source = new BitmapImage(new Uri("./Images/plan1.png", UriKind.Relative));
           this.Tag = 0;
        }
        public Rect getRec()
        {
            return new Rect(this.X, this.Y, 30, 30);
        }

        public Bullet Shoot()
        {
            Bullet bullet = new Bullet(this);
            return bullet;
        }

        public void StopShoot()
        {
            cancellation?.Cancel();
        }
        public override bool HitState
        {
            set
            {
                base.HitState = value;
                if (value)
                {
                    Common.mainPage.canvas.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate
                    {
                        Common.mainPage.canvas.Children.Remove(this.image);
                        Common.ballList.Remove(this);
                        //发送全局通知，游戏结束
                        if (GameOverEvent != null) {
                            GameOverEvent.Invoke();
                        }
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
