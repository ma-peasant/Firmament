using Firmament.Module;
using Firmament.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Firmament
{
    /// <summary>
    /// EasyQuardTreeTestPage.xaml 的交互逻辑
    /// </summary>
    public partial class EasyQuardTreeTestPage : Page
    {
        //将球分组创建Task任务
        List<List<Ball>> collections = new List<List<Ball>>();
        //最大任务数，和小球的分类数保持一致
        private int taskCount = 800;
        List<Task> tasks = new List<Task>();
        private int interval = 100;    //时间间隔 ms
        private const int BallCount = 2000;   // 产生的小球的个数
        private Random random = new Random();
        #region 页码基础定义
        /**舞台宽度/2 */
        private double maxWidth;
        /**舞台高度/2 */
        private double maxHeight;
        /**小球列表 */
        private List<Ball> ballList;
        /**格子区域二位数组 */
        List<List<List<Ball>>> gridList = new List<List<List<Ball>>>();
        /**格子行数 */
        private int gridRow = 20;
        /**格子列数 */
        private int gridCol = 20;
        /**格子高宽 */
        private int gridWidth = 400;
        /**格子高宽 */
        private int gridHeight = 400;

        #endregion
        public EasyQuardTreeTestPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //格子高度
            this.gridHeight = (int)Math.Floor(this.canvas.ActualHeight / this.gridRow);
            this.gridWidth = (int)Math.Floor(this.canvas.ActualWidth / this.gridCol);
            this.ballList = new List<Ball>();
            //舞台边缘值
            this.maxWidth = this.canvas.ActualWidth-10;
            this.maxHeight = this.canvas.ActualHeight-10;
            // 格子列表

            // 格子列表
            for (int i = 0; i < gridRow; i++)
            {
                gridList.Add(new List<List<Ball>>());
                for (int j = 0; j < gridCol; j++)
                {
                    gridList[i].Add(new List<Ball>());
                }
            }
            //创建小球
            this.createBall();

            InitUpdateTimer();
        }

        private void InitUpdateTimer() {
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = this.interval; // 更新间隔，可以根据需要调整   大概就是30帧
            timer.Elapsed += Timer_Tick; ;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            update();
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
                ball.x = random.NextDouble() * this.maxWidth;
                ball.y = random.NextDouble() * this.maxHeight;
                Canvas.SetLeft(ball.rectangle, ball.x);
                Canvas.SetTop(ball.rectangle, ball.y);
                //随机速度
                ball.xSpeed = random.NextDouble() * ball.speed;
                ball.ySpeed = random.NextDouble() * ball.speed;
                this.ballList.Add(ball);
                this.canvas.Children.Add(ball.rectangle);
            }

            int groupCount  = BallCount / taskCount;
            int residue = BallCount % taskCount;
            for (int i = 0; i < taskCount; i++)
            {
                List<Ball> collection = this.ballList.Skip(i * groupCount).Take(groupCount).ToList();
                collections.Add(collection);
            }
            if (residue != 0) {
                List<Ball> collection = this.ballList.Skip(taskCount* groupCount).Take(this.ballList.Count - taskCount*groupCount).ToList();
                collections.Add(collection);
                taskCount++;
            }
            
         
        }
        private void btn_start_Click(object sender, RoutedEventArgs e)
        {
            canvas.Children.Clear();
            ballList.Clear();
        }

        public void update()
        {
            this.updateBallMoveWithTask();
            this.updateBallGridWithTask();
            this.checkCollisionWithTask();

            //updateBallMove(); ;
            //this.updateBallGrid();
            ////检查碰撞
            //this.checkCollision();
        }

       
        /**刷新小球移动 */
        private void updateBallMoveWithTask()
        {
            int len = this.ballList.Count;
            Ball ball;
            // 初始化任务池
            tasks.Clear();
            for (int j = 0; j < taskCount; j++)
            {
                int currentJ = j;
                int everyCollectionCount = collections[currentJ].Count;
                Task task =  new TaskFactory().StartNew(() =>
                {
                    for (int i = 0; i < everyCollectionCount; i++)
                    {
                        ball = collections[currentJ][i];
                        //移动
                        ball.x += ball.xSpeed;
                        ball.y += ball.ySpeed;
                        //边缘检测 达到边缘后速度取反
                        if (ball.x + ball.Width / 2 > this.maxWidth)
                        {
                            ball.x = this.maxWidth - ball.Width / 2;
                            ball.xSpeed = -ball.speed;
                        }
                        else if (ball.x - ball.Width / 2 < 0)
                        {
                            ball.x = ball.Width / 2;
                            ball.xSpeed = ball.speed;
                        }
                        if (ball.y + ball.Height / 2 > this.maxHeight)
                        {
                            ball.y = this.maxHeight - ball.Height / 2;
                            ball.ySpeed = -ball.speed;
                        }
                        else if (ball.y - ball.Height / 2 < 0)
                        {
                            ball.y = ball.Height / 2;
                            ball.ySpeed = ball.speed;
                        }
                        this.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                        {
                            Canvas.SetLeft(ball.rectangle, ball.x);
                            Canvas.SetTop(ball.rectangle, ball.y);
                        });
                    }
                });
                tasks.Add(task);
            }
            Task.WaitAll(tasks.ToArray());
        }

        private void updateBallMove()
        {
            //int len = this.ballList.Count;
            //Ball ball;
            //for (int i = 0; i < len; i++)
            //{
            //    ball = ballList[i];
            //    //移动
            //    ball.x += ball.xSpeed;
            //    ball.y += ball.ySpeed;
            //    //边缘检测 达到边缘后速度取反
            //    if (ball.x + ball.Width / 2 > this.maxWidth)
            //    {
            //        ball.x = this.maxWidth - ball.Width / 2;
            //        ball.xSpeed = -ball.speed;
            //    }
            //    else if (ball.x - ball.Width / 2 < 0)
            //    {
            //        ball.x = ball.Width / 2;
            //        ball.xSpeed = ball.speed;
            //    }
            //    if (ball.y + ball.Height / 2 > this.maxHeight)
            //    {
            //        ball.y = this.maxHeight - ball.Height / 2;
            //        ball.ySpeed = -ball.speed;
            //    }
            //    else if (ball.y - ball.Height / 2 < 0)
            //    {
            //        ball.y = ball.Height / 2;
            //        ball.ySpeed = ball.speed;
            //    }
            //    this.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
            //    {
            //        Canvas.SetLeft(ball.rectangle, ball.x);
            //        Canvas.SetTop(ball.rectangle, ball.y);
            //    });
            //}

            Parallel.ForEach(ballList, ball =>
            {
                //移动
                ball.x += ball.xSpeed;
                ball.y += ball.ySpeed;
                //边缘检测 达到边缘后速度取反
                if (ball.x + ball.Width / 2 > this.maxWidth)
                {
                    ball.x = this.maxWidth - ball.Width / 2;
                    ball.xSpeed = -ball.speed;
                }
                else if (ball.x - ball.Width / 2 < 0)
                {
                    ball.x = ball.Width / 2;
                    ball.xSpeed = ball.speed;
                }
                if (ball.y + ball.Height / 2 > this.maxHeight)
                {
                    ball.y = this.maxHeight - ball.Height / 2;
                    ball.ySpeed = -ball.speed;
                }
                else if (ball.y - ball.Height / 2 < 0)
                {
                    ball.y = ball.Height / 2;
                    ball.ySpeed = ball.speed;
                }
                this.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                {
                    Canvas.SetLeft(ball.rectangle, ball.x);
                    Canvas.SetTop(ball.rectangle, ball.y);
                });
            });
        }

        /**刷新小球所在格子 */
        private void updateBallGrid()
        {
            //清理格子
            for (int i = 0; i < this.gridRow; i++)
            {
                for (int j = 0; j < this.gridCol; j++)
                {
                    this.gridList[i][j].Clear();
                }
            }


            Parallel.ForEach(ballList, ball =>
            {
                //位置x决定的是所在的列，y决定所在的行
                int row = (int)Math.Floor(ball.y / this.gridHeight);
                int col = (int)Math.Floor(ball.x / this.gridWidth);
                ball.row = row;
                ball.col = col;
                this.gridList[row][col].Add(ball);
                this.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                {
                    ball.Hit_State = false;
                    //ball.rectangle.Fill = new SolidColorBrush(Colors.Blue);
                });
            });
            ////将小球置蓝色，重新计算小球所属行列的格子
            //for (int i = 0; i < this.ballList.Count; i++)
            //{
            //    Ball ball = this.ballList[i];
            //    //位置x决定的是所在的列，y决定所在的行
            //    int row = (int)Math.Floor(ball.y / this.gridHeight);
            //    int col = (int)Math.Floor(ball.x / this.gridWidth);
            //    ball.row = row;
            //    ball.col = col;
            //    this.gridList[row][col].Add(ball);
            //    this.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
            //    {
            //        ball.Hit_State = false;
            //        //ball.rectangle.Fill = new SolidColorBrush(Colors.Blue);
            //    });
            //}
         }

        /**刷新小球所在格子 */
        private void updateBallGridWithTask()
        {
            //清理格子
            for (int i = 0; i < this.gridRow; i++)
            {
                for (int j = 0; j < this.gridCol; j++)
                {
                    this.gridList[i][j].Clear();
                }
            }

            // 初始化任务池
            tasks.Clear();

            for (int j = 0; j < taskCount; j++)
            {
                int currentJ = j;
                int everyCollectionCount = collections[currentJ].Count;
                Task task = new TaskFactory().StartNew(() =>
                {
                    //将小球置蓝色，重新计算小球所属行列的格子
                    for (int i = 0; i < everyCollectionCount; i++)
                    {
                        Ball ball = collections[currentJ][i];
                        //位置x决定的是所在的列，y决定所在的行
                        int row = (int)Math.Floor(ball.y / this.gridHeight);
                        int col = (int)Math.Floor(ball.x / this.gridWidth);
                        ball.row = row;
                        ball.col = col;
                        this.gridList[row][col].Add(ball);
                        this.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                        {
                            ball.Hit_State = false;
                            //ball.rectangle.Fill = new SolidColorBrush(Colors.Blue);
                        });
                    }
                });
                tasks.Add(task);
            }
            Task.WaitAll(tasks.ToArray());
        }

        /**碰撞检测 */
        private void checkCollision()
        {
            //int count = 0;
            //for (int i = 0; i < this.ballList.Count; i++)
            //{
            //    Ball ballA = this.ballList[i];
            //    List<Ball> list = this.gridList[ballA.row][ballA.col];
            //    for (int j = 0; j < list.Count; j++)
            //    {
            //        //count++;
            //        Ball ballB = list[j];
            //        if (ballA != ballB)
            //        {
            //            if (this.rectRect(ballA, ballB))
            //            {
            //                this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate
            //                {
            //                    ballA.Hit_State = true;
            //                    ballB.Hit_State = true;
            //                    //ballA.rectangle.Fill = new SolidColorBrush(Colors.Red);
            //                    //ballB.rectangle.Fill = new SolidColorBrush(Colors.Red);
            //                });

            //            }
            //        }
            //    }
            //}

            Parallel.ForEach(ballList, ball =>
            {
                List<Ball> list = this.gridList[ball.row][ball.col];
                for (int j = 0; j < list.Count; j++)
                {
                    //count++;
                    Ball ballB = list[j];
                    if (ball != ballB)
                    {
                        if (this.rectRect(ball, ballB))
                        {
                            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate
                            {
                                ball.Hit_State = true;
                                ballB.Hit_State = true;
                                //ballA.rectangle.Fill = new SolidColorBrush(Colors.Red);
                                //ballB.rectangle.Fill = new SolidColorBrush(Colors.Red);
                            });
                        }
                    }
                }
            });
            //Console.WriteLine("检查次数:", count);
        }
        private void checkCollisionWithTask()
        {
            // 初始化任务池
            tasks.Clear();

            for (int l = 0; l < 100; l++)
            {
                int currentl = l;
                int everyCollectionCount = collections[currentl].Count;
                Task task = new TaskFactory().StartNew(() =>
                {
                    //将小球置蓝色，重新计算小球所属行列的格子
                    for (int k = 0; k < everyCollectionCount; k++)
                    {
                        //int count = 0;
                            Ball ballA = collections[currentl][k];
                            List<Ball> list = this.gridList[ballA.row][ballA.col];
                            for (int j = 0; j < list.Count; j++)
                            {
                                //count++;
                                Ball ballB = list[j];
                                if (ballA != ballB)
                                {
                                    if (this.rectRect(ballA, ballB))
                                    {
                                        this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate
                                        {
                                            ballA.Hit_State = true;
                                            ballB.Hit_State = true;
                                            //ballA.rectangle.Fill = new SolidColorBrush(Colors.Red);
                                            //ballB.rectangle.Fill = new SolidColorBrush(Colors.Red);
                                        });

                                    }
                                }
                        }
                    }
                });
                tasks.Add(task);
            }
            Task.WaitAll(tasks.ToArray());


           
            //Console.WriteLine("检查次数:", count);
        }

        /**
         * cc.Intersection.rectRect
         * @param a 
         * @param b
         * @returns true碰撞 false未碰撞
         */
        private bool rectRect(Ball a, Ball b)
        {
            var a_min_x = a.x - a.Width / 2;
            var a_min_y = a.y - a.Height / 2;
            var a_max_x = a.x + a.Width / 2;
            var a_max_y = a.y + a.Height / 2;
            var b_min_x = b.x - b.Width / 2;
            var b_min_y = b.y - b.Height / 2;
            var b_max_x = b.x + b.Width / 2;
            var b_max_y = b.y + b.Height / 2;
            return a_min_x <= b_max_x && a_max_x >= b_min_x && a_min_y <= b_max_y && a_max_y >= b_min_y;
        }
    }
}
