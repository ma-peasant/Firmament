using Firmament.Module;
using Firmament.Utils;
using Firmament.Utils.QuardTree;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Firmament
{
    /// <summary>
    /// QuardTreeTestPage.xaml 的交互逻辑
    /// </summary>
    public partial class GamePage : Page
    {
       

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

        QuardTreeHelp quardTreeHelp = null;
        public GamePage()
        {
            InitializeComponent();
            this.Loaded += QuardTreeTestPage_Loaded; ;
            this.KeyDown += MainWindow_KeyDown;
            this.KeyUp += MainWindow_KeyUp;
        }

        private void QuardTreeTestPage_Loaded(object sender, RoutedEventArgs e)
        {
            InitKeyDownTimer();
            InitArmyProductTimer();
            quardTreeHelp = new QuardTreeHelp(this.canvas.ActualWidth, this.ActualHeight, 10);
        }

        private void MainWindow_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.S)
            {
                isSKeyPressed = false;
            }
            if (e.Key == Key.Left)
            {
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

        #region 定时器初始化
        private void InitKeyDownTimer()
        {
            KeyDownTimer = new System.Timers.Timer();
            KeyDownTimer.Interval = 50; // 计时器间隔为 100 毫秒
            KeyDownTimer.AutoReset = true;
            KeyDownTimer.Elapsed += KeyDownTimer_Elapsed;
        }

        private void InitArmyProductTimer()
        {
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
                plan.tag = 1;
                canvas.Children.Add(plan.control);
                plan.Show(canvas.ActualWidth, canvas.ActualHeight);
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
                        canvas.Children.Add(role.Shoot().control);
                        quardTreeHelp.InsertElement(role.Shoot());
                    }
                });
            }
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
            {
                if (role != null)
                {
                    Canvas.SetLeft(role.control, left);
                    Canvas.SetTop(role.control, top);
                }
            });

        }
        #endregion

        private void btn_start_Click(object sender, RoutedEventArgs e)
        {
            //1、创建角色
            role = new Role();
            role.tag = 0;
            canvas.Children.Add(role.control);
            quardTreeHelp.InsertElement(role);
            left = canvas.ActualWidth / 2;
            top = canvas.ActualHeight - 50;
            //2、让角色位于中间
            Canvas.SetLeft(role.control, left);
            Canvas.SetTop(role.control, top);
            //3、启动定时器
            KeyDownTimer.Start();     //按键功能扫描
            ArmyProductTimer.Start(); //生产敌机扫描
            //4、检测碰撞
            //AdjustAll();
            quardTreeHelp.InitUpdateTimer();
        }

    }
}
