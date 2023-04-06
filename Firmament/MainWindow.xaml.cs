using Firmament.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
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
        //敌机集合
        List<Plan> plans = new List<Plan>();
   
        Role role = null;
        double left = 0;
        double top = 0;

        bool isSKeyPressed = false;

        System.Timers.Timer sKeyDownTimer = new System.Timers.Timer();
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
            this.KeyDown += MainWindow_KeyDown;
            this.KeyUp += MainWindow_KeyUp;
            CompositionTarget.Rendering += CompositionTarget_Rendering;


            sKeyDownTimer = new System.Timers.Timer();
            sKeyDownTimer.Interval = 100; // 计时器间隔为 100 毫秒
            sKeyDownTimer.AutoReset = true;
            sKeyDownTimer.Elapsed += SKeyDownTimer_Elapsed;
        }

        private void SKeyDownTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (isSKeyPressed)
            {
                Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                {
                    canvas.Children.Add(role.Shoot());
                });
            }
            else
            {
                sKeyDownTimer.Stop();
              
            }
        }

      
        private void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void MainWindow_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.S:
                    if (role != null)
                    {
                        isSKeyPressed = false;
                        sKeyDownTimer.Stop();
                        canvas.Children.Add(role.Shoot());
                    }
                    break;
            }
            e.Handled = true;
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            HandleKeyDown(e);
            e.Handled = true;
        }

        private async void HandleKeyDown(KeyEventArgs e) {

            Console.WriteLine(e.Key.ToString());
           await Task.Run(() =>
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
                        break;
                    case Key.S:

                        isSKeyPressed = true;
                        sKeyDownTimer.Start();
                       
                        break;
                }
            });
            Canvas.SetLeft(role, left);
            Canvas.SetTop(role, top);
        }

      
        // Define the method you want to trigger
        private void Shoot(object sender, ElapsedEventArgs e)
        {
            // Do something

        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
        }


        private void btn_start_Click(object sender, RoutedEventArgs e)
        {
            //new TaskFactory().StartNew(ControlMove)
            //canvas = new Canvas();
            Plan plan = new Plan();
            plans.Add(plan);
            canvas.Children.Add(plan);
            plan.Show(canvas.ActualWidth,canvas.ActualHeight);

            role = new Role();
            canvas.Children.Add(role);
            left = canvas.ActualWidth / 2;
            top = canvas.ActualHeight - 50;
            //让角色位于中间
            Canvas.SetLeft(role, left);
            Canvas.SetTop(role, top);

            AdjustAll();
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
            Task.Run(() =>
            {
                for (int i = 0; i < plans.Count; i++)
                {
                    Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                    {
                        AdjustPositatiom(plans[i]);
                    });
                }
            });
          
        }
    }
}
