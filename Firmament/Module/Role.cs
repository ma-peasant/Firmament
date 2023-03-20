using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Firmament.Module
{
    //游戏角色
    public class Role :Image
    {
        public List<Bullet> bullets = new List<Bullet>();
        private bool isShort = false;
        private Canvas _canvas;
        public Role(Canvas canvas) {
            //Image image = new Image();
            this.Source = new BitmapImage(new Uri("./Images/plan1.png", UriKind.Relative));
            this.Width = 30;
            this.Height = 30;

            canvas.Children.Add(this);
            this._canvas = canvas;
        }
        public Rect getRec()
        {
            return new Rect(Canvas.GetLeft(this), Canvas.GetTop(this), 30, 30);

        }

        public void Shoot() {
            Thread.Sleep(16);
            isShort = true;
            Task task =  new TaskFactory().StartNew(() => {
                while (isShort)
                {
                    Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                    {
                        Bullet bullet = new Bullet(_canvas, this);
                        bullets.Add(bullet);
                    });
                    Thread.Sleep(20);
                }
            });
            
        }
        public void StopShoot()
        {
            isShort = false ;
        }
    }
}
