﻿using Firmament.Module;
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
        //敌机集合
        List<Plan> ArmyPlans = new List<Plan>();

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
                ArmyPlans.Add(plan);
                canvas.Children.Add(plan);
                plan.Show(canvas.ActualWidth, canvas.ActualHeight);
                ArmyPlans.Add(plan);
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
            QuardTest();
            //FunctionTest();

            ////1、创建角色
            //role = new Role();
            //canvas.Children.Add(role);
            //left = canvas.ActualWidth / 2;
            //top = canvas.ActualHeight - 50;
            ////2、让角色位于中间
            //Canvas.SetLeft(role, left);
            //Canvas.SetTop(role, top);
            ////3、启动定时器
            //KeyDownTimer.Start();     //按键功能扫描
            //ArmyProductTimer.Start(); //生产敌机扫描
            ////4、检测碰撞
            //AdjustAll();
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

            if (plan.IsOver(bullets))
            {
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
                for (int i = 0; i < ArmyPlans.Count; i++)
                {
                    Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                    {
                        AdjustPositatiom(ArmyPlans[i]);
                    });
                }
            });

        }

        private void FunctionTest()
        {
            Rect rect = new Rect(0, 0, 50, 50);
            Rectangle rectangle = new Rectangle();
            rectangle.Width = 50;
            rectangle.Height = 50;
            rectangle.Fill = new SolidColorBrush(Colors.Green);
            Canvas.SetLeft(rectangle, rect.X);
            Canvas.SetTop(rectangle, rect.Y);
            canvas.Children.Add(rectangle);

            Rect rect2 = new Rect(60, 60, 10, 10);
            Rectangle rectangle2 = new Rectangle();
            rectangle2.Width = 10;
            rectangle2.Height = 10;
            rectangle2.Fill = new SolidColorBrush(Colors.Red);
            Canvas.SetLeft(rectangle2, rect2.X);
            Canvas.SetTop(rectangle2, rect2.Y);
            canvas.Children.Add(rectangle2);

            if (CheckCollision.IsIntersecting(rect, rect2))
            {
                MessageBox.Show("相交");
            }




        }


        private Random random = new Random();
        private QuadTree quadTree;
        //四叉树算法测试
        private void QuardTest()
        {
            count = 0;
            canvas.Children.Clear();
            quadTree = new QuadTree(new Rect(0, 0, this.canvas.ActualWidth, this.canvas.ActualHeight), 5);
            for (int i = 0; i < 50; i++)
            {
                double x = random.NextDouble() * 10 * this.canvas.ActualWidth / 10;
                double y = random.NextDouble() * 10 * this.canvas.ActualHeight / 10;
                Ball ball = new Ball();
                Canvas.SetLeft(ball.rectangle, x);
                Canvas.SetTop(ball.rectangle, y);
                canvas.Children.Add(ball.rectangle);
                quadTree.Insert(ball);
            }
            foreach (Rectangle rectangle1 in canvas.Children)
            {
                ChangeLocation(rectangle1);
            }
            //ChangeCheck(quadTree);
        }

        public void ChangeLocation(Rectangle rec)
        {
            //DispatcherTimer 运行在UI线程
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(16); // 更新间隔，可以根据需要调整
            timer.Tick += (sender, e) => {
                double canvasWidth = canvas.ActualWidth; // 画布宽度
                double canvasHeight = canvas.ActualHeight; // 画布高度

                // 计算新的位置
                double newX = 0, newY = 0;
                newX = Canvas.GetLeft(rec) + random.NextDouble() * 20 - 10; // X 轴随机偏移量
                newY = Canvas.GetTop(rec) + random.NextDouble() * 20 - 10; // Y 轴随机偏移量

                // 确保新的位置在规定范围内
                newX = Math.Max(0, Math.Min(canvasWidth - rec.Width, newX));
                newY = Math.Max(0, Math.Min(canvasHeight - rec.Height, newY));
                // 更新 Rectangle 的位置
                Canvas.SetLeft(rec, newX);
                Canvas.SetTop(rec, newY);
                HitCheck(quadTree, quadTree.root);
            };
            timer.Start();
        }

        public void ChangeCheck(QuadTree quard)
        {
            //DispatcherTimer是运行在UI线程上的
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 16; // 更新间隔，可以根据需要调整
            timer.Elapsed += (sender, e) => {
                //new TaskFactory().StartNew(() =>
                //{
                //    Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                //    {
                //        AddLine(quard,quard.root);
                //    });

                //});

                Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                {
                    HitCheck(quard, quard.root);
                });

            };
            timer.Start();
        }

        int count = 0;
        private void HitCheck(QuadTree quard, QuadTreeNode root)
        {
            //if (root != null)
            //{
            //    if (root.Objects.Count > 0)
            //    {
            //        // 进行碰撞检测
            //        foreach (Ball obj in root.Objects)
            //        {
            //            // 查询与当前对象可能发生碰撞的其他对象
            //            //List<Ball> potentialCollisions = quard.Query(obj);
            //            //List<Ball> list = this.ballList.Where(ball => ball.flag == ballA.flag).ToList();
            //            //要排除自身
            //            if (potentialCollisions.Contains(obj))
            //            {
            //                potentialCollisions.Remove(obj);
            //            }
            //            if (potentialCollisions.Count > 0)
            //            {
            //                bool isHit = false;
            //                // 对查询到的对象进行碰撞检测
            //                foreach (var collisionObj in potentialCollisions)
            //                {
            //                    if (IsColliding(obj, collisionObj))
            //                    {
            //                        isHit = true;
            //                        HandleCollision(obj, collisionObj);
            //                        break;
            //                    }
            //                }
            //                if (!isHit)
            //                {
            //                    obj.rectangle.Fill = new SolidColorBrush(Colors.Green);
            //                }

            //            }

            //        }
            //    }

            //    foreach (QuadTreeNode node2 in root.Children)
            //    {
            //        HitCheck(quard, node2);
            //    }
            //}
        }

        private void HandleCollision(Ball obj, Ball collisionObj)
        {
            //obj.Fill = new SolidColorBrush(Colors.Red);
            //collisionObj.Fill = new SolidColorBrush(Colors.Red);
            this.canvas.Children.Remove(obj.rectangle);
            this.canvas.Children.Remove(collisionObj.rectangle);
            this.quadTree.delete(obj);
            this.quadTree.delete(collisionObj);
        }

        private bool IsColliding(Ball obj, Ball obj2)
        {
            Rect rectObj = new Rect(obj.x, obj.y, obj.Width, obj.Height);
            Rect rectObj2 = new Rect(obj2.x, obj2.y, obj2.Width, obj2.Height);
            return rectObj.IntersectsWith(rectObj2);
        }

        //遍历四叉树，如果节点的Bound不为空，就绘制
        private void AddLine(QuadTree quard, QuadTreeNode node)
        {
            if (node == quard.root)
            {
                // 使用 LINQ 查询语句筛选出颜色为黄色的矩形
                var yellowRectangles = canvas.Children.OfType<Rectangle>().Where(rectangle => rectangle.Stroke is SolidColorBrush brush && brush.Color == Colors.Yellow).ToList();
                // 从 Children 集合中移除筛选出的矩形
                foreach (var rectangle in yellowRectangles)
                {
                    canvas.Children.Remove(rectangle);
                }
            }

            if (node != null)
            {
                Rectangle rectangle = new Rectangle();
                rectangle.Width = node.Bounds.Width;
                rectangle.Height = node.Bounds.Height;
                Canvas.SetLeft(rectangle, node.Bounds.X);
                Canvas.SetTop(rectangle, node.Bounds.Y);
                rectangle.Stroke = new SolidColorBrush(Colors.Yellow);
                rectangle.StrokeThickness = 1;
                rectangle.Fill = new SolidColorBrush(Colors.Transparent);
                canvas.Children.Add(rectangle);

                foreach (QuadTreeNode node2 in node.Children)
                {
                    AddLine(quard, node2);
                }
            }
        }
    }
}
