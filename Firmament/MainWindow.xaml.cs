using Firmament.Module;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

        //子弹集合
        List<Bullet> bullets = new List<Bullet>();


        Role role = null;
        double left = 0;
        double top = 0;

        bool isSKeyPressed = false;
        bool isLeftKeyPressed = false;
        bool isRightKeyPressed = false;
        bool isUpKeyPressed = false;
        bool isDownKeyPressed = false;
        
        //开一个定时器 ，专门处理按钮点击
        System.Timers.Timer KeyDownTimer;

        //开一个定时器 ，生产敌机
        System.Timers.Timer ArmyProductTimer;
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
            this.KeyDown += MainWindow_KeyDown;
            this.KeyUp += MainWindow_KeyUp;
        }

        private void MainWindow_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.S) {
                isSKeyPressed = false;
            }
            if (e.Key == Key.Left) {
                isLeftKeyPressed = false;
            }
            if (e.Key == Key.Right)
            {
                isRightKeyPressed = false;
            }
            if (e.Key == Key.Up)
            {
                isUpKeyPressed = false;
            }
            if (e.Key == Key.Down)
            {
                isDownKeyPressed = false;
            }
        }
        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                isLeftKeyPressed = true;
            }
            if (e.Key == Key.Right)
            {
                isRightKeyPressed = true; 
            }
            if (e.Key == Key.Up)
            {
                isUpKeyPressed = true;
            }
            if (e.Key == Key.Down)
            {
                isDownKeyPressed = true;
            }
            if (e.Key == Key.S)
            {
                isSKeyPressed = true;
            }
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            InitKeyDownTimer();
            InitArmyProductTimer();
        }
        #region 定时器初始化
        private void InitKeyDownTimer() {
            KeyDownTimer = new System.Timers.Timer();
            KeyDownTimer.Interval = 50; // 计时器间隔为 100 毫秒
            KeyDownTimer.AutoReset = true;
            KeyDownTimer.Elapsed += KeyDownTimer_Elapsed;
        }

        private void InitArmyProductTimer() {
            ArmyProductTimer = new System.Timers.Timer();
            ArmyProductTimer.Interval = 2000; // 计时器间隔为 100 毫秒
            ArmyProductTimer.AutoReset = true;
            ArmyProductTimer.Elapsed += ArmyProductTimer_Elapsed;
        }
        /// <summary>
        /// 生产敌机
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ArmyProductTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
            {
                Plan plan = new Plan();
                plans.Add(plan);
                canvas.Children.Add(plan);
                plan.Show(canvas.ActualWidth, canvas.ActualHeight);
                plans.Add(plan);
            });
        }

        /// <summary>
        /// 按键处理程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KeyDownTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (isLeftKeyPressed)
            {
                left = left - 10;
                if (left <= 0)
                {
                    left = 0;
                }
                else if (left >= canvas.ActualWidth - 30)
                {
                    left = canvas.ActualWidth - 30;
                }
            }
            if (isRightKeyPressed)
            {
                isRightKeyPressed = true;
                left = left + 10;
                if (left <= 0)
                {
                    left = 0;
                }
                else if (left >= canvas.ActualWidth - 30)
                {
                    left = canvas.ActualWidth - 30;
                }
            }
            if (isUpKeyPressed)
            {
                isUpKeyPressed = true;
                top = top - 10;
                if (top <= 0)
                {
                    top = 0;
                }
                else if (top >= canvas.ActualHeight - 30)
                {
                    top = canvas.ActualHeight - 30;
                }
            }
            if (isDownKeyPressed)
            {
                isDownKeyPressed = true;
                top = top + 10;
                if (top <= 0)
                {
                    top = 0;
                }
                else if (top >= canvas.ActualHeight - 30)
                {
                    top = canvas.ActualHeight - 30;
                }
            }
            if (isSKeyPressed)
            {
                Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                {
                    if (role != null)
                    {

                        canvas.Children.Add(role.Shoot());
                    }
                });
            }
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
            {
                if (role != null)
                {
                    Canvas.SetLeft(role, left);
                    Canvas.SetTop(role, top);
                }
            });

        }
        #endregion



        private void btn_start_Click(object sender, RoutedEventArgs e)
        {
           
            role = new Role();
            canvas.Children.Add(role);
            left = canvas.ActualWidth / 2;
            top = canvas.ActualHeight - 50;
            //让角色位于中间
            Canvas.SetLeft(role, left);
            Canvas.SetTop(role, top);
            AdjustAll();

            KeyDownTimer.Start();
            ArmyProductTimer.Start();
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

            if (plan.IsOver(bullets)) {
                canvas.Children.Remove(plan);
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
