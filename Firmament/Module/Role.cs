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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Firmament.Module
{
    //游戏角色
    public class Role :Image
    {

        public List<Bullet> bullets = new List<Bullet>();

        private CancellationTokenSource cancellation = new CancellationTokenSource();
        public Role() {
            //Image image = new Image();
            this.Source = new BitmapImage(new Uri("./Images/plan1.png", UriKind.Relative));
            this.Width = 30;
            this.Height = 30;
        }
        public Rect getRec()
        {
            return new Rect(Canvas.GetLeft(this), Canvas.GetTop(this), 30, 30);
        }

        public Rectangle Shoot()
        {
           
            //Rectangle rectangle =  await new TaskFactory().StartNew(() =>
            //{
            //    Bullet bullet = new Bullet(this);
            //    return bullet.rectangle;
            //});
            Bullet bullet = new Bullet(this);
            return bullet.rectangle;
        }

        public void StopShoot()
        {
            cancellation?.Cancel();
        }

        //public void Shoot()
        //{
        //    Task task = new TaskFactory().StartNew(() => {
        //        while (cancellation.Token.IsCancellationRequested)
        //        {
        //            Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
        //            {
        //                Bullet bullet = new Bullet(this);
        //                bullets.Add(bullet);
        //            });
        //            Thread.Sleep(200);
        //        }
        //    });
        //}

    }
}
