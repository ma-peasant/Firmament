using Firmament.Module;
using Firmament.Utils;
using System;
using System.Threading;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace Firmament
{
    /// <summary>
    /// QuardTreeTestPage.xaml 的交互逻辑
    /// </summary>
    public partial class GamePage : Page
    {
        //应该把这些都放到Common里面去， 包括产生敌机
        private InputHandler _inputHandler;
        Role role = null;

        //开一个定时器 ，生产敌机
        System.Timers.Timer ArmyProductTimer;

        QuardTreeHelp quardTreeHelp = null;
        public GamePage()
        {
            InitializeComponent();
            // 实例化 InputHandler
            this.Loaded += QuardTreeTestPage_Loaded; ;
        }

        private void GamePage_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            _inputHandler.HandleKeyUp(e); // 将按键事件传递给 InputHandler
          
            e.Handled = true;
        }
        private void GamePage_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            Console.WriteLine(e.Key.ToString()  + ":" + e.ImeProcessedKey.ToString());
            _inputHandler.HandleKeyDown(e); // 将按键事件传递给 InputHandler
            e.Handled = true;
        }

        private void QuardTreeTestPage_Loaded(object sender, RoutedEventArgs e)
        {
            _inputHandler = new InputHandler();
            InitArmyProductTimer();
            quardTreeHelp = new QuardTreeHelp(this.canvas);
            Common.quardTreeHelp = quardTreeHelp;
        }

        #region 定时器初始化
      
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
            Plan plan = new Plan();
            quardTreeHelp.InsertElement(plan);
            this.canvas.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
            {
                canvas.Children.Add(plan.image);
            });
        }

        #endregion

        private bool isStart = false;
        private void btn_start_Click(object sender, RoutedEventArgs e)
        {
            if (isStart)
            {
                isStart = false;
                GameSuspend();
                this.btn_start.Content = "开始游戏";
            }
            else {
                isStart = true;
                GameStart();
                this.btn_start.Content = "暂停";
                this.canvas.Focus();
                Keyboard.Focus(this.canvas);
            }
        }

        //游戏暂停
        private void GameSuspend() {
            role.KeyDownTimer.Stop();
            ArmyProductTimer.Stop();
            quardTreeHelp.Suspend();
        }

        private void GameStart() {
            if (role == null) {
                //1、创建角色
                role = new Role(_inputHandler);
                role.GameOverEvent += Role_GameOverEvent; ;
                canvas.Children.Add(role.image);
                quardTreeHelp.InsertElement(role);
                role.X = canvas.ActualWidth / 2;
                role.Y = canvas.ActualHeight - 50;
                //2、让角色位于中间
                Canvas.SetLeft(role.image, role.X);
                Canvas.SetTop(role.image, role.Y);
            }
            //3、启动定时器
            ArmyProductTimer.Start(); //生产敌机扫描
            //4、检测碰撞
            quardTreeHelp.Start();
        }

        private void Role_GameOverEvent()
        {
            //1、游戏结束， 将role清除
            role.KeyDownTimer.Stop();
            role.GameOverEvent -= Role_GameOverEvent;
            role = null;
            //停止定时器
            ArmyProductTimer.Stop();
            quardTreeHelp.Stop();
            this.canvas.Children.Clear();
            this.btn_start.Content = "开始游戏";
            MessageBox.Show("游戏结束");
           
        }
    }
}
