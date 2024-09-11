using Firmament.Module;
using Firmament.Utils.QuardTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
        private int interval = 100;
        QuadTree<BaseElement> rootTree;
        private const int MaxCount = 10;
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
            this.maxWidth = canvas.ActualWidth - 10;
            this.maxHeight = canvas.ActualHeight - 10;
            rootTree = new QuadTree<BaseElement>(new Rect(0, 0, this.canvas.ActualWidth, this.canvas.ActualHeight), MaxCount);
        }


        public void InsertElement(BaseElement baseElement)
        {
            ballList.Add(baseElement);
            this.rootTree.Insert(baseElement);
        }


        public void InitUpdateTimer()
        {
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = this.interval; // 更新间隔，可以根据需要调整   大概就是30帧
            timer.Elapsed += Timer_Tick; ;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Common.mainPage.canvas.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate
            {
                //需要计算小球的矩形情况
                lock (lockObject)
                {
                    this.updateBallGrid();
                }

                lock (lockObject)
                {
                    ////检查碰撞
                    this.checkCollision();
                }
            });
           
        }

        private void updateBallGrid()
        {
            //更新子弹和敌人的位置，不再单个控制
            for (int i = 0; i < this.ballList.Count; i++)
            {
                if (ballList[i].Tag == 0)
                {
                    continue;
                }
                else {
                    if (ballList[i].Tag == 1)
                    {
                        UpdatePlanPosition(ballList[i]);
                    }
                    else if(ballList[i].Tag == 2) {
                        UpdateBulletPosition(ballList[i]);
                    }
                }
            }

                rootTree.Update(this.ballList.ToList<BaseElement>());
        }


        private void UpdatePlanPosition(BaseElement baseElement) {
            bool isOut = false;
            baseElement.Y = baseElement.Y + baseElement.YSpeed;

            //边缘检测 达到边缘后速度取反
            if (baseElement.X + baseElement.Width / 2 > maxWidth || baseElement.X - baseElement.Width / 2 < 0 ||
             baseElement.Y + baseElement.Height / 2 > maxHeight || baseElement.Y - baseElement.Height / 2 < 0)
            {
                isOut = true;
            }

            if (isOut)
            {
                //在UI和四叉树上移除该对象
                Common.mainPage.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                {
                    Common.ballList.Remove(baseElement);
                    Common.mainPage.canvas.Children.Remove(baseElement.image);
                });

            }
            else
            {
                Common.mainPage.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                {
                    Canvas.SetLeft(baseElement.image, baseElement.X);
                    Canvas.SetTop(baseElement.image, baseElement.Y);
                });

            }

        }
        private void UpdateBulletPosition(BaseElement baseElement) {
            bool isout = false;

            baseElement.Y -= baseElement.YSpeed;
            //边缘检测 达到边缘后速度取反
            if (baseElement.X + baseElement.Width / 2 > Common.mainPage.canvas.ActualWidth)
            {
                isout = true;
            }
            else if (baseElement.X - baseElement.Width / 2 < 0)
            {
                isout = true;
            }
            if (baseElement.Y + baseElement.Height / 2 > Common.mainPage.canvas.ActualHeight)
            {
                isout = true;
            }
            else if (baseElement.Y - baseElement.Height / 2 < 0)
            {
                isout = true;
            }
            if (isout)
            {
                //在UI和四叉树上移除该对象
                Common.mainPage.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                {
                    Common.ballList.Remove(baseElement);
                    Common.mainPage.canvas.Children.Remove(baseElement.image);
                });
            }
            else
            {
                Common.mainPage.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                {
                    Canvas.SetLeft(baseElement.image, baseElement.X);
                    Canvas.SetTop(baseElement.image, baseElement.Y);
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
                    BaseElement ballA = this.ballList[i];
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
                            if ((ballA.Tag == 0 && ballB.Tag == 1) || (ballA.Tag == 1 && ballB.Tag == 2) || (ballA.Tag == 1 && ballB.Tag == 0) || (ballA.Tag == 2 && ballB.Tag == 1))
                            {
                                if (this.rectRect(ballA, ballB))
                                {
                                    ballA.HitState = true;
                                    ballB.HitState = true;
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


            }
            //Console.WriteLine("检查次数:", count);
        }

        /**
         * cc.Intersection.rectRect
         * @param a 
         * @param b
         * @returns true碰撞 false未碰撞
         */
        private bool rectRect(BaseElement a, BaseElement b)
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