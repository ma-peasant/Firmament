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
    public class Role :BaseElement
    {
        public List<Bullet> bullets = new List<Bullet>();

        private CancellationTokenSource cancellation = new CancellationTokenSource();
        public Role() {
            this.control  = new Image();
            (this.control as Image).Width = 30;
            (this.control as Image).Height = 30;
            (this.control as Image).Source = new BitmapImage(new Uri("./Images/plan1.png", UriKind.Relative));
            this.Width = 30;
            this.Height = 30;
        }
        public Rect getRec()
        {
            return new Rect(this.x, this.y, 30, 30);
        }

        public Bullet Shoot()
        {
           
            //Rectangle rectangle =  await new TaskFactory().StartNew(() =>
            //{
            //    Bullet bullet = new Bullet(this);
            //    return bullet.rectangle;
            //});
            Bullet bullet = new Bullet(this);
            bullet.tag = 3;
            return bullet;
        }

        public void StopShoot()
        {
            cancellation?.Cancel();
        }
        public override bool Hit_State
        {
            set
            {
                base.Hit_State = value;
                if (base.Hit_State)
                {
                    MessageBox.Show("游戏结束");
                }
                else
                {
                }
            }
            get
            {
                return base.Hit_State;
            }
        }
    }
}
