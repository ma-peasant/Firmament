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
        /**小球列表 */
        private List<BaseElement> ballList;
        private int interval = 100;
        QuadTree rootTree;
        //界面边缘
        private double maxWidth;
        private double maxHeight;
        public QuardTreeHelp(double width ,double height,int maxcount) {
            this.maxWidth = width - 10;
            this.maxHeight = height - 10;
            Common.ballList =  ballList = new List<BaseElement>();
            Common.rootTree =  rootTree = new QuadTree(new Rect(0, 0, width, height), maxcount);
        }

        public void InsertElement(BaseElement baseElement) {
            ballList.Add(baseElement);
            this.rootTree.Insert(baseElement);
            //this.canvas.Children.Add(ball.rectangle);
        }


        public  void InitUpdateTimer()
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
            //需要计算小球的矩形情况
            this.updateBallGrid();
            ////检查碰撞
            this.checkCollision();

        }

        private void updateBallGrid()
        {
            rootTree.Update(this.ballList.ToList<BaseElement>());

            //将小球置蓝色，重新计算小球所属行列的格子
            //for (int i = 0; i < this.ballList.Count; i++)
            //{
            //    BaseElement ball = this.ballList[i];
            //    Common.frmae.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
            //    {
            //        ball.Hit_State = false;
            //        //ball.rectangle.Fill = new SolidColorBrush(Colors.Blue);
            //    });
            //}
        }

        private void checkCollision()
        {

            for (int i = 0; i < this.ballList.Count; i++)
            {
                try
                {
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
                            if (ballA != ballB)
                            {
                                bool isCheck = false;
                                if (ballA.tag == 0 && ballB.tag == 1)
                                {
                                    isCheck = true;
                                }
                                if (ballA.tag == 1 && ballB.tag == 0) {
                                    isCheck = true;
                                }
                                if (ballA.tag == 3 && ballB.tag == 1) {
                                    isCheck = true;
                                }
                                if (isCheck && this.rectRect(ballA, ballB))
                                {
                                    Common.frmae.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate
                                    {
                                        ballA.Hit_State = true;
                                        ballB.Hit_State = true;
                                        //ballA.rectangle.Fill = new SolidColorBrush(Colors.Red);
                                        //ballB.rectangle.Fill = new SolidColorBrush(Colors.Red);

                                        Common.frmae.canvas.Children.Remove(ballA.control);
                                        Common.frmae.canvas.Children.Remove(ballB.control);
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
