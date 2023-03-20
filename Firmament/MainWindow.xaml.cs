using Firmament.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Firmament
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Plan> plans = new List<Plan>();
   
        Role role = null;
        double left = 0;
        double top = 0;

        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
            this.KeyDown += MainWindow_KeyDown;
            this.KeyUp += MainWindow_KeyUp;

        }

        private void MainWindow_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.S:
                    role.StopShoot();
                    break;
            }
            //        Thread thread = new Thread(() =>
            //{
            //    Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
            //    {

            //        }
            //    });
            //});
            //thread.Start();
            e.Handled = true;
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {

            switch (e.Key)
            {
                case Key.Left:
                    left = left - 10;
                    if (left <= 0)
                    {
                        left = 0;
                    }
                    else if (left >= canvas.ActualWidth - 30)
                    {

                        left = canvas.ActualWidth - 30;
                    }
                    Canvas.SetLeft(role, left);
                    break;
                case Key.Right:
                    left = left + 10;
                    if (left <= 0)
                    {
                        left = 0;
                    }
                    else if (left >= canvas.ActualWidth - 30)
                    {
                        left = canvas.ActualWidth - 30;
                    }
                    Canvas.SetLeft(role, left);
                    break;
                case Key.Up:

                    top = top - 10;
                    if (top <= 0)
                    {
                        top = 0;
                    }
                    else if (top >= canvas.ActualHeight - 30)
                    {
                        top = canvas.ActualHeight - 30;
                    }
                    Canvas.SetTop(role, top);
                    break;
                case Key.Down:
                    top = top + 10;
                    if (top <= 0)
                    {
                        top = 0;
                    }
                    else if (top >= canvas.ActualHeight - 30)
                    {
                        top = canvas.ActualHeight - 30;
                    }
                    Canvas.SetTop(role, top);
                    break;
                case Key.S:
                    role.Shoot();
                    break;
            }
            //        Thread thread = new Thread(() =>
            //{
            //    Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
            //    {
                   
            //        }
            //    });
            //});
            //thread.Start();
            e.Handled = true;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {


        }


        private void btn_start_Click(object sender, RoutedEventArgs e)
        {
            //new TaskFactory().StartNew(ControlMove)
            //canvas = new Canvas();
            Plan plan = new Plan(this.canvas);
            plans.Add(plan);
            plan.Show();


            role = new Role(this.canvas);
            left = canvas.ActualWidth / 2;
            top = canvas.ActualHeight - 50;

            Canvas.SetLeft(role, left);
            Canvas.SetTop(role, top);

            AdjustAll();
            AdjustBullet();
        }

        private void AdjustPositatiom(Plan plan)
        {
            // 判断两个矩形是否相交
            bool isCollision = role.getRec().IntersectsWith(plan.getRec());

            // 如果发生碰撞
            if (isCollision)
            {

                // 设置两个飞机的状态为销毁
                //role.IsDestroyed = true;
                //plan.IsDestroyed = true;

                // 在游戏中移除两个飞机
                canvas.Children.Remove(plan);
                canvas.Children.Remove(role);
                MessageBox.Show("飞机相撞,game over");
            }
        }



        private void AdjustAll()
        {

            System.Timers.Timer timer = new System.Timers.Timer(1000);
            timer.Elapsed += Timer_Elapsed;
            timer.AutoReset = true;
            timer.Start();
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            for (int i = 0; i < plans.Count; i++)
            {
                Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                {
                    AdjustPositatiom(plans[i]);
                });

            }
        }

        private void AdjustBullet()
        {

            System.Timers.Timer timer = new System.Timers.Timer(100);
            timer.Elapsed += Timer_Elapsed_Bullet;
            timer.AutoReset = true;
            timer.Start();
        }
        private void Timer_Elapsed_Bullet(object sender, System.Timers.ElapsedEventArgs e)
        {
            Parallel.ForEach(plans, plan =>
            {
                AdjustBullet(plan, role.bullets);
              
            });
            //for (int i = 0; i < plans.Count; i++)
            //{
            //    new TaskFactory().StartNew(() =>
            //    {
            //        Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
            //        {
            //            AdjustBullet(plans[i], bullets);
            //        });
            //    });
            //}
        }
        private void AdjustBullet(Plan plan ,  List<Bullet> bullets)
        {
            Parallel.ForEach(bullets, bullet =>
            {
                Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                {
                    // 判断两个矩形是否相交
                    bool isCollision = plan.getRec().IntersectsWith(bullet.getRec());

                    // 如果发生碰撞
                    if (isCollision)
                    {

                        // 设置两个飞机的状态为销毁
                        //role.IsDestroyed = true;
                        //plan.IsDestroyed = true;

                        // 在游戏中移除两个飞机
                        canvas.Children.Remove(plan);
                        bullet.Clear();
                        //MessageBox.Show("飞机相撞,game over");
                    }
                });
              
            });
        }
    }
}
