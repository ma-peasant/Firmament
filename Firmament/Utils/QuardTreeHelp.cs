using Firmament.Module;
using Firmament.Utils.QuardTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Firmament.Utils
{
    //检测碰撞
    public class QuardTreeHelp
    {
        /**小球列表 */
        private List<BaseElement> ballList;
        QuadTree<BaseElement> rootTree;
        private const int MaxCount = 20;
        //界面边缘
        private double maxWidth;
        private double maxHeight;
        private Canvas canvas;
        Object lockObject = new Object();
        public QuardTreeHelp(Canvas canvas)
        {
            this.canvas = canvas;
            //格子高度
            this.ballList = new List<BaseElement>();
            Common.ballList = this.ballList;
            //舞台边缘值
            maxWidth = canvas.ActualWidth - 10;
            maxHeight = canvas.ActualHeight - 10;
            rootTree = new QuadTree<BaseElement>(new Rect(0, 0, this.canvas.ActualWidth, this.canvas.ActualHeight), MaxCount);
        }

        public void InsertElement(BaseElement baseElement)
        {
            ballList.Add(baseElement);
            this.rootTree.Insert(baseElement);
        }

        public void Start()
        {
            isRun = true;
            Run();
        }
        //暂停
        public void Suspend()
        {
            isRun = false;
        }
        public void Stop() {
            isRun = false;
            this.ballList.Clear();
            Common.ballList.Clear();
            //四叉树不用动， 更新角色位置的时候会根据ballList重新计算的
        }
        bool isRun = true;
        public async void Run()
        {
            while (isRun)
            {
                lock (lockObject)
                {
                    rootTree.Update(this.ballList.ToList<BaseElement>());
                    ////检查碰撞
                    this.ParallelCheckCollision();
                }
                // 控制帧率
                await Task.Delay(16); // 每秒 60 帧
            }
        }


        private void ParallelCheckCollision() {
            // 假设 Tag == 0 是主角色，Tag == 1 是敌机
            var mainAndEnemies = this.ballList.Where(ball => ball.Tag == 0 || ball.Tag == 1).ToList();

            // 对主角色和敌机并行执行逻辑
            Parallel.ForEach(mainAndEnemies, ballA =>
            {
                try
                {
                    // 对于同一节点的物体进行判断
                    List<BaseElement> list = this.ballList.Where(ball => ball.Flag == ballA.Flag).ToList();

                    if (list != null && list.Count > 0)
                    {
                        for (int j = 0; j < list.Count; j++)
                        {
                            BaseElement ballB = list[j];
                            if (ballB == null)
                            {
                                break;
                            }

                            // 判断条件，根据 Tag 进行不同的碰撞检测逻辑
                            if ((ballA.Tag == 0 && ballB.Tag == 1) || (ballA.Tag == 1 && ballB.Tag == 2) || (ballA.Tag == 1 && ballB.Tag == 0) || (ballA.Tag == 2 && ballB.Tag == 1))
                            {
                                if (this.IsHit(ballA, ballB))
                                {
                                    // 并发修改时要加锁确保安全
                                    lock (ballA)
                                    {
                                        ballA.HitState = true;
                                    }

                                    lock (ballB)
                                    {
                                        ballB.HitState = true;
                                    }
                                }
                            }
                        }
                    }
                }
                catch (NullReferenceException e)
                {
                    Console.WriteLine(e.Message);
                }
                catch (InvalidOperationException e)
                {
                    Console.WriteLine(e.Message);
                }
            });
        }

        /**
         * cc.Intersection.rectRect
         * @param a 
         * @param b
         * @returns true碰撞 false未碰撞
         */
        private bool IsHit(BaseElement a, BaseElement b)
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

    }
}