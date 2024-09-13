using Firmament.Utils;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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

        private InputHandler _inputHandler;
        bool isSKeyPressed = false;
        bool isLeftKeyPressed = false;
        bool isRightKeyPressed = false;
        bool isUpKeyPressed = false;
        bool isDownKeyPressed = false;

        //开一个定时器 ，专门处理按钮点击
        public System.Timers.Timer KeyDownTimer;

        public Role(InputHandler inputHandler) {
          this.image = new System.Windows.Controls.Image();
          this.Width =  this.image.Width = 30;
          this.Height =  this.image.Height = 30;
           this.image.Source = new BitmapImage(new Uri("./Images/plan1.png", UriKind.Relative));
           this.Tag = 0;

            // 使用外部传入的 InputHandler
            _inputHandler = inputHandler;

            // 订阅 InputHandler 的按键事件
            _inputHandler.OnKeyPressed += HandleKeyPress;
            InitKeyDownTimer();
        }

        private void InitKeyDownTimer()
        {
            KeyDownTimer = new System.Timers.Timer();
            KeyDownTimer.Interval = 100; // 计时器间隔为 100 毫秒
            KeyDownTimer.AutoReset = true;
            KeyDownTimer.Elapsed += KeyDownTimer_Elapsed;
            KeyDownTimer.Start();
        }

        /// <summary>
        /// 按键处理程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KeyDownTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            double left = this.X;
            double top = this.Y;
            if (isLeftKeyPressed)
            {
                left = left - 10;
                if (left <= 0)
                {
                    left = 0;
                }
                else if (left >= Common.mainPage.canvas.ActualWidth - 30)
                {
                    left = Common.mainPage.canvas.ActualWidth - 30;
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
                else if (left >= Common.mainPage.canvas.ActualWidth - 30)
                {
                    left = Common.mainPage.canvas.ActualWidth - 30;
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
                else if (top >= Common.mainPage.canvas.ActualHeight - 30)
                {
                    top = Common.mainPage.canvas.ActualHeight - 30;
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
                else if (top >= Common.mainPage.canvas.ActualHeight - 30)
                {
                    top = Common.mainPage.canvas.ActualHeight - 30;
                }
            }
            Common.mainPage.canvas.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
            {
            this.X = left;
            this.Y = top;
            Canvas.SetLeft(this.image, this.X);
            Canvas.SetTop(this.image, this.Y);

            if (isSKeyPressed) {
                Bullet bullet = new Bullet(this);
                    Common.mainPage.canvas.Children.Add(bullet.image);
                    Common.quardTreeHelp.InsertElement(bullet);
                }
            });
        }

        private void HandleKeyPress(KeyEventArgs e, string type)
        {
            if (type == "UP")
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
            else {
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
        }

        public Rect getRec()
        {
            return new Rect(this.X, this.Y, 30, 30);
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
