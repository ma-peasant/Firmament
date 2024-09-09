using Firmament.Module;
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
using System.Windows.Threading;

namespace Firmament.Utils
{
    //检测碰撞
    class QuardTreeHelp
    {
        //将球分组创建Task任务
        List<List<BaseElement>> collections = new List<List<BaseElement>>();
        //最大任务数，和小球的分类数保持一致
        private int taskCount = 500;
        List<Task> tasks = new List<Task>();
        private int interval = 100;    //时间间隔 ms
        private Random random = new Random();
        #region 页码基础定义
        /**舞台宽度/2 */
        private double maxWidth;
        /**舞台高度/2 */
        private double maxHeight;
        /**小球列表 */
        private List<BaseElement> ballList;
        /**格子区域二位数组 */
        List<List<List<BaseElement>>> gridList = new List<List<List<BaseElement>>>();
        /**格子行数 */
        private int gridRow = 20;
        /**格子列数 */
        private int gridCol = 20;
        /**格子高宽 */
        private int gridWidth = 400;
        /**格子高宽 */
        private int gridHeight = 400;
        #endregion
        private Canvas canvas;
        public QuardTreeHelp(Canvas canvas)
        {
            this.canvas = canvas;
            //格子高度
            this.gridHeight = (int)Math.Floor(canvas.ActualHeight / this.gridRow);
            this.gridWidth = (int)Math.Floor(canvas.ActualWidth / this.gridCol);
            this.ballList = new List<BaseElement>();
            //舞台边缘值
            this.maxWidth = canvas.ActualWidth - 10;
            this.maxHeight = canvas.ActualHeight - 10;
            // 格子列表

            // 格子列表
            for (int i = 0; i < gridRow; i++)
            {
                gridList.Add(new List<List<BaseElement>>());
                for (int j = 0; j < gridCol; j++)
                {
                    gridList[i].Add(new List<BaseElement>());
                }
            }

            InitUpdateTimer();
        }


        public void InsertElement(BaseElement baseElement)
        {
            ballList.Add(baseElement);
        }


        private void InitUpdateTimer()
        {
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = this.interval; // 更新间隔，可以根据需要调整   大概就是30帧
            timer.Elapsed += Timer_Tick; ;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            update();
        }


        public void update()
        {
            int count = this.ballList.Count;
            if (count > 0)
            {
                int groupCount = count / taskCount;
                int residue = count % taskCount;
                for (int i = 0; i < taskCount; i++)
                {
                    List<BaseElement> collection = this.ballList.Skip(i * groupCount).Take(groupCount).ToList();
                    collections.Add(collection);
                }
                if (residue != 0)
                {
                    List<BaseElement> collection = this.ballList.Skip(taskCount * groupCount).Take(this.ballList.Count - taskCount * groupCount).ToList();
                    collections.Add(collection);
                    taskCount++;
                }
            }
            else
            {
                return;
            }
            this.updateBallGridWithTask();
            this.checkCollisionWithTask();
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
                        BaseElement ball = collections[currentJ][i];
                        //位置x决定的是所在的列，y决定所在的行
                        int row = (int)Math.Floor(ball.y / this.gridHeight);
                        int col = (int)Math.Floor(ball.x / this.gridWidth);
                        ball.row = row;
                        ball.col = col;
                        this.gridList[row][col].Add(ball);
                        this.canvas.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                        {
                            ball.Hit_State = false;
                        });
                    }
                });
                tasks.Add(task);
            }
            Task.WaitAll(tasks.ToArray());
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
                        BaseElement ballA = collections[currentl][k];
                        List<BaseElement> list = this.gridList[ballA.row][ballA.col];
                        for (int j = 0; j < list.Count; j++)
                        {
                            //count++;
                            BaseElement ballB = list[j];
                            if (ballA != ballB)
                            {
                                if (this.rectRect(ballA, ballB))
                                {
                                    this.canvas.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate
                                    {
                                        ballA.Hit_State = true;
                                        ballB.Hit_State = true;
                                        if (ballA.tag == 0 || ballB.tag == 0) {
                                            MessageBox.Show("游戏结束");
                                        }
                                    });

                                }
                            }
                        }
                    }
                });
                tasks.Add(task);
            }
            Task.WaitAll(tasks.ToArray());
        }

        /**
         * cc.Intersection.rectRect
         * @param a 
         * @param b
         * @returns true碰撞 false未碰撞
         */
        private bool rectRect(BaseElement a, BaseElement b)
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