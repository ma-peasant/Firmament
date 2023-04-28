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
   
        Role role = null;
        double left = 0;
        double top = 0;

        bool isSKeyPressed = false;
        bool isLeftKeyPressed = false;
        bool isRightKeyPressed = false;
        bool isUpKeyPressed = false;
        bool isDownKeyPressed = false;
        
        //开一个定时器 ，专门处理按钮点击
        System.Timers.Timer KeyDownTimer = new System.Timers.Timer();
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
            //bool isuse =  IsFileInUse(new FileInfo(@"E:\智众工作文档\MMC 院内检验数据接口文档（3.6）(1).pdf"));
            // IsRegex();
            KeyDownTimer = new System.Timers.Timer();
            KeyDownTimer.Interval = 50; // 计时器间隔为 100 毫秒
            KeyDownTimer.AutoReset = true;
            KeyDownTimer.Elapsed += KeyDownTimer_Elapsed; ;
        }

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
                    if (role != null) { 
                        canvas.Children.Add(role.Shoot());
                    }
                });
            }
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
            {
                if (role != null) {
                    Canvas.SetLeft(role, left);
                    Canvas.SetTop(role, top);
                }
            });
           
        }


        // Define a method to check if a file is in use
        private  bool IsFileInUse(FileInfo file)
        {
            try
            {
                using (FileStream fileStream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    // The file is not currently in use
                    return false;
                }
            }
            catch (IOException)
            {
                // The file is currently in use
                return true;
            }
        }

        private void IsRegex() {
            string pattern = @"\.(jpe?g|bmp|png)$";
            string fileName = "example_image.PNG";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            if (regex.IsMatch(fileName))
            {
                Console.WriteLine("Match found!");
            }
            else
            {
                Console.WriteLine("No match found.");
            }
        }


        private void btn_start_Click(object sender, RoutedEventArgs e)
        {
            //Plan plan = new Plan();
            //plans.Add(plan);
            //canvas.Children.Add(plan);
            //plan.Show(canvas.ActualWidth,canvas.ActualHeight);

            role = new Role();
            canvas.Children.Add(role);
            left = canvas.ActualWidth / 2;
            top = canvas.ActualHeight - 50;
            //让角色位于中间
            Canvas.SetLeft(role, left);
            Canvas.SetTop(role, top);
            //AdjustAll();

            KeyDownTimer.Start();
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
