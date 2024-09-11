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
    public class QuardTreeHelp
    {
        /**小球列表 */
        private List<BaseElement> ballList;
        private int interval = 100;
        QuadTree rootTree;
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
            rootTree = new QuadTree(new Rect(0, 0, this.canvas.ActualWidth, this.canvas.ActualHeight), MaxCount);
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
            //需要计算小球的矩形情况
            lock (lockObject) { 
                this.updateBallGrid();
            }

            lock (lockObject)
            {
                ////检查碰撞
                this.checkCollision();
            }
        }

        private void updateBallGrid()
        {
            rootTree.Update(this.ballList.ToList<BaseElement>());
        }

        private void checkCollision()
        {
            for (int i = 0; i < this.ballList.Count; i++)
            {
                try
                {
                    //flag相等的代表是一个节点的， 对同节点的进行判断。 
                    BaseElement ballA = this.ballList[i];
                    List<BaseElement> list = this.ballList.Where(ball => ball.flag == ballA.flag).ToList();
                    if (list != null && list.Count > 0)
                    {
                        for (int j = 0; j < list.Count; j++)
                        {
                            BaseElement ballB = list[j];
                            if (ballB == null)
                            {
                                break;
                            }
                            if ((ballA.tag == 0 && ballB.tag == 1) || (ballA.tag == 1 && ballB.tag == 2) || (ballA.tag == 1 && ballB.tag == 0) || (ballA.tag == 2 && ballB.tag == 1))
                            {
                                if (this.rectRect(ballA, ballB))
                                {
                                    ballA.Hit_State = true;
                                    ballB.Hit_State = true;
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