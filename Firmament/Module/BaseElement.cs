using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Firmament.Module
{
   public abstract class BaseElement
    {
        public FrameworkElement control;
        public int tag = 0;  //标签，用于区分同类，同类相撞不处理

        public int flag = 0;
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

        public BaseElement()
        {
        }

        public virtual bool Hit_State
        {
            set
            {
                hit_state = value;
            }
            get
            {
                return hit_state;
            }
        }
    }
}
