using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Firmament.Module
{
    public class Ball
    {
        public Rectangle rectangle;
        /**行 */
        public int row = 0;
        /**列 */
        public int col= 0;
        /**移动速度 */
        public double speed = 4;
        /**x轴速度 */
        public double xSpeed = 0;
        /**y轴速度 */
        public double ySpeed = 0;
        /**位置*/
        public double x = 0;
        public double y = 0;

        public double Width;
        public double Height;

        private bool hit_state = false;

        public Ball()
        {
            rectangle = new Rectangle();
            rectangle.Stroke = new SolidColorBrush(Colors.White);
            this.Width = rectangle.Width = 10;
            this.Height = rectangle.Height = 10;
        }

        public bool Hit_State {
            set {
                hit_state = value;
                if (hit_state)
                {
                    this.rectangle.Fill = new SolidColorBrush(Colors.Red);
                }
                else {
                    this.rectangle.Fill = new SolidColorBrush(Colors.Blue);
                }
            }
            get {
                return hit_state;
            }
        }


        //public Rect getRec()
        //{
        //    return new Rect(Canvas.GetLeft(rectangle), Canvas.GetTop(rectangle), 2, 5);
        //}
    }
}
