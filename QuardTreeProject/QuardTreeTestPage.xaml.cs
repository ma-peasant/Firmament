using QuardTreeProject.Module;
using QuardTreeProject.Utils.QuardTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace QuardTreeProject
{
    /// <summary>
    /// QuardTreeTestPage.xaml 的交互逻辑
    /// </summary>
    public partial class QuardTreeTestPage : Page
    {
        /**小球列表 */
        private List<Ball> ballList;
        private int interval = 100;
        QuadTree rootTree;
        private const int BallCount = 1000;
        Random random = new Random();
        private const int MaxCount = 5;
        //界面边缘
        private double maxWidth;
        private double maxHeight;
        Object lockObject = new Object();
        public QuardTreeTestPage()
        {
            InitializeComponent();
            this.Loaded += QuardTreeTestPage_Loaded;
        }
        private void QuardTreeTestPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.maxWidth = this.canvas.ActualWidth - 10;
            this.maxHeight = this.canvas.ActualHeight - 10;
            ballList = new List<Ball>();
            rootTree =  new QuadTree(new Rect(0, 0, this.canvas.ActualWidth, this.canvas.ActualHeight),MaxCount);
        }
        private void btn_start_Click(object sender, RoutedEventArgs e)
        {
            //1、生产矩形
            //创建小球
            this.createBall();
            InitUpdateTimer();
        }

        /// <summary>
        /// 创建小球
        /// </summary>
        private void createBall()
        {
            for (int i = 0; i < BallCount; i++)
            {
                //随机位置
                Ball ball = new Ball();
                ball.X = random.NextDouble() * this.maxWidth;
                ball.Y = random.NextDouble() * this.maxHeight;
                Canvas.SetLeft(ball.control, ball.X);
                Canvas.SetTop(ball.control, ball.Y);
                //随机速度
                ball.XSpeed = random.NextDouble() * ball.Speed;
                ball.YSpeed = random.NextDouble() * ball.Speed;
                this.rootTree.Insert(ball);
                ballList.Add(ball);
                this.canvas.Children.Add(ball.control);
            }
        }

    private void InitUpdateTimer()
    {
        System.Timers.Timer timer = new System.Timers.Timer();
        timer.Interval = this.interval; // 更新间隔，可以根据需要调整   大概就是30帧
        timer.Elapsed += Timer_Tick; ;
        timer.Start();
    }

        private void Timer_Tick(object sender, ElapsedEventArgs e)
        {
            Update();
        }
        public void Update()
        {
            lock (lockObject) { 
              updateBallMove();
            }
            //需要计算小球的矩形情况
            this.updateBallGrid();
            this.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
            {
                this.AddLine(rootTree, rootTree.root, true);
            });

            lock (lockObject)
            {
                ////检查碰撞
                this.checkCollision();
            }
        }

        private void updateBallGrid()
        {
            rootTree.Update(this.ballList.ToList<Ball>());
            //将小球置蓝色，重新计算小球所属行列的格子
            for (int i = 0; i < this.ballList.Count; i++)
            {
                Ball ball = this.ballList[i];
                this.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                {
                    ball.HitState = false;
                });
            }
        }

        private void updateBallMove()
        {
            int len = this.ballList.Count;
            Ball ball;
            for (int i = 0; i < len; i++)
            {
                ball = ballList[i];
                //移动
                ball.X += ball.XSpeed;
                ball.Y += ball.YSpeed;
                //边缘检测 达到边缘后速度取反
                if (ball.X + ball.Width / 2 > this.maxWidth)
                {
                    ball.X = this.maxWidth - ball.Width / 2;
                    ball.XSpeed = -ball.Speed;
                }
                else if (ball.X - ball.Width / 2 < 0)
                {
                    ball.X = ball.Width / 2;
                    ball.XSpeed = ball.Speed;
                }
                if (ball.Y + ball.Height / 2 > this.maxHeight)
                {
                    ball.Y = this.maxHeight - ball.Height / 2;
                    ball.YSpeed = -ball.Speed;
                }
                else if (ball.Y - ball.Height / 2 < 0)
                {
                    ball.Y = ball.Height / 2;
                    ball.YSpeed = ball.Speed;
                }
                this.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                {
                    Canvas.SetLeft(ball.control, ball.X);
                    Canvas.SetTop(ball.control, ball.Y);
                });
            }
        }

        private void checkCollision()
        {
           
            for (int i = 0; i < this.ballList.Count; i++)
            {
                try
                {
                    //flag相等的代表是一个节点的， 对同节点的进行判断。 
                    Ball ballA = this.ballList[i];
                    List<Ball> list = this.ballList.Where(ball=>ball.Flag == ballA.Flag).ToList();
                    if (list != null && list.Count > 0)
                    {
                        for (int j = 0; j < list.Count; j++)
                        {
                            Ball ballB = list[j];
                            if (ballB == null) {
                                break;
                            }
                            if (ballA != ballB)
                            {
                                if (this.rectRect(ballA, ballB))
                                {
                                    this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate
                                    {
                                        ballA.HitState = true;
                                        ballB.HitState = true;
                                        //ballA.rectangle.Fill = new SolidColorBrush(Colors.Red);
                                        //ballB.rectangle.Fill = new SolidColorBrush(Colors.Red);

                                        this.canvas.Children.Remove(ballA.control);
                                        this.canvas.Children.Remove(ballB.control);
                                    });
                                    this.ballList.Remove(ballA);
                                    this.ballList.Remove(ballB);
                                }
                            }
                        }
                    }
                }
                catch (NullReferenceException e)
                {
                    Console.WriteLine(e.Message);
                }
                catch (InvalidOperationException e) {
                    Console.WriteLine(e.Message);
                }
            }
        }

        /**
         * cc.Intersection.rectRect
         * @param a 
         * @param b
         * @returns true碰撞 false未碰撞
         */
        private bool rectRect(Ball a, Ball b)
        {
            var a_min_x = a.X - a.Width / 2;
            var a_min_y = a.Y - a.Height / 2;
            var a_max_x = a.X + a.Width / 2;
            var a_max_y = a.Y + a.Height / 2;
            var b_min_x = b.X - b.Width / 2;
            var b_min_y = b.Y - b.Height / 2;
            var b_max_x = b.X + b.Width / 2;
            var b_max_y = b.Y + b.Height / 2;
            return a_min_x <= b_max_x && a_max_x >= b_min_x && a_min_y <= b_max_y && a_max_y >= b_min_y;
        }

        private void AddLine(QuadTree quard, QuadTreeNode node,bool isupdate)
        {
            if (isupdate) {
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
                    AddLine(quard, node2, false);
                }
            }
        }
    }
}
